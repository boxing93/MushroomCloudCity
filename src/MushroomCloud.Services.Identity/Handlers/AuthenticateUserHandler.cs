using Microsoft.Extensions.Logging;
using MushroomCloud.Common.Commands;
using MushroomCloud.Common.Commands.IdentityCommands;
using MushroomCloud.Common.Events.IdentityEvents;
using MushroomCloud.Common.Exceptions;
using MushroomCloud.Services.Identity.Services;
using RawRabbit;
using System;
using System.Threading.Tasks;

namespace MushroomCloud.Services.Identity.Handlers
{
    public class AuthenticateUserHandler : ICommandHandler<AuthenticateUser>
    {
        private readonly ILogger _logger;
        private readonly IBusClient _busClient;
        private readonly IUserService _userService;

        public AuthenticateUserHandler(IBusClient busClient,IUserService userService,ILogger<AuthenticateUser> logger)
        {
            _busClient = busClient;
            _userService = userService;
            _logger = logger;
        }

        public async Task HandleAsync(AuthenticateUser command)
        {
            _logger.LogInformation($"Authenticating user: '{command.Email}'.");
            try
            {
                await _userService.LoginAsync(command.Email, command.Password);
                await _busClient.PublishAsync(new UserAuthenticated(command.Email));
                _logger.LogInformation($"User: '{command.Email}' was authenticated.");
                return;
            }
            catch (MushroomCloudException ex)
            {
                _logger.LogError(ex, ex.Message);
                await _busClient.PublishAsync(new UserAuthenticatedRejected(command.Email,
                    ex.Code, ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                await _busClient.PublishAsync(new UserAuthenticatedRejected(command.Email,
                    ex.Message, "error"));
            }
        }
    }
}
