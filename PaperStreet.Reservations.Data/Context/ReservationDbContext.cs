using Microsoft.EntityFrameworkCore;

namespace PaperStreet.Reservations.Data.Context
{
    public class ReservationDbContext : DbContext
    {
        public ReservationDbContext(DbContextOptions options) : base(options)
        {
            
        }

        public ReservationDbContext()
        {
            
        }

        public DbSet<Domain.Models.Reservation> Reservations { get; set; }
    }
}