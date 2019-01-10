using System;
using System.Threading.Tasks;
using MushroomCloud.Common.Commands;
using MushroomCloud.Common.Commands.ActivitiesCommand;
using MushroomCloud.Common.Events;
using MushroomCloud.Common.Events.ActivityEvents;
using RawRabbit;

namespace MushroomCloud.Services.Activities.Handlers
{
    public class CreateActivityHandler : ICommandHandler<CreateActivity>
    {
        private readonly IBusClient _busClient;
        public CreateActivityHandler(IBusClient busClient)
        {
            _busClient = busClient;
        }

        public async Task HandleAsync(CreateActivity command)
        {
            Console.WriteLine($"Creating activity: {command.Name}");
            await _busClient.PublishAsync(new ActivityCreated(command.Id,command.UserId,command.Name,command.Category,command.Description,command.CreatedAt));
        }
    }
}