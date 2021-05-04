using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace StudyTestingEnvironment.Models.Identity
{
    public class RegistryRequest
    {
        /// <summary>
        /// Username for login.
        /// </summary>
        [Required]
        [StringLength(55, MinimumLength = 3)]        
        [JsonPropertyName("login")]
        public string Login { get; set; }

        /// <summary>
        /// Email for login and security operations.
        /// </summary>
        [Required]
        [EmailAddress]
        [JsonPropertyName("email")]
        public string Email { get; set; }

        /// <summary>
        /// First name of user.
        /// </summary>
        [Required]
        [StringLength(55, MinimumLength = 3)]
        [JsonPropertyName("firstname")]
        public string FirstName { get; set; }

        /// <summary>
        /// Last name of user.
        /// </summary>
        [Required]
        [StringLength(55, MinimumLength = 3)]
        [JsonPropertyName("lastname")]
        public string LastName { get; set; }

        /// <summary>
        /// Patronymic of user.
        /// </summary>
        [Required]
        [StringLength(55, MinimumLength = 3)]
        [JsonPropertyName("patronymic")]
        public string Patronymic { get; set; }
        
        /// <summary>
        /// Role of user.
        /// </summary>
        [Required]
        [JsonPropertyName("role")]
        public RegistryRole Role { get; set; }

        /// <summary>
        /// Password from 8 to 55 length. Must includes one big letter, one number and one special symbol.        
        /// </summary>
        /// <remarks>
        /// Example: MyStr0ng!Password
        /// </remarks>
        [Required]
        [StringLength(55, MinimumLength = 8)]
        [JsonPropertyName("password")]
        public string Password { get; set; }

        /// <summary>
        /// Password repeat.
        /// </summary>
        [Required]
        [Compare("Password", ErrorMessage = "Passwords are not equals.")]
        [JsonPropertyName("repeat_password")]
        public string RepeatPassword { get; set; }
    }
}