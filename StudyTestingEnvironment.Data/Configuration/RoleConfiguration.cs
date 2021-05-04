using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudyTestingEnvironment.Data.Models;
using System;

namespace StudyTestingEnvironment.Data.Configuration
{
    internal class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            var data = new Role[]
            {
                new Role
                {                    
                    Id = Guid.NewGuid(),
                    Name = "Teacher",
                    NormalizedName = "TEACHER"
                },

                new Role
                {
                    Id = Guid.NewGuid(),
                    Name = "Student",
                    NormalizedName = "STUDENT"
                },

                new Role
                {
                    Id = Guid.NewGuid(),
                    Name = "Moderator",
                    NormalizedName = "MODERATOR"
                }
            };

            builder.HasData(data);
        }
    }
}