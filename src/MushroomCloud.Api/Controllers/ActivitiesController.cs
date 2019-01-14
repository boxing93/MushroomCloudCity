using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MushroomCloud.Common.Commands.ActivitiesCommand;
using RawRabbit;

namespace MushroomCloud.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ActivitiesController : ControllerBase
    {
        private readonly IBusClient _busClient;
        public ActivitiesController(IBusClient busClient) => _busClient = busClient;

        [HttpPost("")]
        public async Task<IActionResult> Post([FromBody] CreateActivity command)
        {
            command.Id = Guid.NewGuid();
            command.UserId = Guid.NewGuid();
            command.CreatedAt = DateTime.UtcNow;
            await _busClient.PublishAsync(command);
            return Accepted($"activities/{command.Id}");
        }

        [HttpGet("get")]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }
    }

}