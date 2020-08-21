namespace PaperStreet.Domain.Core.Events.User.Logging
{
    public abstract class UserLogEvent
    {
        public abstract string LogType { get; set; }
        
        public abstract string LogMessage { get; set; }
    }
}