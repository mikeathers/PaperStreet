using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PaperStreet.Logging.Data.Context;
using PaperStreet.Logging.Domain.Models;

namespace PaperStreet.Logging.Application.QueryHandlers.User
{
    public class AllUserLogs : IRequestHandler<Queries.User.AllUserLogs.Query, List<AuthenticationLog>>
    {
        private readonly LoggingDbContext _context;

        public AllUserLogs(LoggingDbContext context)
        {
            _context = context;
        }

        public async Task<List<AuthenticationLog>> Handle(Queries.User.AllUserLogs.Query request, CancellationToken cancellationToken)
        {
            return await _context.AuthenticationLogs.ToListAsync(cancellationToken: cancellationToken);
        }
    }
}