using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MushroomCloud.Common.Auth;
using MushroomCloud.Common.Commands;
using MushroomCloud.Common.Commands.IdentityCommands;
using MushroomCloud.Common.Emails;
using MushroomCloud.Common.Mongo;
using MushroomCloud.Common.RabbitMq;
using MushroomCloud.Services.Activities.Domain.Services;
using MushroomCloud.Services.Identity.Domain.Models;
using MushroomCloud.Services.Identity.Domain.Repositories;
using MushroomCloud.Services.Identity.Handlers;
using MushroomCloud.Services.Identity.Services;

namespace MushroomCloud.Services.Identity
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
            services.AddLogging();
            services.AddMongoDb(Configuration);
            services.AddRabbitMq(Configuration);
            services.AddJwt(Configuration);
            services.AddScoped<IJwtHandler, JwtHandler>();
            services.AddScoped<ICommandHandler<CreateUser>,CreateUserHandler>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IEncrypter, Encrypter>();
            services.AddScoped<IUserRepository<User>, UserRepository<User>>();
            services.AddScoped<IEmailService,EmailService>();
            services.AddIdentity<User, Role>()
                .AddMongoDbStores<User, Role, Guid>
                (
                   "mongodb://localhost:27017",
                    "MushroomCloud-Services-Users"
                )
                .AddDefaultTokenProviders();
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
            // Services.AddDefaultIdentity<IdentityUser>(config =>
            // {
            //     config.SignIn.RequireConfirmedEmail = true;
            // });
            app.UseAuthentication();
            app.ApplicationServices.GetService<IDatabaseInitializer>().InitializeAsync();

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
