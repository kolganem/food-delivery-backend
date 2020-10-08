using Microsoft.AspNetCore.Identity;
using System;

namespace Domain.Model
{
    public class User:IdentityUser<Guid>
    {
        public string Address { get; set; }
        public byte[] Photo { get; set; }
    }
}
