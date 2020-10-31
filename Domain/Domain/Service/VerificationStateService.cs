using System;
using System.Linq;
using Domain.Application;
using Domain.Model.Verification;
using Infrastructure.Repository;

namespace Domain.Service
{
    public class VerificationStateService:IVerificationStateService
    {
        public VerificationStateService(IRepository<VerificationState> repository)
        {
            _repository = repository;
        }

        public bool Create(string to, string status, bool isConfirmed)
        {
            try
            {
                var state = new VerificationState
                {
                    To = to,
                    IsConfirmed = isConfirmed,
                    VerificationStatus = status,
                    CreatedDateTime = DateTime.Now
                };
                var result = _repository.Insert(state);
                if (result > 0)
                {
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }

        }

        public bool Update(string userName, bool isConfirmed)
        {
            try
            {
                var verificationStates = _repository.Get(vs => vs.To == userName);
                var maxDatetime = verificationStates.Max(vs => vs.CreatedDateTime);
                var lastState = _repository.Get(vs => vs.CreatedDateTime == maxDatetime).FirstOrDefault();
                if (lastState==null)
                {
                    return false;
                }

                lastState.IsConfirmed = isConfirmed;
                lastState.VerificationStatus = "approved";
                var result = _repository.Update(lastState);
                if (result > 0)
                {
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        public bool IsConfirmed(string userName)
        {
            try
            {
                var verificationStates = _repository.Get(vs => vs.To == userName);
                var maxDatetime = verificationStates.Max(vs => vs.CreatedDateTime);
                var lastState = _repository.Get(vs => vs.CreatedDateTime == maxDatetime).FirstOrDefault();
                if (lastState == null)
                {
                    return false;
                }

                return lastState.IsConfirmed;
            }
            catch
            {
                return false;
            }
        }

        private readonly IRepository<VerificationState> _repository;
    }
}
