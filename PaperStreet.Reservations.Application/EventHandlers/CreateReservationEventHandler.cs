using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PaperStreet.Reservations.Application.Events;

namespace PaperStreet.Reservations.Application.EventHandlers
{
    public class CreateReservationEventHandler : IRequestHandler<CreateReservationEvent, bool>
    {
        public Task<bool> Handle(CreateReservationEvent request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}