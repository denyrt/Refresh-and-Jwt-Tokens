using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using StudyTestingEnvironment.Data.Contexts;
using StudyTestingEnvironment.Data.Models;
using StudyTestingEnvironment.Models.Common;
using StudyTestingEnvironment.Models.Identity;
using StudyTestingEnvironment.Services.Identity.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudyTestingEnvironment.Services.Identity
{
    public class SessionManager : ISessionManager
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISessionsRepository _sessionsRepository;
        private readonly IdentityContext _identityContext;
        private readonly IUserHelper _userHelper;

        public SessionManager(IHttpContextAccessor httpContextAccessor,
            ISessionsRepository sessionsRepository,
            IdentityContext identityContext,
            IUserHelper userHelper)
        {
            _httpContextAccessor = httpContextAccessor;
            _sessionsRepository = sessionsRepository;
            _identityContext = identityContext;
            _userHelper = userHelper;
        }

        public async Task<RefreshSession> CreateRefreshSession(User user, string fingerPrint)
        {
            var session = new RefreshSession
            {            
                UserAgent = _httpContextAccessor.HttpContext.Request.Headers["User-Agent"].ToString() ?? string.Empty,
                FingerPrint = fingerPrint,
                IpAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString(),
                ExpiresInUTC = DateTime.UtcNow.AddDays(60)
            };

            var createResult = await _sessionsRepository.CreateSessionAsync(user.Id, session);            
            return createResult;
        }

        public async Task RemoveAllSessions(Guid userId)
        {
            await _sessionsRepository.DeleteUserSessions(userId);            
        }

        public async Task<OperationResult> RemoveUserSession(Guid userId, Guid sessionId)
        {
            var resultFactory = new ResultFactory();
            var session = await _sessionsRepository.FindSessionAsync(userId, sessionId);
            if (session == null)
            {
                return resultFactory.CreateOperationResult(false, 
                    "Session not found.");
            }

            var remove = await _sessionsRepository.DeleteSessionAsync(sessionId);

            if (remove.Value != null)
            {                
                return resultFactory.CreateOperationResult(true, "Success removed");
            }
            else
            {
                return resultFactory.CreateOperationResult(false, "Error while removing session.");
            }            
        }

        public async Task<RefreshSession> RemoveCurrentSession(string fingerPrint)
        {
            Guid.TryParse(_httpContextAccessor.HttpContext.Request.GetRefreshToken(), out var sessionId);
            var session = await _sessionsRepository.FindSessionAsync(sessionId);
            if (session == null) return null;

            var currentUser = await _userHelper.GetCurrentUser();
            if (CheckForHackingAttempt(fingerPrint, currentUser, session))
            {
                return null;
            }            

            var removeResult = await _sessionsRepository.DeleteSessionAsync(sessionId);
            return removeResult.Value;
        }

        public async Task<IReadOnlyCollection<RefreshSession>> UserSessions(Guid userId)
        {
            var sessions = await _sessionsRepository.GetUserSessions(userId);
            return sessions.ToArray();
        }

        public bool CheckForHackingAttempt(string fingerPrint, User user, RefreshSession session)
        {
            var currentUserAgent = _httpContextAccessor.HttpContext.Request.Headers["User-Agent"];
            var validatedAttempt = fingerPrint == session.FingerPrint
                      && currentUserAgent == session.UserAgent;

            if (user.UseIpBinding)
            {
                var currentIp = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
                validatedAttempt = validatedAttempt && (session.IpAddress == currentIp);
            }

            return !validatedAttempt;
        }
    }
}