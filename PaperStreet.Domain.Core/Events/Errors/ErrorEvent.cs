namespace PaperStreet.Domain.Core.Events.Errors
{
    public class ErrorEvent : Event
    {
        public string UserId { get; }
        public string Message { get; set; }

        public ErrorEvent(string userId)
        {
            UserId = userId;
        }
    }
}