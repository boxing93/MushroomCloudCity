using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MushroomCloud.Api.Repositories;
using MushroomCloud.Common.Commands.ActivitiesCommand;
using System.Linq;
using RawRabbit;

namespace MushroomCloud.Api.Controllers
{
    [Route("[controller]")]
    // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ActivitiesController : Controller
    {
        private readonly IBusClient _busClient;
        private readonly IActivityRepository _activityRepository;

        public ActivitiesController(IBusClient busClient, IActivityRepository activityRepository)
        {
            _busClient = busClient;
            _activityRepository = activityRepository;
        }

        [HttpGet("")]
        public async Task<IActionResult> Get()
        {
            var activities = await _activityRepository.BrowseAsync(Guid.Parse(User.Identity.Name));

            return Json(activities.Select(x => new {x.Id, x.Name}));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var activity = await _activityRepository.GetAsync(id);
            if(activity == null)
            {
                return NotFound();
            }
            if (activity.UserId != Guid.Parse(User.Identity.Name))
            {
                return Unauthorized();
            }

            return Json(activity);
        }

        [HttpPost("")]
        public async Task<IActionResult> Post([FromBody] CreateActivity command)
        {
            command.Id = Guid.NewGuid();
            // command.UserId = Guid.Parse(User.Identity.Name);
            command.UserId = Guid.NewGuid();
            command.CreatedAt = DateTime.UtcNow;
            await _busClient.PublishAsync(command);
            return Accepted($"activities/{command.Id}");
        }

      
    }

}