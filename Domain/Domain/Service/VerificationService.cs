using System.Threading.Tasks;
using Domain.Application;
using Domain.Model.Verification;
using Microsoft.Extensions.Options;
using Twilio.Exceptions;
using Twilio.Rest.Verify.V2.Service;

namespace Domain.Service
{
    public class VerificationService:IVerificationService
    {
        public VerificationService(IOptions<TwilioVerifySettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task<VerificationResult> SendVerificationCodeAsync(string to, string channel)
        {
            try
            {
                var verification = await VerificationResource.CreateAsync(
                    to: to,
                    channel: channel.ToLower(),
                    pathServiceSid: _settings.VerificationServiceSid
                );
                if (verification != null)
                {
                    return new VerificationResult
                    {
                        VerificationStatus = verification.Status
                    };
                }
            }
            catch (TwilioException e)
            {
                //TODO: Log
                return new VerificationResult
                {
                    VerificationStatus = "error",
                    Error = e.Message
                };
            }
            catch
            {
                //TODO: Log
            }
            return new VerificationResult
            {
                VerificationStatus = "error",
                Error = "There was an error sending the verification code"
            };
        }

        public async Task<VerificationResult> ConfirmVerificationCodeAsync(string to, string verificationCode)
        {
            try
            {
                var verification = await VerificationCheckResource.CreateAsync(
                    to: to,
                    code: verificationCode,
                    pathServiceSid: _settings.VerificationServiceSid
                );
                if (verification != null)
                {
                    return new VerificationResult
                    {
                        VerificationStatus = verification.Status
                    };
                }
            }
            catch (TwilioException e)
            {
                //TODO: Log
                return new VerificationResult
                {
                    VerificationStatus = "error",
                    Error = e.Message
                };
            }
            catch
            {
                //TODO: Log
            }
            return new VerificationResult
            {
                VerificationStatus = "error",
                Error = "There was an error confirming the verification code"
            };
        }

        private readonly TwilioVerifySettings _settings;
    }
}
