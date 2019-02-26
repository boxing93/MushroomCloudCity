using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MushroomCloud.Common.Commands.IdentityCommands;
using MushroomCloud.Common.Emails;
using MushroomCloud.Services.Identity.Domain.Models;
using MushroomCloud.Services.Identity.Domain.Repositories;
using MushroomCloud.Services.Identity.Services;
using RawRabbit;

namespace MushroomCloud.Services.Identity.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly IUserRepository<User> _userRepository;
        private readonly IBusClient _busClient;
        private readonly UserManager<User> _userManager;
        private readonly ILogger _logger;
        private readonly IEmailService _emailService;
        private string _token = null;

        public AccountController(IUserService userService, IBusClient busClient, IUserRepository<User> userRepository, ILogger<AccountController> logger, IEmailService emailService,UserManager<User> userManager)
        {
            _busClient = busClient;
            _userService = userService;
            _userRepository = userRepository;
            _logger = logger;
            _emailService = emailService;
            _userManager = userManager;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthenticateUser command)
        {
            await _busClient.PublishAsync(command);
            return Accepted();
        }
        //To Do: Logger
        [HttpPost("/Password/GetLink")]
        [AllowAnonymous]
        public async Task<IActionResult> SendResetPasswordLink([FromBody]User user)
        {
            _logger.LogInformation($"############## START ###############");
            if (ModelState.IsValid)
            {
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user))) return NotFound();
                _token = null;
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                _token = token;
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = token }, protocol: HttpContext.Request.Scheme);
                _logger.LogError($"User: '{user.Email}' was created with name: '{user.UserName}'.");
                           _logger.LogDebug(callbackUrl);
                await _emailService.SendEmailAsync(user.Email, "Reset Password",
           $"Please reset your password by clicking here: <a href='{callbackUrl}'>link</a>");
                return View("ForgotPasswordConfirmation");
            }
            return Accepted();
        }
        //To Do: Logger
        [HttpPost("/Password/Reset")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPasswordAsync([FromBody]ResetPasswordCommand command)
        {
            var user = await _userManager.FindByEmailAsync(command.Email);
            var result = await _userManager.ResetPasswordAsync(user, _token, command.Password);

            if (result.Succeeded)
            {
                ViewBag.Message = "Password reset successful!";
                return View("Success");
            }
            else
            {
                ViewBag.Message = "Error while resetting the password!";
                return View("Error");
            }
        }
        [HttpGet("test")]
        public IActionResult Test()
        {
            _logger.LogInformation($"asdsdadadasdsaasdadsdsadsadsadsa");
            return Accepted();
        }
    }
}

