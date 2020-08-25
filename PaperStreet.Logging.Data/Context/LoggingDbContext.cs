using Microsoft.EntityFrameworkCore;
using PaperStreet.Logging.Domain.Models;

namespace PaperStreet.Logging.Data.Context
{
    public class LoggingDbContext : DbContext
    {
        public LoggingDbContext(DbContextOptions options) : base(options)
        {
            
        }

        public LoggingDbContext()
        {
            
        }

        public DbSet<AuthenticationLog> AuthenticationLogs { get; set; }
        public DbSet<ErrorLog> ErrorLogs { get; set; }
    }
}