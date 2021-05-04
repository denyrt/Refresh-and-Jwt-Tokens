using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using StudyTestingEnvironment.Requirements;
using StudyTestingEnvironment.Services.Identity;
using System.Threading.Tasks;

namespace StudyTestingEnvironment.Handlers
{
    public class SecureTokenHandler : AuthorizationHandler<SecureTokenRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISecurityService _securityService;
        private readonly IUserHelper _userHelper;

        public SecureTokenHandler(IHttpContextAccessor httpContextAccessor,
            ISecurityService securityService,
            IUserHelper userHelper)
        {
            _httpContextAccessor = httpContextAccessor;
            _securityService = securityService;
            _userHelper = userHelper;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
            SecureTokenRequirement requirement)
        {
            var secureToken = _httpContextAccessor.HttpContext.Request.GetSecurityTokenCookie();
            var userId = _userHelper.GetCurrentUserId();

            if (userId == null || string.IsNullOrEmpty(secureToken))
            {
                context.Fail();                
            }

            if (!await _securityService.CheckSecurityToken(userId.Value, secureToken))
            {
                context.Fail();
            }

            context.Succeed(requirement);
        }
    }
}