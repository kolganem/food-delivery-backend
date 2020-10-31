using System.Net;
using System.Threading.Tasks;
using Domain.Application;
using Domain.Model.Verification;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Domain.Service
{
    public class EmailService:IEmailService
    {
        public EmailService(IOptions<TwilioSendGridSettings> settings)
        {
            _settings = settings.Value;
            _client=new SendGridClient(_settings.ApiKey);
        }
        public async Task<Response> SendAsync(string to)
        {
            try
            {
                var sendGridMessage = new SendGridMessage();
                sendGridMessage.SetFrom(_settings.From);
                sendGridMessage.AddTo(to);
                sendGridMessage.SetTemplateId(_settings.TemplateId);
                //TODO: put user data data
                //TODO: handle exceptions
                /*
                sendGridMessage.SetTemplateData(new HelloEmail
                {
                    Name = "Vivien",
                    Url = "https://www.vivienfabing.com"
                });
                */
                var response = await _client.SendEmailAsync(sendGridMessage);
                return response;
            }
            catch
            {
                return new Response(HttpStatusCode.InternalServerError, null, null);
            }
        }

        private readonly TwilioSendGridSettings _settings;
        private readonly SendGridClient _client;
    }
}
