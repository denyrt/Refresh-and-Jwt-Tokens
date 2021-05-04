using StudyTestingEnvironment.Models.Common;
using System;
using System.Text.Json.Serialization;

namespace StudyTestingEnvironment.Models.Identity
{
    public class LoginResult : OperationResult
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }
        
        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonPropertyName("csfr_token")]
        public string XsfrToken { get; set; }

        [JsonIgnore]
        public DateTime RefreshExpiresInUTC { get; set; }
    }
}
