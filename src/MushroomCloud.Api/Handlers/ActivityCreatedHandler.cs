using System;
using System.Threading.Tasks;
using MushroomCloud.Api.Models;
using MushroomCloud.Api.Repositories;
using MushroomCloud.Common.Events;
using MushroomCloud.Common.Events.ActivityEvents;

namespace MushroomCloud.Api.Handlers
{
    public class ActivityCreatedHandler : IEventHandler<ActivityCreated>
    {
        private readonly IActivityRepository _activityRepository;

        public ActivityCreatedHandler(IActivityRepository activityRepository)
        {
            _activityRepository = activityRepository;
        }
        public async Task HandleAsync(ActivityCreated @event)
        {
            Activity model = new Activity
            {
                Id = @event.Id,
                UserId = @event.UserId,
                Name = @event.Name
            };
            await _activityRepository.AddAsync(model);
            Console.WriteLine($"Activity created: {@event.Name}");
        }
    }
}