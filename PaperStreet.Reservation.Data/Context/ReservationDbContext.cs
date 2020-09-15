using Microsoft.EntityFrameworkCore;

namespace PaperStreet.Reservation.Data.Context
{
    public class ReservationDbContext : DbContext
    {
        public ReservationDbContext(DbContextOptions options) : base(options)
        {
            
        }

        public ReservationDbContext()
        {
            
        }
    }
}