using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Model.Identity;
using Domain.Model.ServiceModel;

namespace Domain.Application
{
    public interface IAdminService
    {
        Task<ServiceResult> ConfirmBusinessAsync(string userName);
        Task<IEnumerable<Business>> GetBusinessesAsync();
    }
}