using System.Text.Json.Serialization;

namespace StudyTestingEnvironment.Models.Common
{
    public class OperationResult
    {
        [JsonIgnore]
        public bool IsSuccess { get; set; }
        
        [JsonPropertyName("description")]
        public string Description { get; set; }
    }
}