using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Application;
using Domain.Model.Identity;
using Domain.Model.ServiceModel;
using Microsoft.AspNetCore.Identity;

namespace Domain.Service
{
    public class AdminService:IAdminService
    {
        public AdminService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        public async Task<ServiceResult> ConfirmBusinessAsync(string userName)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(userName);

                if (!(user is Business))
                {
                    return new ServiceResult
                    {
                        Errors = new[] { $"User with this user name: {userName} does not exists" }
                    };
                }

                var business = user as Business;
                business.IsVerified = true;

                var identityResult = await _userManager.UpdateAsync(business);
                if (identityResult.Succeeded)
                {
                    return new ServiceResult
                    {
                        Success = true
                    };
                }

                return new ServiceResult
                {
                    Errors = new[] {$"Can not confirm business user with user name: {userName}"}
                };
            }
            catch (Exception e)
            {
                return new ServiceResult
                {
                    Errors = new[] { $"Confirming process down with message: {e.Message}" }
                };
            }
        }

        public async Task<IEnumerable<Business>> GetBusinessesAsync()
        {
            try
            {
                var users = await _userManager.GetUsersInRoleAsync(UserRole.Business.ToString());
                var businesses = users.Cast<Business>();
                return businesses;
            }
            catch
            {
                return null;
            }
        }

        private readonly UserManager<User> _userManager;
    }
}
