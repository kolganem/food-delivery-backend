using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Domain.Application;
using Domain.Common.Factory;
using Domain.Model;
using Domain.Model.Identity;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Domain.Service
{
    public class AuthService:IAuthService
    {
        public AuthService(IRepository<RefreshToken> repository, 
                            JwtSettings settings, 
                            TokenValidationParameters tokenValidationParameters,
                            UserManager<User> userManager,
                            RoleManager<IdentityRole> roleManager)
        {
            _repository = repository;
            _jwtSettings = settings;
            _tokenValidationParameters = tokenValidationParameters;
            _userManager = userManager;
            _roleManager = roleManager;

        }

        public async Task<AuthenticationResult> RegisterAsync(string userName, string password, UserRole role)
        {
            var existingUser = await _userManager.FindByNameAsync(userName);

            if (existingUser != null)
            {
                return new AuthenticationResult
                {
                    Errors = new[] { $"User with this user name: { userName } already exists" }
                };
            }

            var userFactory = new UserFactory(userName);
            var newUser = userFactory.CreateUser(role);

            var authenticationResultCreateUser = await CreateUser(newUser, password);
            if (!authenticationResultCreateUser.Success)
            {
                return authenticationResultCreateUser;
            }

            var authenticationResultAddToRole = await AddToRole(newUser, role);
            if (!authenticationResultAddToRole.Success)
            {
                return authenticationResultAddToRole;
            }

            return await GenerateAuthenticationResultForUserAsync(newUser);
        }

        public async Task<AuthenticationResult> LoginAsync(string userName, string password)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user == null)
            {
                return new AuthenticationResult
                {
                    Errors = new[] {"User does not exist"}
                };
            }
            var hasValidPassword = await _userManager.CheckPasswordAsync(user, password);

            if (!hasValidPassword)
            {
                return new AuthenticationResult
                {
                    Errors = new[] {"Invalid Username or Password"}
                };

            }

            var roles = await _userManager.GetRolesAsync(user);
            var isBusinessUser = roles.Any(r => r == UserRole.Business.ToString());
            if (isBusinessUser && user is Business businessUser && !businessUser.IsVerified)
            {
                return new AuthenticationResult
                {
                    Errors = new[] {"Account not yet confirmed by administrator"}
                };
            }

            if (isBusinessUser && !(user is Business))
            {
                return new AuthenticationResult
                {
                    Errors = new[] { "Can not find information about user. Please contact Food Delivery Support." }
                };
            }

            return await GenerateAuthenticationResultForUserAsync(user);
        }

        public async Task<AuthenticationResult> RefreshTokenAsync(string token, string refreshToken)
        {
            var validatedToken = GetPrincipalFromToken(token);

            if (validatedToken == null)
            {
                return new AuthenticationResult
                {
                    Errors = new[] {"Invalid Token"}
                };
            }

            var expiryDateUnix = long.Parse(validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

            var expiryDateTimeUtc = new DateTime(new DateTime().Ticks,kind: DateTimeKind.Utc).AddSeconds(expiryDateUnix);

            if (expiryDateTimeUtc > DateTime.UtcNow)
            {
                return new AuthenticationResult {Errors = new[] {"This token hasn't expired yet"}};
            }

            var storedRefreshToken = _repository.Get(x => x.Token == refreshToken).FirstOrDefault();
            if (storedRefreshToken == null)
            {
                return new AuthenticationResult {Errors = new[] {"This refresh token doesn't exist"}};
            }

            if (DateTime.UtcNow > storedRefreshToken.ExpiryDate)
            {
                return new AuthenticationResult {Errors = new[] {"This refresh token has expired"}};
            }

            if (storedRefreshToken.Used)
            {
                return new AuthenticationResult {Errors = new[] {"This refresh token has been used"}};
            }

            var user = await _userManager.FindByIdAsync(validatedToken.Claims.Single(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value);

            storedRefreshToken.Used = true;
            _repository.Update(storedRefreshToken);

            return await GenerateAuthenticationResultForUserAsync(user);
        }

        private ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var tokenValidationParameters = _tokenValidationParameters
                    .Clone();

                tokenValidationParameters.ValidateLifetime = false;

                var principal = tokenHandler
                    .ValidateToken(token, tokenValidationParameters, out var validatedToken);

                if (!IsJwtWithValidSecurityAlgorithm(validatedToken))
                {
                    return null;
                }

                return principal;
            }
            catch
            {
                return null;
            }
        }

        private bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
        {
            return (validatedToken is JwtSecurityToken jwtSecurityToken) &&
                   jwtSecurityToken.Header.Alg
                   .Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
        }

        private async Task<AuthenticationResult> GenerateAuthenticationResultForUserAsync(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Key);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            var userClaims = await _userManager.GetClaimsAsync(user);
            claims.AddRange(userClaims);

            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
                var role = await _roleManager.FindByNameAsync(userRole);
                if (role == null) continue;
                var roleClaims = await _roleManager.GetClaimsAsync(role);

                foreach (var roleClaim in roleClaims)
                {
                    if (claims.Contains(roleClaim))
                        continue;

                    claims.Add(roleClaim);
                }
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(_jwtSettings.TokenLifeTime),
                SigningCredentials =
                    new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var refreshToken = new RefreshToken
            {
                UserId = user.Id,
                CreationDate = DateTime.Now,
                ExpiryDate = token.ValidTo.AddMonths(1)
            };

            _repository.Insert(refreshToken);

            return new AuthenticationResult
            {
                Success = true,
                Token = tokenHandler.WriteToken(token),
                RefreshToken = refreshToken.Token
            };
        }
      
        private async Task<AuthenticationResult> AddToRole(User newUser, UserRole role)
        {
            var createdRole = await _userManager.AddToRoleAsync(newUser, role.ToString());
            if (!createdRole.Succeeded)
            {
                return new AuthenticationResult()
                {
                    Success = false,
                    Errors = createdRole.Errors.Select(x => x.Description)
                };
            }

            return new AuthenticationResult() { Success = true };
        }
        private async Task<AuthenticationResult> CreateUser(User newUser, string password)
        {
            var createdUser = await _userManager.CreateAsync(newUser, password);
            if (!createdUser.Succeeded)
            {
                var authenticationResult = new AuthenticationResult
                {
                    Success = false,
                    Errors = createdUser.Errors.Select(x => x.Description)
                };
                return authenticationResult;
            }

            return new AuthenticationResult() { Success = true };
        }

        private readonly IRepository<RefreshToken> _repository;
        private readonly UserManager<User> _userManager;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JwtSettings _jwtSettings;
    }
}
