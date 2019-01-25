using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MushroomCloud.Common.Commands;
using MushroomCloud.Common.Commands.ActivitiesCommand;
using MushroomCloud.Common.Events.ActivityEvents;
using MushroomCloud.Common.Exceptions;
using MushroomCloud.Services.Activities.Services;
using RawRabbit;

namespace MushroomCloud.Services.Activities.Handlers
{
    public class CreateActivityHandler : ICommandHandler<CreateActivity>
    {
        private readonly IBusClient _busClient;
        private readonly IActivityService _activityService;
        private readonly ILogger _logger;
        public CreateActivityHandler(IBusClient busClient,IActivityService activityService,ILogger logger)
        {
            _activityService = activityService;
            _logger = logger;
            _busClient = busClient;
        }

        public async Task HandleAsync(CreateActivity command)
        {
            _logger.LogInformation($"Creating activity: {command.Name}");
            try
            {
                await _activityService.AddAsync(command.Id, command.UserId, command.Category, command.Name, command.Description, command.CreatedAt);
                await _busClient.PublishAsync(new ActivityCreated(command.Id, command.UserId, command.Name, command.Category, command.Description, command.CreatedAt));
                return;
            }
            catch (MushroomCloudException ex)
            {
                await _busClient.PublishAsync(new RejectCreateActivity(command.UserId,command.Id,ex.Message,ex.Code));
                _logger.LogError(ex.Message);
            }
            catch (Exception ex)
            {
                await _busClient.PublishAsync(new RejectCreateActivity(command.UserId, command.Id, ex.Message, "error"));
                _logger.LogError(ex.Message);
            }
        }
    }
}