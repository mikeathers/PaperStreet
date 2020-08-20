using System.Text;
using Microsoft.Extensions.Configuration;
using PaperStreet.Authentication.Application.Interfaces;

namespace PaperStreet.Authentication.Application.Services
{
    public class EmailBuilder : IEmailBuilder
    {
        private readonly IConfiguration _configuration;
        public EmailBuilder(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public string ConfirmationEmail(string firstName, string userId, string emailConfirmationCode)
        {
            var webSiteUrl = _configuration.GetSection("WebSiteUrl").Path;
            
            var emailUrl = $"http://{webSiteUrl}/api/authentication/confirm-email/{userId}/{emailConfirmationCode}";
            
            var sb = new StringBuilder();
            
            sb.Append($"<p>Hi {firstName}</p>");
            sb.Append("<br />");
            sb.Append("<br />");
            sb.Append("<p>Please confirm your account by clicking on the link below.<p>");
            sb.Append("<br />");
            sb.Append("<br />");
            sb.Append($"<a href={emailUrl}>Confirm Account :)</a>");
            sb.Append("<br />");
            sb.Append("<br />");
            sb.Append("<p>Thanks<p>");

            return sb.ToString();
        }
    }
}