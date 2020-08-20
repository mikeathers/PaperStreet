namespace PaperStreet.Domain.Core.Models
{
    public class Email
    {
        public string To { get; set; }
        public string FirstName { get; set; }
        public string UserId { get; set; }
        public string Subject { get; set; }
        public string HtmlContent { get; set; }
        public string PlainTextContent { get; set; }
    }
}