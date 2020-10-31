using System;

namespace Domain.Model.Identity
{
    public class JwtSettings
    {
        public string Key { get; set; }
        public TimeSpan TokenLifeTime { get; set; }
    }
}
