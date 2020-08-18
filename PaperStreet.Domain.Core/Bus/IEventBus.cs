using System.Threading.Tasks;
using PaperStreet.Domain.Core.Commands;
using PaperStreet.Domain.Core.Events;

namespace PaperStreet.Domain.Core.Bus
{
    public interface IEventBus
    {
        Task SendCommand<T>(T command) where T : Command;

        void Publish<T>(T @event) where T : Event;

        void Subscribe<T, TH>() where T : Event where TH : IEventHandler;
    }
}