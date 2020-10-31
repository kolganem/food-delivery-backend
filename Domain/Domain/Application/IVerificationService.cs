using System.Threading.Tasks;
using Domain.Model.Verification;

namespace Domain.Application
{
    public interface IVerificationService
    {
        Task<VerificationResult> SendVerificationCodeAsync(string to, string channel);
        Task<VerificationResult> ConfirmVerificationCodeAsync(string to, string verificationCode);
    }
}
