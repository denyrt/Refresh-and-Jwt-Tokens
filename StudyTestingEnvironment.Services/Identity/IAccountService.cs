using StudyTestingEnvironment.Models.Common;
using StudyTestingEnvironment.Models.Identity;
using System.Threading.Tasks;

namespace StudyTestingEnvironment.Services.Identity
{
    public interface IAccountService
    {
        Task<bool> ConfirmRegistry(ConfirmRegistryRequest request);

        Task<bool> SendConfirmRegistryLetter(string email);

        Task<ForgotPasswordResult> ForgotPassword(ForgotPasswordRequest request);

        Task<OperationResult> ResetPassword(ResetPasswordRequest request);
    }
}