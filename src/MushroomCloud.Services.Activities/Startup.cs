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
using MushroomCloud.Common.Commands;
using MushroomCloud.Common.Commands.ActivitiesCommand;
using MushroomCloud.Common.Mongo;
using MushroomCloud.Common.RabbitMq;
using MushroomCloud.Services.Activities.Domain.Repository;
using MushroomCloud.Services.Activities.Handlers;
using MushroomCloud.Services.Activities.Repositories;
using MushroomCloud.Services.Activities.Services;

namespace MushroomCloud.Services.Activities
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddLogging();
            services.AddMongoDb(Configuration);
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IActivityRepository, ActivityRepository>();
            //services.AddScoped<IDatabaseSeeder, MongoSeeder>();
            services.AddRabbitMq(Configuration);
            services.AddSingleton<ICommandHandler<CreateActivity>, CreateActivityHandler>();
            services.AddScoped<IActivityService, ActivityService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //else
            //{
            //    app.UseHsts();
            //}

            //app.UseHttpsRedirection();
           app.ApplicationServices.GetService<IDatabaseInitializer>().InitializeAsync();
            app.UseMvc();
        }
    }
}
