using System.Net;
using System.Threading.Tasks;
using PaperStreet.Domain.Core.Models;

namespace PaperStreet.Communication.Application.Interfaces
{
    public interface IEmailSender
    {
        Task SendEmail(Email emailToSend);
    }
}