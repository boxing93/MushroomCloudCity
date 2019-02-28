using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace MushroomCloud.Common.Emails
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailOptions = new EmailOptions();
            var section = _configuration.GetSection("email");
            section.Bind(emailOptions);
            using (var client = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = emailOptions.Username,
                    Password = emailOptions.Password
                };

                client.Credentials = credential;
                client.Host = emailOptions.Host;
                client.Port = emailOptions.Port;
                client.EnableSsl = true;

                using (var emailMessage = new MailMessage())
                {
                    emailMessage.To.Add(new MailAddress(email));
                    emailMessage.From = new MailAddress(emailOptions.Username);
                    emailMessage.Subject = subject;
                    emailMessage.Body = message;
                    client.Send(emailMessage);
                }
            }
            await Task.CompletedTask;
        }
    }
}