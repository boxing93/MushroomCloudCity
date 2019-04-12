using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MushroomCloud.Common.Services;

namespace MushroomCloud.Services.SignalR
{
    public class Program
    {
        public static void Main(string[] args)
        {
            HostedService.Create<Startup>(args)
            .UseRabbitMq()
            .Build()
            .RunAsync();
        }
    }
}
