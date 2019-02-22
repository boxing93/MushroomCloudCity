using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using MushroomCloud.Common.Commands;
using MushroomCloud.Common.Commands.IdentityCommands;
using MushroomCloud.Common.Events.IdentityEvents;
using MushroomCloud.Common.Exceptions;
using MushroomCloud.Services.Identity.Domain.Models;
using MushroomCloud.Services.Identity.Services;
using RawRabbit;

namespace MushroomCloud.Services.Identity.Handlers
{
    public class ResetPasswordHandler : ICommandHandler<ResetPasswordCommand>
    {
        private readonly ILogger _logger;
        private readonly IBusClient _busClient;
        private readonly IUserService _userService;

        public ResetPasswordHandler(IBusClient busClient,
            IUserService userService,
            ILogger<CreateUser> logger)
        {
            _busClient = busClient;
            _userService = userService;
            _logger = logger;
        }

        public async Task HandleAsync(ResetPasswordCommand command)
        {
            _logger.LogInformation($"Reset user password with email: '{command.Email}'.");
            try
            {
                await _busClient.PublishAsync(new PasswordReseted(command.Email));
                _logger.LogInformation($"User password with Email: '{command.Email}' was reseted.");
                return;
            }
            catch (MushroomCloudException ex)
            {
                _logger.LogError(ex, ex.Message);
                await _busClient.PublishAsync(new CreateUserRejected(command.Email,
                    ex.Message, ex.Code));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                await _busClient.PublishAsync(new CreateUserRejected(command.Email,
                    ex.Message, "error"));
            }
        }
    }
}