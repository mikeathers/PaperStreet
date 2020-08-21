using System.Threading.Tasks;
using PaperStreet.Authentication.Domain.Models;
using PaperStreet.Domain.Core.Models;

namespace PaperStreet.Authentication.Application.Interfaces
{
    public interface IUserConfirmationEmail
    {
        Task Send(AppUser user);
    }
}