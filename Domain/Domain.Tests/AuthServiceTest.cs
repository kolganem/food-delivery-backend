using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Domain.Application;
using Domain.Model;
using Domain.Model.Identity;
using Domain.Service;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Moq;
using NUnit.Framework;

namespace Domain.Tests
{
    public class AuthServiceTest
    {
        [OneTimeSetUp]
        public void Init()
        {
            var repository = GetRepository();
            var jwtSetting = new JwtSettings
            {
                Key = "this is my custom secret key for auth",
                TokenLifeTime = new TimeSpan(1, 0, 0)
            };
            var tokenValidationParameter = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = false,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSetting.Key)),
                ValidateIssuer = false,
                ValidateAudience = false
            };
            _authService = new AuthService(repository, jwtSetting, tokenValidationParameter, null, null);
        }
        public void CanRegister()
        {

        }

        public void CanLogin()
        {

        }

        public void CanRefreshToken()
        {

        }

        private IRepository<RefreshToken> GetRepository()
        {
            
            var repository = new Mock<GenericRepository<RefreshToken>>();
            repository.Setup(r => r.GetAll()).Returns(_refreshTokens);

            repository.Setup(r => r.Get(It.IsAny<Expression<Func<RefreshToken, bool>>>(),
                                                                        It.IsAny<Func<IQueryable<RefreshToken>, IOrderedQueryable<RefreshToken>>>(),
                                                                        It.IsAny<string>()))
                     .Returns((Expression<Func<RefreshToken, bool>> predicate,
                                        Func<IQueryable<RefreshToken>, IOrderedQueryable<RefreshToken>> orderBy,
                                        string include) => _refreshTokens.AsQueryable().Where(predicate));

            repository.Setup(r => r.Insert(It.IsAny<RefreshToken>()))
                        .Returns((RefreshToken target) =>
                        {
                            if (target==null)
                            {
                                return 0;
                            }
                            _refreshTokens.Add(target);
                            return 1;
                        });
            repository.Setup(r => r.Update(It.IsAny<RefreshToken>()))
                .Returns((RefreshToken target) =>
                {
                    var index = _refreshTokens.IndexOf(target);
                    if (index>=0)
                    {
                        _refreshTokens[index] = target;
                        return 1;
                    }

                    return 0;
                });

            return repository.Object;
        }

        private UserManager<User> GetUserManager()
        {
            var userManager = new Mock<UserManager<User>>();

            userManager.Setup(um => um.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((string email) =>
                {
                    return _users.Find(x => x.Email == email);
                });
            userManager.Setup(um => um.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((string id) =>
                {
                    return _users.FirstOrDefault(u => u.Id == id);
                });
            userManager.Setup(um => um.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .Returns(async (User user,string password) =>
                {
                    return await Task.Run<IdentityResult>(() => IdentityResult.Success);
                });

            return userManager.Object;
        }
        private readonly List<RefreshToken> _refreshTokens = new List<RefreshToken>
        {
            new RefreshToken
            {
                Token = "a771e0ef-e592-4ae3-ace0-ffc89eff89de",
                CreationDate = DateTime.Parse("2020-10-17 12:02:17.6690927"),
                ExpiryDate = DateTime.Parse("2020-11-17 10:02:17.0000000"),
                Used = false,
                Invalidated = false,
                UserId = "d7c197c4-c8dd-40b2-af3e-f751c60815fa"
            }
        };

        private readonly List<User> _users = new List<User>
        {
            new User
            {
                Id = "d7c197c4-c8dd-40b2-af3e-f751c60815fa",
                UserName = "dummy2@mail.ru",
                NormalizedUserName = "DUMMY2@MAIL.RU",
                Email = "dummy2@mail.ru",
                NormalizedEmail = "DUMMY2@MAIL.RU",
                EmailConfirmed = true,
                PasswordHash = "AQAAAAEAACcQAAAAEPpZuH/qCf5QVkXgtkmrpLLmmdBKS+mlZzqzD+908ktlVh0JHlaZmQVvMBvI6VxH2g==",
                SecurityStamp = "4RARUU6ZMKJ2FA5ICS333KWTCM6OA6LJ",
                ConcurrencyStamp = "34ad7a41-f0b1-42ff-8812-f49b8b7c054f",
                PhoneNumber = null,
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnabled = true,
                AccessFailedCount = 0,
                Address = null,
                Photo = null
            }
        };
        private IAuthService _authService;
    }
}
