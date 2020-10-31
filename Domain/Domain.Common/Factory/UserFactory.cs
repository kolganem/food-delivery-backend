using System;
using Domain.Model.Identity;

namespace Domain.Common.Factory
{
    public class UserFactory
    {
        public UserFactory(string userName)
        {
            _userName = userName;
        }

        public User CreateUser(UserRole role)
        {
            switch (role)
            {
                case UserRole.User:
                    var user = new User
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserName = _userName,
                        PhoneNumber = _userName
                    };
                    return user;
                case UserRole.Business:
                    var businessUser = new Business()
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserName = _userName,
                        Email = _userName,
                        IsVerified = false
                    };
                    return businessUser;
                default:
                    var admin = new User
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserName = _userName
                    };
                    return admin;
            }
        }

        private readonly string _userName;
    }
}
