using System.Text.Json.Serialization;

namespace StudyTestingEnvironment.Models.Identity
{
    public class ConfirmRegistryRequest
    {
        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("token")]
        public string ConfirmToken { get; set; }
    }
}