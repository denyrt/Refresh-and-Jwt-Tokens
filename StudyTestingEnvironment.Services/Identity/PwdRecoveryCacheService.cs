using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using StudyTestingEnvironment.Models.Options;
using System;
using System.Threading.Tasks;

namespace StudyTestingEnvironment.Services.Identity
{
    public class PwdRecoveryCacheService : IPwdRecoveryCacheService
    {
        private readonly IDistributedCache _distributedCache;
        private readonly PwdRecoveryCachingOptions _options;

        public PwdRecoveryCacheService(IDistributedCache distributedCache,
            IOptions<PwdRecoveryCachingOptions> options)
        {
            _distributedCache = distributedCache;
            _options = options.Value;
        }

        public async Task<DateTime?> GetMailExpiresDateUtc(string mail)
        {
            var expiredDate = await _distributedCache.GetStringAsync(mail);
            
            if (expiredDate == null)
            {
                return null;
            }

            if (!DateTime.TryParse(expiredDate, out var date))
            {
                return null;
            }

            return date;
        }        

        public async Task<double> CacheMail(string mail)
        {
            var expires = TimeSpan.FromSeconds(_options.ExpireSeconds);
            var dateTime = DateTime.Now.Add(expires);
            var options = new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = expires };
            await _distributedCache.SetStringAsync(mail, dateTime.ToLongTimeString(), options);
            return _options.ExpireSeconds;
        }
    }
}