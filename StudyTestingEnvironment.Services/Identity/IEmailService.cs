using System.Threading.Tasks;

namespace StudyTestingEnvironment.Services.Identity
{
    public interface IEmailService
    {
        Task<bool> SendMessage(string email,
                               string subject,
                               string content,
                               string htmlContent = null);
    }
}