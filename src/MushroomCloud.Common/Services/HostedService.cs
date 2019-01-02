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
        private Task _executingTask;
        private CancellationTokenSource _cts;

        public HostedService(IWebHost webHost)
        {
            _webHost = webHost;
        }
        public Task RunAsync() => _webHost.RunAsync();

        public void Run() => _webHost.Run();
        // protected Task ExecuteAsync(CancellationToken cancellationToken);

        // public Task StartAsync(CancellationToken cancellationToken)
        // {
        //     // Create a linked token so we can trigger cancellation outside of this token's cancellation
        //     _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

        //     // Store the task we're executing
        //     _executingTask = ExecuteAsync(_cts.Token);

        //     // If the task is completed then return it, otherwise it's running
        //     return _executingTask.IsCompleted ? _executingTask : Task.CompletedTask;
        // }

        // public async Task StopAsync(CancellationToken cancellationToken)
        // {
        //     // Stop called without start
        //     if (_executingTask == null)
        //     {
        //         return;
        //     }

        //     // Signal cancellation to the executing method
        //     _cts.Cancel();

        //     // Wait until the task completes or the stop token triggers
        //     await Task.WhenAny(_executingTask, Task.Delay(-1, cancellationToken));

        //     // Throw if cancellation triggered
        //     cancellationToken.ThrowIfCancellationRequested();
        // }

        public static HostBuilder Create<TStartup>(string[] args) where TStartup : class
        {
            Console.Title = typeof(TStartup).Namespace;
            var config = new ConfigurationBuilder()
                .AddEnvironmentVariables().AddCommandLine(args).Build();

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