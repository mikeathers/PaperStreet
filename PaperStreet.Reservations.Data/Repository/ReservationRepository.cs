using System;
using System.Threading.Tasks;
using PaperStreet.Reservations.Application.Interfaces;
using PaperStreet.Reservations.Data.Context;
using PaperStreet.Reservations.Domain.Models;

namespace PaperStreet.Reservations.Data.Repository
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly ReservationDbContext _context;

        public ReservationRepository(ReservationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> SaveReservation(Reservation reservation)
        {
            await _context.Reservations.AddAsync(reservation);
            var success = await _context.SaveChangesAsync() > 0;
            if (success) return true;
            throw new Exception("Problem saving the reservation");
        }
    }
}