using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MushroomCloud.Common.Commands.IdentityCommands;
using MushroomCloud.Common.Emails;
using MushroomCloud.Common.Exceptions;
using MushroomCloud.Services.Identity.Domain.Models;
using MushroomCloud.Services.Identity.Domain.Repositories;
using MushroomCloud.Services.Identity.Services;
using RawRabbit;

namespace MushroomCloud.Services.Identity.Controllers
{
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly IUserRepository<User> _userRepository;
        private readonly IBusClient _busClient;
        private readonly UserManager<User> _userManager;
        private readonly ILogger _logger;
        private readonly IEmailService _emailService;
        private string _token = null;

        public AccountController(IUserService userService, IBusClient busClient, IUserRepository<User> userRepository, ILogger<AccountController> logger, IEmailService emailService, UserManager<User> userManager)
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
        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ResetPasswordLink([FromBody]ResetPasswordCommand command)
        {
            var user = await _userManager.FindByEmailAsync(command.Email);

            if (ModelState.IsValid)
            {
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user))) throw new MushroomCloudException("asassasassa");

                _token = null;
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                _token = token;

                var callbackUrl = Url.Action("ForgotPassword", "Account", new { userId = user.Id, code = token }, protocol: HttpContext.Request.Scheme);
                _logger.LogError($"User: '{user.Email}' was created with name: '{user.UserName}'.");

                await _emailService.SendEmailAsync(user.Email, "Reset Password",
           $"Please reset your password by clicking here: <a href='{callbackUrl}'>link</a>");
                await _busClient.PublishAsync(command);
                return View("ForgotPasswordConfirmation");
            }

            return Accepted();
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody]CreateUser command)
        {
            await _userService.RegisterAsync(command.Email, command.Password, command.Email);
            return Accepted($"User: {command.Email}, was registered!");
        }

        //To Do: Logger
        [HttpGet("ForgotPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword()
        {
            await Task.CompletedTask;
            return View();
        }
    }
}

