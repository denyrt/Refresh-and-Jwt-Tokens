using ServiceStack.Redis;
using StudyTestingEnvironment.Data.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudyTestingEnvironment.Services.Identity.Repositories
{
    public class SessionsRepository : ISessionsRepository
    {
        private const string SessionInfix = "refreshsession";
        private readonly IRedisClientsManager _redisManagerPool;

        public SessionsRepository(IRedisClientsManager redisManagerPool)
        {
            _redisManagerPool = redisManagerPool;            
        }
        
        public async Task<RefreshSession> CreateSessionAsync(Guid userId, RefreshSession refreshSession)
        {
            await using var client = await _redisManagerPool.GetClientAsync();
            string keyPattern;

            do
            {
                refreshSession.Id = Guid.NewGuid();
                keyPattern = string.Format("*:{0}:{1}", SessionInfix, refreshSession.Id);                
            }
            while (await ContainsKeyPattern(client, keyPattern));

            var key = string.Format("{0}:{1}:{2}", userId, SessionInfix, refreshSession.Id);
            var created = await client.SetAsync(key, refreshSession, refreshSession.ExpiresInUTC);
            return created ? refreshSession : null;
        }

        public async Task<KeyValuePair<Guid, RefreshSession>> DeleteSessionAsync(Guid sessionId)
        {
            await using var client = await _redisManagerPool.GetClientAsync();
            var keyPattern = string.Format("*:{0}:{1}", SessionInfix, sessionId);
            var keys = await ToListAsync(client.GetKeysByPatternAsync(keyPattern));
            if (keys.Count == 0) return default;
            var entity = await client.GetAsync<RefreshSession>(keys.FirstOrDefault());
            await client.RemoveAsync(keys[0]);
            Guid.TryParse(keys[0].Split(':')[0], out var userId);
            return new KeyValuePair<Guid, RefreshSession>(userId, entity);
        }

        public async Task DeleteUserSessions(Guid userId)
        {
            await using var client = await _redisManagerPool.GetClientAsync();
            var keyPattern = string.Format("{0}:{1}:*", userId, SessionInfix);
            var keys = await ToListAsync(client.GetKeysByPatternAsync(keyPattern));
            await client.RemoveAllAsync(keys);
        }

        public async Task<RefreshSession> FindSessionAsync(Guid refreshSessionId)
        {
            await using var client = await _redisManagerPool.GetClientAsync();
            var keyPattern = string.Format("*:{0}:{1}", SessionInfix, refreshSessionId);
            var keys = await ToListAsync(client.GetKeysByPatternAsync(keyPattern));
            if (keys.Count == 0) return null;
            var find = await client.GetAsync<RefreshSession>(keys[0]);
            return find;
        }

        public async Task<RefreshSession> FindSessionAsync(Guid userId, Guid refreshSessionId)
        {
            await using var client = await _redisManagerPool.GetClientAsync();
            var keyPattern = string.Format("{0}:{1}:{2}", userId, SessionInfix, refreshSessionId);
            var find = await client.GetAsync<RefreshSession>(keyPattern);
            return find;
        }

        public async Task<IList<RefreshSession>> GetUserSessions(Guid userId)
        {
            await using var client = await _redisManagerPool.GetClientAsync();
            var keyPattern = string.Format("{0}:{1}:*", userId, SessionInfix);
            var keys = await ToListAsync(client.GetKeysByPatternAsync(keyPattern));
            if (keys.Count == 0) return Array.Empty<RefreshSession>();
            return (await client.GetAllAsync<RefreshSession>(keys))
                .Select(pair => pair.Value)
                .ToList();
        }

        public async Task<RefreshSession> UpdateSessionAsync(Guid sessionId, RefreshSession refreshSession)
        {
            var tuple = await DeleteSessionAsync(sessionId);
            if (tuple.Value == null) return null;

            var session = tuple.Value;
            session.CreateAtUTC = DateTime.UtcNow;
            session.ExpiresInUTC = refreshSession.ExpiresInUTC;
            session.FingerPrint = refreshSession.FingerPrint;
            session.IpAddress = refreshSession.IpAddress;
            session.UserAgent = refreshSession.UserAgent;
            return await CreateSessionAsync(tuple.Key, session);
        }

        private static async Task<List<T>> ToListAsync<T>(IAsyncEnumerable<T> asyncEnumerable)
        {
            var list = new List<T>();
            await foreach (var entry in asyncEnumerable)
            {                
                list.Add(entry);
            }
            return list;
        }

        private static async Task<bool> ContainsKeyPattern(IRedisClientAsync client, string keyPattern)
        {
            var list = await ToListAsync(client.GetKeysByPatternAsync(keyPattern));
            return list.Any();
        }        
    }
}