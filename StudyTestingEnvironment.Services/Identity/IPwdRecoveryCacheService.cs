using System;
using System.Threading.Tasks;

namespace StudyTestingEnvironment.Services.Identity
{
    /// <summary>
    /// Service for temporary caching mail of user to protect from spam using password recovery.
    /// </summary>
    public interface IPwdRecoveryCacheService
    {
        Task<DateTime?> GetMailExpiresDateUtc(string mail);

        Task<double> CacheMail(string mail);
    }
}