using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Model.Verification
{
    public class TwilioSendGridSettings
    {
        public string ApiKey { get; set; }
        public string From { get; set; }
        public string TemplateId { get; set; }
    }
}
