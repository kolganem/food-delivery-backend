using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Domain.Common.ValidationAttribute;

namespace API.ViewModels.Request
{
    public class UserLogin
    {
        [JsonRequired]
        [JsonProperty]
        [EmailOrPhone("EmailOrPhone")]
        public string UserName { get; set; }

        [JsonRequired]
        [JsonProperty]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
