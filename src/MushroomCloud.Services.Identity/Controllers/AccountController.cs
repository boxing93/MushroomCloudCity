using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MushroomCloud.Common.Commands.IdentityCommands;
using MushroomCloud.Services.Identity.Services;
using RawRabbit;

namespace MushroomCloud.Services.Identity.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly IUserService _userService;

        private readonly IBusClient _busClient;

        public AccountController(IUserService userService, IBusClient busClient)
        {
            _busClient = busClient;
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthenticateUser command)
        {
            await _busClient.PublishAsync(command);
            return Accepted();
        }
        //Json(await _userService.LoginAsync(command.Email, command.Password));
    }
}

