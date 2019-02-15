using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace MushroomCloud.Common.Emails
{
    public static class Extensions
    {
        public static void AddEmailClient(this IServiceCollection service, IConfiguration configuration)
        {
            var emailOptions = new EmailOptions();
            var section = configuration.GetSection("email");
            section.Bind(emailOptions);
            service.AddScoped((serviceProvider) =>
            {
                return new SmtpClient()
                {
                    Host = configuration.GetValue<String>(emailOptions.Host),
                    Port = configuration.GetValue<int>(emailOptions.Port.ToString()),
                    Credentials = new NetworkCredential(
                                            configuration.GetValue<String>(emailOptions.Username),
                                            configuration.GetValue<String>(emailOptions.Password))
                };
            });
        }
    }
}
