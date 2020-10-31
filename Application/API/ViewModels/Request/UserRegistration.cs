using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Domain.Model.Identity;
using Domain.Common.ValidationAttribute;

namespace API.ViewModels.Request
{
    public class UserRegistration
    {
        [JsonRequired]
        [JsonProperty]
        [EmailOrPhone("EmailOrPhone")]
        public string UserName { get; set; }

        [JsonRequired]
        [JsonProperty]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [JsonRequired]
        [JsonProperty]
        public UserRole Role { get; set; }
    }
}
