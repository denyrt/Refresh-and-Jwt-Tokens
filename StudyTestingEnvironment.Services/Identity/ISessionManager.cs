using StudyTestingEnvironment.Data.Models;
using StudyTestingEnvironment.Models.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudyTestingEnvironment.Services.Identity
{
    public interface ISessionManager
    {
        Task<RefreshSession> CreateRefreshSession(User user, string fingerPrint);

        Task<OperationResult> RemoveUserSession(Guid userId, Guid sessionId);

        Task RemoveAllSessions(Guid userId);

        Task<RefreshSession> RemoveCurrentSession(string fingerPrint);

        Task<IReadOnlyCollection<RefreshSession>> UserSessions(Guid userId);

        /// <summary>
        /// Check hacking attempt using sensetive data with user options.
        /// </summary>
        /// <param name="session"> Current session. </param>
        /// <returns> <c>true</c> when hacking attempt detected. Else <c>false</c>. </returns>
        bool CheckForHackingAttempt(string fingerPrint, User user, RefreshSession session);
    }
}