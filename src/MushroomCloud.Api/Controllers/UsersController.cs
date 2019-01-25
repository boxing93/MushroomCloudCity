using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MushroomCloud.Common.Commands.ActivitiesCommand;
using MushroomCloud.Common.Commands.IdentityCommands;
using MushroomCloud.Common.Events;
using MushroomCloud.Common.Events.ActivityEvents;
using RawRabbit;

namespace MushroomCloud.Api.Controllers
{
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IBusClient _busClient;
        public UsersController(IBusClient busClient)
        {
            _busClient = busClient;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Post([FromBody] CreateUser command)
        {
            await _busClient.PublishAsync(command);
            
            return Accepted();
        }
    }
}