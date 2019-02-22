using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        private readonly IUserRepository _userRepository;
        private readonly IBusClient _busClient;
        private readonly UserManager<User> _userManager;
        private readonly ILogger _logger;
        private readonly IEmailService _emailService;

        public AccountController(IUserService userService, IBusClient busClient, IUserRepository userRepository, ILogger logger, IEmailService emailService)
        {
            _busClient = busClient;
            _userService = userService;
            _userRepository = userRepository;
            _logger = logger;
            _emailService = emailService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthenticateUser command)
        {
            await _busClient.PublishAsync(command);
            return Accepted();
        }
        //To Do: Logger
        [HttpPost("/password/link")]
        [AllowAnonymous]
        public async Task<IActionResult> SendResetPasswordLink([FromBody]User user)
        {
            if (ModelState.IsValid)
            {
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                    return NotFound();
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = token }, protocol: HttpContext.Request.Scheme);
                await _emailService.SendEmailAsync(user.Email, "Reset Password",
           $"Please reset your password by clicking here: <a href='{callbackUrl}'>link</a>");
                return View("ForgotPasswordConfirmation");
            }
            return Accepted();
        }
        //To Do: Logger
        [HttpPost("/password/reset")]
        [AllowAnonymous]
        public IActionResult ResetPassword(ResetPasswordViewModel obj)
        {
            IdentityUser user = _userManager.
                         FindByNameAsync(obj.UserName).Result;

            IdentityResult result = _userManager.ResetPasswordAsync
                      (user, obj.Token, obj.Password).Result;
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
    }
}

