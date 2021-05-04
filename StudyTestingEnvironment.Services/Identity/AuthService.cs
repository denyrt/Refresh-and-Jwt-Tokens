using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using StudyTestingEnvironment.Data.Contexts;
using StudyTestingEnvironment.Data.Models;
using StudyTestingEnvironment.Models.Identity;
using StudyTestingEnvironment.Models.Options;
using StudyTestingEnvironment.Services.Identity.Repositories;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace StudyTestingEnvironment.Services.Identity
{
    public class AuthService : IAuthService
    {
        private readonly ISessionManager _sessionManager;
        private readonly IJwtTokenHelper _jwtTokenHelper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailService _emailService;
        private readonly IUserHelper _userHelper;
        private readonly IdentityContext _identityContext;
        private readonly AspNetUserManager<User> _userManager;
        private readonly AspNetRoleManager<Role> _roleManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ISessionsRepository _sessionsRepository;

        private readonly ResultFactory _authResultFactory = new ResultFactory();
        private readonly FrontendRoutingOptions _frontendRoutingOptions;

        public AuthService(ISessionManager sessionManager,
            IJwtTokenHelper jwtTokenHelper,
            IHttpContextAccessor httpContextAccessor,
            IEmailService emailService,
            IUserHelper userHelper,
            IdentityContext identityContext,
            AspNetUserManager<User> userManager,
            AspNetRoleManager<Role> roleManager,
            SignInManager<User> signInManager,
            IOptions<FrontendRoutingOptions> frontendRoutingOptions,
            ISessionsRepository sessionsRepository)
        {
            _sessionManager = sessionManager;
            _jwtTokenHelper = jwtTokenHelper;
            _httpContextAccessor = httpContextAccessor;
            _emailService = emailService;
            _userHelper = userHelper;
            _identityContext = identityContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _frontendRoutingOptions = frontendRoutingOptions.Value;
            _sessionsRepository = sessionsRepository;
        }        

        public async Task<LoginResult> Login(LoginRequest loginRequest)
        {            
            var user = await _userManager.FindByNameAsync(loginRequest.LoginOrEmail) ??
                       await _userManager.FindByEmailAsync(loginRequest.LoginOrEmail);

            if (user == null) return _authResultFactory.LoginFailed();

            var signInResult = await _signInManager.CheckPasswordSignInAsync(user, loginRequest.Password, false);
            if (!signInResult.Succeeded) return _authResultFactory.LoginFailed();

            var userRoles = await _userManager.GetRolesAsync(user);
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, string.Join(",", userRoles))
            };

            var refreshSession = await _sessionManager.CreateRefreshSession(user, loginRequest.FingerPrint);
            if (refreshSession == null)
            {
                return _authResultFactory.LoginFailed();
            }

            var accessToken = _jwtTokenHelper.CreateJwtToken(claims);
            var refreshToken = refreshSession.Id.ToString();

            _httpContextAccessor.HttpContext.Response.Cookies.ResetRefreshTokenCookie(refreshToken,
                refreshSession.ExpiresInUTC);

            return _authResultFactory.LoginSuccess(accessToken, refreshToken, refreshSession.ExpiresInUTC);
        }
       
        public async Task<bool> Logout(string fingerPrint)
        {
            var session = await _sessionManager.RemoveCurrentSession(fingerPrint);
            _httpContextAccessor.HttpContext.Response.Cookies.RemoveRefreshTokenCookie();
            return session != null;
        }

        public async Task LogoutFromAllDevices()
        {
            var userId = _userHelper.GetCurrentUserId();
            if (userId != null)
            {
                await _sessionManager.RemoveAllSessions(userId.Value);
                _httpContextAccessor.HttpContext.Response.Cookies.RemoveRefreshTokenCookie();
            }
        }

        public async Task<RefreshTokenResult> RefreshToken(string fingerPrint)
        {
            var resultFactory = new ResultFactory();
            var refreshToken = _httpContextAccessor.HttpContext.Request.GetRefreshToken();
            if (!Guid.TryParse(refreshToken, out var refreshTokenId))
            {
                return resultFactory.CreateRefreshTokenExpiredResult();
            }

            var session = await _sessionsRepository.FindSessionAsync(refreshTokenId);
            if (session == null)
            {
                return resultFactory.CreateRefreshTokenExpiredResult();
            }

            var removed = await _sessionsRepository.DeleteSessionAsync(session.Id);
            var userId = removed.Key;
            var removedSession = removed.Value;
            var user = await _identityContext.Users.FindAsync(userId);
            if (user == null)
            {
                return resultFactory.CreateRefreshTokenExpiredResult();
            }

            _httpContextAccessor.HttpContext.Response.Cookies.RemoveRefreshTokenCookie();

            if (_sessionManager.CheckForHackingAttempt(fingerPrint, user, session))
            {
                return resultFactory.CreateRefreshTokenExpiredResult();
            }

            if (DateTime.UtcNow > session.ExpiresInUTC)
            {                
                return resultFactory.CreateRefreshTokenExpiredResult();
            }                       
           
            var roles = await _userManager.GetRolesAsync(user);
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, string.Join(",", roles))
            };

            var refreshSession = new RefreshSession
            {               
                ExpiresInUTC = session.ExpiresInUTC,
                FingerPrint = session.FingerPrint,
                IpAddress = session.IpAddress,
                UserAgent = session.UserAgent                
            };

            var added = await _sessionsRepository.CreateSessionAsync(userId, refreshSession);
            await _identityContext.SaveChangesAsync();

            _httpContextAccessor.HttpContext.Response.Cookies.SetRefreshTokenCookie(added.Id.ToString(),
                added.ExpiresInUTC);   

            var accessToken = _jwtTokenHelper.CreateJwtToken(claims);
            return resultFactory.CreateRefreshTokenResultSuccess(accessToken, added.Id.ToString());            
        }

        public async Task<RegistryResult> Registry(RegistryRequest request)
        {
            if (await _userManager.FindByEmailAsync(request.Email) != null)
            {
                return _authResultFactory.RegistryFailed("This Email already exists.");
            }
            
            if (await _userManager.FindByNameAsync(request.Login) != null)
            {
                return _authResultFactory.RegistryFailed("This login already exists.");
            }

            var registryUser = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Patronymic = request.Patronymic,
                UserName = request.Login,
                Email = request.Email
            };

            var registryResult = await _userManager.CreateAsync(registryUser, request.Password);
            if (!registryResult.Succeeded)
            {
                return _authResultFactory.RegistryFailed();
            }
            
            var role = request.Role switch
            {
                RegistryRole.Student => "Student",
                RegistryRole.Teacher => "Teacher",
                _ => string.Empty
            };

            var addRoleResult = await _userManager.AddToRoleAsync(registryUser, role);
            if (!addRoleResult.Succeeded)
            {
                return _authResultFactory.RegistryFailed();
            }
            
            var confirmToken = await _userManager.GenerateEmailConfirmationTokenAsync(registryUser);            
            var confirmUrl = string.Format("{0}?token={1}&email={2}",
                _frontendRoutingOptions.ConfirmEmailUrl,
                confirmToken,
                registryUser.Email);
            await _emailService.SendMessage(registryUser.Email,
                 subject: "Confirm Registry",
                 content: "Open this link in browser to confirm you mail: " + HttpUtility.HtmlEncode(confirmUrl),
                 htmlContent: $"<html><body>Click <a href=\"{confirmUrl}\">here</a> to confirm your mail.</html></body>");

            return _authResultFactory.SuccessRegistry(registryUser.Email);
        }
    }
}