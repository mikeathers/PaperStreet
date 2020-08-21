namespace PaperStreet.Communication.Domain.Models
{
    public class SendGridSettings
    {
        public string ApiKey { get; set; }
        public string FromSenderAddress { get; set; }
        public string FromSenderName { get; set; }
    }
}