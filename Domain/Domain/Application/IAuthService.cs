using Domain.Model.Identity;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Domain.Application
{
    public interface IAuthService
    {
        Task<AuthenticationResult> RegisterAsync(string userName, string password, UserRole role);

        Task<AuthenticationResult> LoginAsync(string userName, string password);

        Task<AuthenticationResult> RefreshTokenAsync(string token, string refreshToken);
    }
}