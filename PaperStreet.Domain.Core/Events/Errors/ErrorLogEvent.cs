namespace PaperStreet.Domain.Core.Events.Errors
{
    public class ErrorLogEvent : Event
    {
        public string UserId { get; }
        public string Message { get; set; }

        public ErrorLogEvent(string userId, string message)
        {
            UserId = userId;
            Message = message;
        }
    }
}