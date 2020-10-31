using Domain.Model.ServiceModel;

namespace Domain.Model.Identity
{
    public class AuthenticationResult:ServiceResult
    {
        public string Token { get; set; }

        public string RefreshToken { get; set; }
    }
}
