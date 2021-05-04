using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using StudyTestingEnvironment.Data.Contexts;
using StudyTestingEnvironment.Data.Models;
using StudyTestingEnvironment.Models.Identity;
using System;
using System.Threading.Tasks;

namespace StudyTestingEnvironment.Services.Identity
{
    public class SecurityService : ISecurityService
    {
        private readonly IDistributedCache _distributedCache;
        private readonly UserManager<User> _userManager;
        private readonly IdentityContext _identityContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SecurityService(IDistributedCache distributedCache,
            UserManager<User> userManager,
            IdentityContext identityContext,
            IHttpContextAccessor httpContextAccessor)
        {
            _distributedCache = distributedCache;
            _userManager = userManager;
            _identityContext = identityContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> CheckSecurityToken(Guid userId, string securityToken)
        {
            
            var key = string.Format("{0}:{1}", userId, securityToken);
            var currentIp = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
            var cachedIp = await _distributedCache.GetStringAsync(key);

            return currentIp.Equals(cachedIp);
        }

        public async Task<SecureToken> CreateSecurityToken(Guid userId, string password)
        {
            var user = await _identityContext.Users.FindAsync(userId);
            if (user == null)
            {
                return null;
            }

            if (!await _userManager.CheckPasswordAsync(user, password))
            {
                return null;
            }

            Guid secureToken;
            string key;            

            do
            {
                secureToken = Guid.NewGuid();
                key = string.Format("{0}:{1}", userId, secureToken);
            } 
            while (!string.IsNullOrEmpty(await _distributedCache.GetStringAsync(key)));

            var currentIp = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
            var expires = TimeSpan.FromMinutes(10);
            await _distributedCache.SetStringAsync(key,
                currentIp,
                new DistributedCacheEntryOptions 
                {
                    AbsoluteExpirationRelativeToNow = expires
                });

            return new SecureToken { Token = secureToken, Expires = DateTime.Now.Add(expires) };
        }
    }
}