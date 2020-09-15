using System.Threading.Tasks;
using PaperStreet.Reservations.Domain.Models;

namespace PaperStreet.Reservations.Application.Interfaces
{
    public interface IReservationRepository
    {
        Task<bool> SaveReservation(Reservation reservation);
    }
}