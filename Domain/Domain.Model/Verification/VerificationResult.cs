using Newtonsoft.Json;

namespace Domain.Model.Verification
{
    public class VerificationResult
    {
        [JsonRequired]
        [JsonProperty]
        public string VerificationStatus { get; set; }

        [JsonProperty]
        public string Error { get; set; }
    }
}
