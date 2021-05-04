using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudyTestingEnvironment.Data.Models
{
    public class User : IdentityUser<Guid>
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Patronymic { get; set; }

        public virtual ICollection<Role> Roles { get; } = new List<Role>();

        [Required]
        public bool UseIpBinding { get; set; } = true;
    }
}