using MushroomCloud.Common.Commands.IdentityCommands;
using MushroomCloud.Common.Services;

namespace MushroomCloud.Services.Identity
{
    public class Program
    {
        public static void Main(string[] args)
        {
            HostedService.Create<Startup>(args)
            .UseRabbitMq()
            .SubscribeToCommand<CreateUser>()
            .SubscribeToCommand<AuthenticateUser>()
            .SubscribeToCommand<ResetPasswordCommand>()
            .SubscribeToCommand<AuthenticateUser>()
            .Build()
            .RunAsync();        
        }
    }
}
