namespace PaperStreet.Logging.Domain.Models
{
    public class AuthenticationLog : Log
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
    }
}