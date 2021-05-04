using StudyTestingEnvironment.Data.Models;
using System;
using System.Threading.Tasks;

namespace StudyTestingEnvironment.Services.Identity
{
    public interface IUserHelper
    {
        Guid? GetCurrentUserId();

        Task<User> GetCurrentUser();
    }
}