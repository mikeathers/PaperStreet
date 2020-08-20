using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using PaperStreet.Communication.Domain.Models;
using PaperStreet.Domain.Core.Models;
using SendGrid.Helpers.Mail;
using ISendGridClient = PaperStreet.Communication.Application.Interfaces.ISendGridClient;

namespace PaperStreet.Communication.Application.Services
{
    public class SendGridClient : ISendGridClient
    {
        private readonly SendGridSettings _sendGridSettings;

        public SendGridClient(IConfiguration configuration)
        {
            _sendGridSettings = new SendGridSettings();
            var sectionData = configuration.GetSection("SendGrid");
            sectionData.Bind(_sendGridSettings);
        }
        
        public async Task<HttpStatusCode> SendEmailAsync(Email emailToSend)
        {
            var client = Create();
            var fromAddress = CreateFromAddress();
            var toAddress = CreateToAddress(emailToSend.To, emailToSend.FirstName);
            
            var message = CreateSingleEmail(
                fromAddress,
                toAddress,
                emailToSend.Subject,
                emailToSend.PlainTextContent,
                emailToSend.HtmlContent);
            
            var response = await client.SendEmailAsync(message);
            
            return response.StatusCode;
        }
        
        private SendGrid.SendGridClient Create()
        {
            var key = _sendGridSettings.ApiKey;
            var client = new SendGrid.SendGridClient(key);
            return client;
        }

        private static SendGridMessage CreateSingleEmail(
            EmailAddress @from, 
            EmailAddress to, 
            string subject,
            string htmlContent,
            string plainTextContent)
        {
            var message = MailHelper.CreateSingleEmail(@from, to, subject, plainTextContent, htmlContent);
            return message;
        }

        private EmailAddress CreateFromAddress()
        {
            return new EmailAddress(_sendGridSettings.FromEmailAddress, _sendGridSettings.FromSendersName);
        }

        private EmailAddress CreateToAddress(string address, string name)
        {
            return new EmailAddress(address, name);
        }
    }
}