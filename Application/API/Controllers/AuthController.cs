using System.Threading.Tasks;
using API.ViewModels.Request;
using Domain.Application;
using Domain.Model.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        public AuthController(IAuthService authService, IVerificationStateService verificationStateService)
        {
            _authService = authService;
            _verificationStateService = verificationStateService;
        }

        [HttpPost("register")]
        public async Task<AuthenticationResult> RegisterAsync([FromBody] UserRegistration user)
        {
            var isConfirmed = _verificationStateService.IsConfirmed(user.UserName);
            if (isConfirmed)
            {
                var result = await _authService.RegisterAsync(user.UserName, user.Password, user.Role);
                return result;
            }

            return new AuthenticationResult
            {
                Success = false,
                Errors = new[]
                {
                    $"User with user name {user.UserName} does not confirm e-mail/phone number"
                }
            };
        }

        [HttpPost("login")]
        public async Task<AuthenticationResult> LoginAsync([FromBody] UserLogin user)
        {
            var result = await _authService.LoginAsync(user.UserName, user.Password);
            return result;
        }

        [HttpPost("refresh-token")]
        public async Task<AuthenticationResult> RefreshTokenAsync([FromBody] UserRefreshToken user)
        {
            var result = await _authService.RefreshTokenAsync(user.Token, user.RefreshToken);
            return result;
        }

        private readonly IVerificationStateService _verificationStateService;
        private readonly IAuthService _authService;
    }
}
