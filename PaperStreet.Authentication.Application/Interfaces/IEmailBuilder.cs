using System.Threading.Tasks;

namespace PaperStreet.Authentication.Application.Interfaces
{
    public interface IEmailBuilder
    {
        string ConfirmationEmail(string firstName, string userId, string emailConfirmationCode);
    }
}