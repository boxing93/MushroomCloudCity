using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using MushroomCloud.Common.Commands;
using MushroomCloud.Common.Events;
using MushroomCloud.Common.RabbitMq;
using RawRabbit;

namespace MushroomCloud.Common.Services
{
    public class HostedService : IHostedService
    {
        private readonly IWebHost _webHost;
        
        public HostedService(IWebHost webHost)
        {
            _webHost = webHost;
        }
        public Task RunAsync() => _webHost.RunAsync();

        public void Run() => _webHost.Run();

        public static HostBuilder Create<TStartup>(string[] args) where TStartup : class
        {
            Console.Title = typeof(TStartup).Namespace;
            var config = new ConfigurationBuilder()
                .AddEnvironmentVariables().AddCommandLine(args).AddJsonFile("appsettings.json", optional: true).Build();

            var webHostBuilder = WebHost.CreateDefaultBuilder(args)
            .UseConfiguration(config).UseStartup<TStartup>();
            return new HostBuilder(webHostBuilder.Build());
        }


        public class BusBuilder : BuilderBase
        {
            private readonly IWebHost _webHost;

            private IBusClient _bus;

            public BusBuilder(IWebHost webHost, IBusClient busClient)
            {
                _webHost = webHost;
                _bus = busClient;
            }

            public BusBuilder SubscribeToCommand<TCommand>() where TCommand : ICommand
            {
                var handler = (ICommandHandler<TCommand>)_webHost.Services.GetService(typeof(ICommandHandler<TCommand>));
                _bus.WithCommandHandlerAsync(handler);
                return this;
            }

            public BusBuilder SubscribeToEvent<TEvent>() where TEvent : IEvent
            {
                var handler = (IEventHandler<TEvent>)_webHost.Services.GetService(typeof(IEventHandler<TEvent>));
                _bus.WithEventHandlerAsync(handler);
                return this;
            }
            public override HostedService Build()
            {
                return new HostedService(_webHost);
            }
        }
        public abstract class BuilderBase
        {
            public abstract HostedService Build();
        }

        public class HostBuilder : BuilderBase
        {
            private readonly IWebHost _webHost;

            private IBusClient _bus;
            public HostBuilder(IWebHost webHost)
            {
                _webHost = webHost;
            }

            public BusBuilder UseRabbitMq()
            {
                _bus = (IBusClient)_webHost.Services.GetService(typeof(IBusClient));
                return new BusBuilder(_webHost, _bus);
            }
            public override HostedService Build()
            {
                return new HostedService(_webHost);
            }
        }
    }
}