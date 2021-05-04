using StudyTestingEnvironment.Models.Identity;
using System;
using System.Threading.Tasks;

namespace StudyTestingEnvironment.Services.Identity
{
    public interface ISecurityService
    {
        /// <summary>
        /// Creates security token and cache it.
        /// </summary>
        /// <param name="userId"> User Id. </param>
        /// <param name="password"> User password. </param>
        /// <returns> <c>SecurityToken</c> instance if success. <c>null</c> if something went wrong. </returns>
        Task<SecureToken> CreateSecurityToken(Guid userId, string password);

        Task<bool> CheckSecurityToken(Guid userId, string securityToken);
    }
}