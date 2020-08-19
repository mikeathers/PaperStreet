using System;

namespace PaperStreet.Domain.Core.Events
{
    public abstract class Event
    {
            public DateTime Timestamp { get; protected set; }
            public string MessageType { get; protected set; }

        protected Event()
        {
            Timestamp = DateTime.Now;
            MessageType = GetType().Name;
        }
    }
}