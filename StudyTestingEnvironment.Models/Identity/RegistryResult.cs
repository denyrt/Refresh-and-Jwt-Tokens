using StudyTestingEnvironment.Models.Common;
using System.Text.Json.Serialization;

namespace StudyTestingEnvironment.Models.Identity
{
    public class RegistryResult : OperationResult
    {
        [JsonPropertyName("destination_email")]
        public string DestinationEmail { get; set; }
    }
}