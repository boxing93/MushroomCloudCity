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
        private readonly SignInManager<User> _signInManager;

        public AccountController(IUserService userService, IBusClient busClient, IUserRepository<User> userRepository, ILogger<AccountController> logger, IEmailService emailService, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _busClient = busClient;
            _userService = userService;
            _userRepository = userRepository;
            _logger = logger;
            _emailService = emailService;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost("login")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([FromBody] AuthenticateUser command)
        {
            await _busClient.PublishAsync(command);
            return Accepted();
        }
        //To Do: Logger
        [HttpPost("ForgotPassword")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPasswordLink([FromBody]ResetPasswordCommand command)
        {
            var user = await _userManager.FindByEmailAsync(command.Email);

            if (ModelState.IsValid)
            {
                if (user == null /*|| !(await _userManager.IsEmailConfirmedAsync(user))*/) throw new MushroomCloudException("User not found or not confirmed!");
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                var callbackUrl = Url.Action("ForgotPassword", "Account", new { userId = user.Id, code = token }, protocol: HttpContext.Request.Scheme);
                _logger.LogError($"User: '{user.Email}' was created with name: '{user.UserName}'.");
                await _busClient.PublishAsync(command);
                await _emailService.SendEmailAsync(user.Email, "Reset Password",
           $"Please reset your password by clicking here: <a href='{callbackUrl}'>link</a>");
                return View("ForgotPasswordConfirmation");
            }

            return Accepted();
        }

        [HttpPost("Register")]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody]CreateUser command)
        {
            await _userService.RegisterAsync(command.Email, command.Password, command.Email);
            return Accepted($"User: {command.Email}, was registered!");
        }

        //To Do: Logger
        [HttpGet("ForgotPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(string userId,string code)
        {
            var result = await _userService.ConfirmEmailAsync(userId,code);
            return View(result.Succeeded ? "ForgotPassword" : "Error");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation(4, "User logged out.");
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

    }
}

