using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using StudyTestingEnvironment.Data.Models;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace StudyTestingEnvironment.Services.Identity
{
    public class UserHelper : IUserHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AspNetUserManager<User> _aspNetUserManager;

        public UserHelper(IHttpContextAccessor httpContextAccessor,
            AspNetUserManager<User> aspNetUserManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _aspNetUserManager = aspNetUserManager;
        }

        public Guid? GetCurrentUserId()
        {
            var userId = _httpContextAccessor.HttpContext.User.Claims
                .FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier).Value;

            if (userId == null) return null;

            Guid.TryParse(userId, out var id);
            return id;
        }

        public async Task<User> GetCurrentUser()
        {
            var userId = _httpContextAccessor.HttpContext.User.Claims
                .FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier);

            return userId == null ? null : await _aspNetUserManager.FindByIdAsync(userId.Value);
        }
    }
}
