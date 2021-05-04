using NUnit.Framework;
using ServiceStack.Redis;
using StudyTestingEnvironment.Data.Models;
using StudyTestingEnvironment.Services.Identity.Repositories;
using System;
using System.Threading.Tasks;

namespace StudyTestingEnvironment.NUnitTests
{
    public class Tests
    {
        private ISessionsRepository repository;
        private RedisManagerPool redisPool;
        private Guid user1 = Guid.NewGuid();

        [SetUp]
        public void Setup()
        {
            redisPool = new RedisManagerPool("localhost:6379");
            repository = new SessionsRepository(redisPool);
            ClearRedisStorage().Wait();
        }

        [Test]
        public void TestCreateEntity()
        {
            var session = new RefreshSession
            {
                ExpiresInUTC = DateTime.Now.AddDays(60),
                FingerPrint = "finger",
                IpAddress = "localhost",
                UserAgent = "Chrome"
            };

            var created = repository.CreateSessionAsync(user1, session).Result;
            Assert.AreNotEqual(created, null);
            ClearRedisStorage().Wait();
        }
        
        [Test]
        public void TestFindSession()
        {
            var session = new RefreshSession
            {
                ExpiresInUTC = DateTime.Now.AddDays(60),
                FingerPrint = "finger"
            };

            var created = repository.CreateSessionAsync(user1, session).Result;
            var find1 = repository.FindSessionAsync(created.Id).Result;
            var find2 = repository.FindSessionAsync(user1, created.Id).Result;
            Assert.AreEqual(created, find1);
            Assert.AreEqual(created, find2);
        }

        [Test]
        public void TestGetAllUsersSessions()
        {
            var s1 = repository.CreateSessionAsync(user1, new RefreshSession 
            { 
                FingerPrint = "finger1",
                ExpiresInUTC = DateTime.Now.AddDays(60)
            }).Result;
            var s2 = repository.CreateSessionAsync(user1, new RefreshSession 
            {
                FingerPrint = "finger2",
                ExpiresInUTC = DateTime.Now.AddDays(60)
            }).Result;

            var sessions = repository.GetUserSessions(user1).Result;
            Assert.AreEqual(2, sessions.Count);
            ClearRedisStorage().Wait();
        }

        private async Task ClearRedisStorage()
        {
            await using var client = await redisPool.GetClientAsync();
            var keys = await client.GetAllKeysAsync();
            await client.RemoveAllAsync(keys);            
        }
    }
}