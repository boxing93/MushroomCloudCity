using System;
using System.Threading.Tasks;
using MushroomCloud.Common.Events;
using MushroomCloud.Common.Events.ActivityEvents;

namespace MushroomCloud.Api.Handlers
{
    public class ActivityCreatedHandler : IEventHandler<ActivityCreated>
    {
        public async Task HandleAsync(ActivityCreated @event)
        {
            await Task.CompletedTask;
            Console.WriteLine($"Activity created: {@event.Name}");
        }
    }
}