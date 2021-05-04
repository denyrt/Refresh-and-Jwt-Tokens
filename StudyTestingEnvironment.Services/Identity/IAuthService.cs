using StudyTestingEnvironment.Models.Identity;
using System.Threading.Tasks;

namespace StudyTestingEnvironment.Services.Identity
{
    public interface IAuthService
    {
        Task<LoginResult> Login(LoginRequest loginRequest);

        Task<RegistryResult> Registry(RegistryRequest request);

        Task<bool> Logout(string fingerPrint);

        Task LogoutFromAllDevices();

        Task<RefreshTokenResult> RefreshToken(string fingerPrint);
    }
}