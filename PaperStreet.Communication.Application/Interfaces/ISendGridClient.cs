using System.Net;
using System.Threading.Tasks;
using PaperStreet.Domain.Core.Models;
using SendGrid.Helpers.Mail;

namespace PaperStreet.Communication.Application.Interfaces
{
    public interface ISendGridClient 
    {
        Task<HttpStatusCode> SendEmailAsync(Email emailToSend);
    }
}