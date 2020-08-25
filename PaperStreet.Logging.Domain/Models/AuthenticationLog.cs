namespace PaperStreet.Logging.Domain.Models
{
    public class AuthenticationLog : Log
    {
        public string LogType { get; set; }
        public string LogMessage { get; set; }
    }
}