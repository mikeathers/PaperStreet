using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PaperStreet.Logging.Application.Interfaces;
using PaperStreet.Logging.Data.Context;
using PaperStreet.Logging.Domain.Models;

namespace PaperStreet.Logging.Data.Repository
{
    public class ErrorLogRepository : IErrorLogRepository
    {
        private readonly LoggingDbContext _context;

        public ErrorLogRepository(LoggingDbContext context)
        {
            _context = context;
        }

        public async Task<ErrorLog> SaveErrorLog(ErrorLog errorLog)
        {
            await _context.ErrorLogs.AddAsync(errorLog);
            var success = await _context.SaveChangesAsync() > 0;
            if (success) return errorLog;
            throw new Exception("Problem saving the error log");
        }

        public async Task<List<ErrorLog>> GetAllErrorLogs()
        {
            return await _context.ErrorLogs.ToListAsync();
        }
    }
}