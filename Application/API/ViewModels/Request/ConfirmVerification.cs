using Domain.Common.ValidationAttribute;
using Newtonsoft.Json;

namespace API.ViewModels.Request
{
    public class ConfirmVerification
    {
        [JsonRequired]
        [JsonProperty]
        [EmailOrPhone("EmailOrPhone")]
        public string To { get; set; }

        [JsonRequired]
        [JsonProperty]
        public string VerificationCode { get; set; }
    }
}
