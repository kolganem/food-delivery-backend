using System.Threading.Tasks;
using SendGrid;

namespace Domain.Application
{
    public interface IEmailService
    {
        Task<Response> SendAsync(string to);
    }
}