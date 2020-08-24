using System.Threading.Tasks;

namespace PaperStreet.Authentication.Application.Interfaces
{
    public interface IEmailBuilder
    {
        string ConfirmationEmail(string firstName, string userId, string emailConfirmationCode);

        string ResetPasswordEmail(string firstName, string userId, string resetPasswordConfirmationCode);
        string PasswordChangedEmail(string firstName);
    }
}