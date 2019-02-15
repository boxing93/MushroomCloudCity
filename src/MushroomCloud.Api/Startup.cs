using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MushroomCloud.Api.Handlers;
using MushroomCloud.Api.Repositories;
using MushroomCloud.Common.Auth;
using MushroomCloud.Common.Commands;
using MushroomCloud.Common.Commands.ActivitiesCommand;
using MushroomCloud.Common.Emails;
using MushroomCloud.Common.Events;
using MushroomCloud.Common.Events.ActivityEvents;
using MushroomCloud.Common.Mongo;
using MushroomCloud.Common.RabbitMq;
using MushroomCloud.Services.Activities.Domain.Repository;
using IActivityRepository = MushroomCloud.Api.Repositories.IActivityRepository;

namespace MushroomCloud.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddMongoDb(Configuration);
            services.AddRabbitMq(Configuration);
            services.AddJwt(Configuration);
            services.AddSingleton<IEventHandler<ActivityCreated>,ActivityCreatedHandler>();
            services.AddScoped<IActivityRepository,ActivityRepository>();
            services.AddEmailClient(Configuration);
            return services.BuildServiceProvider();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.ApplicationServices.GetService<IDatabaseInitializer>().InitializeAsync();
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
