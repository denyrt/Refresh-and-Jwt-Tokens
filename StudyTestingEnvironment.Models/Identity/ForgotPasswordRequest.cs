using System.ComponentModel.DataAnnotations;

namespace StudyTestingEnvironment.Models.Identity
{
    public class ForgotPasswordRequest
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }
    }
}