using System.Collections.Generic;

namespace Domain.Model.ServiceModel
{
    public class ServiceResult
    {
        public bool Success { get; set; }
        public IEnumerable<string> Messages { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
