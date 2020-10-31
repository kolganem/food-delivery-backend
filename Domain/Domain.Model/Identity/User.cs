using Microsoft.AspNetCore.Identity;

namespace Domain.Model.Identity
{
    public class User : IdentityUser
    {
        public string Address { get; set; }
        public string Photo { get; set; }
    }
}
