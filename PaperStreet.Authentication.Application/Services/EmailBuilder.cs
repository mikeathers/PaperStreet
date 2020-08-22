using System;
using System.Text;
using System.Web;
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
            var webSiteUrl = _configuration.GetValue<string>("WebSiteUrl");

            var encodedUserId = HttpUtility.UrlEncode(userId);
            var encodedEmailConfirmationCode = HttpUtility.UrlEncode(emailConfirmationCode);
            
            var emailUrl = 
                new Uri($"https://{webSiteUrl}/api/v1/authentication/confirm-email/{encodedUserId}/{encodedEmailConfirmationCode}");
            
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
    }
}