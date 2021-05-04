using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace StudyTestingEnvironment.Models.Identity
{
    public class ResetPasswordRequest
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        [JsonPropertyName("email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [JsonPropertyName("password")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Passwords not equals.")]
        [JsonPropertyName("repeat_password")]
        public string RepeatPassword { get; set; }

        [Required(ErrorMessage = "Token is required.")]
        [JsonPropertyName("token")]
        public string Token { get; set; }
    }
}