using System.Text;
using System.Web;
using Microsoft.Extensions.Configuration;
using PaperStreet.Authentication.Application.Interfaces;

namespace PaperStreet.Authentication.Application.Services
{
    public class EmailBuilder : IEmailBuilder
    {
        private readonly string _websiteUrl;
        public EmailBuilder(IConfiguration configuration)
        {
            _websiteUrl = configuration.GetValue<string>("WebsiteUrl");
        }
        
        public string ConfirmationEmail(string firstName, string userId, string emailConfirmationCode)
        {
            var encodedUserId = HttpUtility.UrlEncode(userId);
            var encodedEmailConfirmationCode = HttpUtility.UrlEncode(emailConfirmationCode);
            
            // TODO: Update url to send user to website frontend with token in the url to be passed into an API call on page load  
            var emailUrl =
                $"https://{_websiteUrl}/api/v1/authentication/confirm-email/{encodedUserId}/{encodedEmailConfirmationCode}";
            
            var sb = new StringBuilder();
            
            sb.Append($"<p>Hi {firstName}</p>");
            sb.Append("<br />");
            sb.Append("<br />");
            sb.Append("<p>Please confirm your account by clicking on the link below.<p>");
            sb.Append("<br />");
            sb.Append("<br />");
            sb.Append($"<a href=\"{emailUrl}\">Confirm Account :)</a>");
            sb.Append("<br />");
            sb.Append("<br />");
            sb.Append("<p>Thanks<p>");

            return sb.ToString();
        }

        public string ResetPasswordEmail(string firstName, string userId, string resetPasswordConfirmationCode)
        {
            var encodedUserId = HttpUtility.UrlEncode(userId);
            var encodedResetPasswordCode = HttpUtility.UrlEncode(resetPasswordConfirmationCode);
            
            // TODO: Update url to send user to website frontend with token in the url to be passed into an API call when user resets password
            var emailUrl =
                $"https://{_websiteUrl}/api/v1/authentication/reset-password/{encodedUserId}/{encodedResetPasswordCode}";
            
            var sb = new StringBuilder();
            
            sb.Append($"<p>Hi {firstName}</p>");
            sb.Append("<br />");
            sb.Append("<br />");
            sb.Append("<p>Please confirm your account by clicking on the link below.<p>");
            sb.Append("<br />");
            sb.Append("<br />");
            sb.Append($"<a href=\"{emailUrl}\">Confirm Account :)</a>");
            sb.Append("<br />");
            sb.Append("<br />");
            sb.Append("<p>Thanks<p>");

            return sb.ToString();
        }

        public string PasswordChangedEmail(string firstName)
        {
            var sb = new StringBuilder();
            
            sb.Append($"<p>Hi {firstName}</p>");
            sb.Append("<br />");
            sb.Append("<br />");
            sb.Append(
                "<p>Your password has been changed. If you did not reset change password, please get in touch immediately<p>");
            sb.Append("<br />");
            sb.Append("<br />");
            sb.Append("<p>Thanks<p>");

            return sb.ToString();
        }
    }
}