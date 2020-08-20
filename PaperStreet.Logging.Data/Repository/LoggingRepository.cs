using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PaperStreet.Logging.Application.Interfaces;
using PaperStreet.Logging.Data.Context;
using PaperStreet.Logging.Domain.Models;

namespace PaperStreet.Logging.Data.Repository
{
    public class LoggingRepository : ILoggingRepository
    {
        private readonly LoggingDbContext _context;

        public LoggingRepository(LoggingDbContext context)
        {
            _context = context;
        }

        public async Task<List<AuthenticationLog>> GetAllAuthenticationLogs()
        {
            return await _context.AuthenticationLogs.ToListAsync();
        }

        public async Task<AuthenticationLog> SaveAuthenticationLog(AuthenticationLog authenticationLog)
        {
            await _context.AuthenticationLogs.AddAsync(authenticationLog);
            var success = await _context.SaveChangesAsync() > 0;
            if (success) return authenticationLog;
            throw new Exception("Problem saving the authentication log");
        }
    }
}