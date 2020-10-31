using System.Threading.Tasks;
using API.ViewModels.Request;
using Domain.Application;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class VerifyController : ControllerBase
    {
        public VerifyController(IVerificationService verificationService, 
                                IVerificationStateService verificationStateService)
        {
            _verificationService = verificationService;
            _verificationStateService = verificationStateService;
        }

        [Route("send")]
        public async Task<IActionResult> SendVerificationCodeAsync([FromBody] SendVerification request)
        {
            var verificationResult =
                await _verificationService.SendVerificationCodeAsync(request.To, request.Channel.ToString());

            switch (verificationResult.VerificationStatus)
            {
                case "error":
                    return StatusCode(StatusCodes.Status500InternalServerError, verificationResult);
                default:
                {
                    var result = _verificationStateService.Create(request.To, verificationResult.VerificationStatus, false);
                    if (result)
                    {
                        return StatusCode(StatusCodes.Status200OK, verificationResult);
                    }

                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }
        }

        [Route("confirm")]
        public async Task<IActionResult> ConfirmVerificationCodeAsync([FromBody] ConfirmVerification request)
        {
            var verificationResult =  await _verificationService.ConfirmVerificationCodeAsync(request.To, request.VerificationCode);
            switch (verificationResult.VerificationStatus)
            {
                case "error":
                    return StatusCode(StatusCodes.Status500InternalServerError, verificationResult);
                case "approved":
                {
                    var result = _verificationStateService.Update(request.To, true);
                    if (result)
                    {
                        return StatusCode(StatusCodes.Status200OK, verificationResult);
                    }

                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
                default: return StatusCode(StatusCodes.Status200OK, verificationResult);
            }
        }

        private readonly IVerificationStateService _verificationStateService;
        private readonly IVerificationService _verificationService;
    }
}
