using System.Threading.Tasks;

namespace MushroomCloud.Common.Emails
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email,string subject,string message);
    }
}