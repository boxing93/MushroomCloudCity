using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MushroomCloud.Common.Events.ActivityEvents;
using MushroomCloud.Common.Services;

namespace MushroomCloud.Services.Activities
{
    public class Program
    {
        public static void Main(string[] args)
        {
            HostedService.Create<Startup>(args)
                    .UseRabbitMq()
                    .SubscribeToEvent<ActivityCreated>()
                    .Build()
                    .Run();
        }
    }
}
