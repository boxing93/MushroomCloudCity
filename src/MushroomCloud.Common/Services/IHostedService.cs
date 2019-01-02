using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MushroomCloud.Common.Services
{
   public interface IHostedService
    {
    // //
    // // Summary:
    // //     Triggered when the application host is ready to start the service.
    // Task StartAsync(CancellationToken cancellationToken);
    // //
    // // Summary:
    // //     Triggered when the application host is performing a graceful shutdown.
    // Task StopAsync(CancellationToken cancellationToken);
            Task RunAsync();

        void Run();
    }
}
