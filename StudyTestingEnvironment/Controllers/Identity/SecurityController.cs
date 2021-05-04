using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudyTestingEnvironment.Services.Identity;
using System;
using System.Threading.Tasks;

namespace StudyTestingEnvironment.Controllers
{
    [Route("api/security")]
    [ApiController]
    public class SecurityController : ControllerBase
    {
        private readonly ISessionManager _sessionManager;
        private readonly IUserHelper _userHelper;
        private readonly ISecurityService _securityService;
        private readonly IHttpContextAccessor _httpContextAccessor;        

        public SecurityController(ISessionManager sessionManager,
            IUserHelper userHelper,
            ISecurityService securityService,
            IHttpContextAccessor httpContextAccessor)
        {
            _sessionManager = sessionManager;
            _userHelper = userHelper;
            _securityService = securityService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("create-security-token")]
        [Authorize]
        public async Task<IActionResult> CreateSecurityToken([FromBody] string password)
        {
            var userId = _userHelper.GetCurrentUserId();
            if (userId == null) return NotFound();
            
            var securityToken = await _securityService.CreateSecurityToken(userId.Value, password);
            if (securityToken == null) return Forbid();

            // Set cookies for browsers.
            _httpContextAccessor.HttpContext.Response.Cookies.SetSecurityTokenCookie(securityToken.Token.ToString(),
                securityToken.Expires);

            // Return data instance for mobile/desktop clients.
            return Ok(securityToken);
        }

        [HttpGet("sessions")]
        [Authorize]
        public async Task<IActionResult> GetAllSessions()
        {
            var userId = _userHelper.GetCurrentUserId();
            var sessions = await _sessionManager.UserSessions(userId.Value);
            return Ok(sessions);
        }        

        [HttpPost("kill-session")]
        [Authorize(Policy = "secure")]
        public async Task<IActionResult> KillSession([FromQuery] Guid sessionId)
        {
            var userId = _userHelper.GetCurrentUserId();
            if (userId == null) return NotFound();
            var killingResult = await _sessionManager.RemoveUserSession(userId.Value, sessionId);
            return killingResult.IsSuccess ? Ok(killingResult) : NotFound(killingResult);
        }
    }
}