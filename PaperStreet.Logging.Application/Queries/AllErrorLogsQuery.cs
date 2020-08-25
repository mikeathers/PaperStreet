using System.Collections.Generic;
using MediatR;
using PaperStreet.Logging.Domain.Models;

namespace PaperStreet.Logging.Application.Queries
{
    public class AllErrorLogsQuery : IRequest<List<ErrorLog>>
    {
        
    }
}