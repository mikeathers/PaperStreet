using System.Threading.Tasks;
using PaperStreet.Domain.Core.Events;

namespace PaperStreet.Domain.Core.Bus
{
    public interface IEventHandler<in TEvent> : IEventHandler where TEvent: Event
    {
        Task Handle(TEvent @event);
    }

    public interface IEventHandler
    {
        
    }
}