using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudyTestingEnvironment.Data.Configuration;
using StudyTestingEnvironment.Data.Models;
using System;

namespace StudyTestingEnvironment.Data.Contexts
{
    public class IdentityContext : IdentityDbContext<User, Role, Guid>
    {        
        public IdentityContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new RoleConfiguration());
        }
    }
}