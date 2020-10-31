using System.Net;
using System.Threading.Tasks;
using Domain.Application;
using Domain.Model.ServiceModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Authorize(Roles ="ADMIN")]
    [Route("[controller]")]
    public class AdminController : ControllerBase
    {
        public AdminController(IAdminService adminService, IEmailService emailService)
        {
            _adminService = adminService;
            _emailService = emailService;
        }

        [Route("confirm-business/{businessUserName}")]
        public async Task<ServiceResult> ConfirmBusiness(string businessUserName)
        {
           var serviceResult = await _adminService.ConfirmBusinessAsync(businessUserName);
           if (serviceResult.Success)
           {
               var response = await _emailService.SendAsync(businessUserName);
               if (response.StatusCode==HttpStatusCode.Accepted)
               {
                   return serviceResult;
               }
               return new ServiceResult
               {
                   Errors = new[] {"User account confirmed. Error while sending e-mail."}
               };
           }

           return serviceResult;
        }

        [Route("businesses")]
        public async Task<IActionResult> GetBusinesses()
        {
            var businesses = await _adminService.GetBusinessesAsync();
            if (businesses!=null)
            {
                return Ok(businesses);
            }

            return BadRequest();
        }

        private readonly IAdminService _adminService;
        private readonly IEmailService _emailService;
    }
}
