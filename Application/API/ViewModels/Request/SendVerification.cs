using System.ComponentModel.DataAnnotations;
using Domain.Common.ValidationAttribute;
using Newtonsoft.Json;

namespace API.ViewModels.Request
{
    public class SendVerification
    {
        [JsonRequired]
        [JsonProperty]
        [EmailOrPhone("EmailOrPhone")]
        public string To { get; set; }

        [JsonRequired]
        [JsonProperty]
        public ChannelType Channel { get; set; }
    }
}
