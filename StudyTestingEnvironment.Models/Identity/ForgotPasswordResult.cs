using StudyTestingEnvironment.Models.Common;
using System.Text.Json.Serialization;

namespace StudyTestingEnvironment.Models.Identity
{
    public class ForgotPasswordResult : OperationResult
    {
        [JsonPropertyName("destination_email")]
        public string DestinationEmail { get; set; }

        /// <summary>
        /// Timeout before next password recovery attempt.
        /// </summary>
        [JsonPropertyName("timeout")]
        public double Timeout { get; set; }
    }
}