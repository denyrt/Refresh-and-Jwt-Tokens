using System.Security.Claims;

namespace StudyTestingEnvironment.Services.Identity
{
    public interface IJwtTokenHelper
    {
        string CreateJwtToken(Claim[] claims);
    }
}