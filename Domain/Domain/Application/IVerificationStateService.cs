using Domain.Model.Verification;

namespace Domain.Application
{
    public interface IVerificationStateService
    {
        bool Create(string to, string status, bool isConfirmed);
        bool Update(string to, bool isConfirmed);
        bool IsConfirmed(string userName);
    }
}
