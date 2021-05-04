using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace StudyTestingEnvironment.Models.Identity
{
    public class LoginRequest
    {
        /// <summary>
        /// Logir or email to authorize.
        /// </summary>
        [Required(ErrorMessage = "Login or Email is required to authorize.")]
        [StringLength(55, MinimumLength = 3)]
        [JsonPropertyName("login")]
        public string LoginOrEmail { get; set; }

        /// <summary>
        /// User password.
        /// </summary>
        [Required(ErrorMessage = "Password is required.")]
        [StringLength(55, MinimumLength = 8)]
        [JsonPropertyName("password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Fingerprint is required.")]
        [JsonPropertyName("fingerprint")]
        public string FingerPrint { get; set; }
    }
}