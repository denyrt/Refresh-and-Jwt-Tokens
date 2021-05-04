using System;
using System.Text.Json.Serialization;

namespace StudyTestingEnvironment.Models.Identity
{    
    public class SecureToken
    {
        [JsonPropertyName("token")]
        public Guid Token { get; set; }

        [JsonPropertyName("expires")]
        public DateTime Expires { get; set; }
    }
}
