namespace PaperStreet.Domain.Core.Events.Errors
{
    public class LogErrorEvent : Event
    {
        public string UserId { get; }
        public string Message { get; set; }

        public LogErrorEvent(string userId, string message)
        {
            UserId = userId;
            Message = message;
        }
    }
}