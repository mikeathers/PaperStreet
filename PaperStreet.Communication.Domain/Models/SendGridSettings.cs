namespace PaperStreet.Communication.Domain.Models
{
    public class SendGridSettings
    {
        public string ApiKey { get; set; }
        public string FromEmailAddress { get; set; }
        public string FromSendersName { get; set; }
    }
}