using System.Collections.Generic;
using MediatR;
using PaperStreet.Logging.Domain.Models;

namespace PaperStreet.Logging.Application.Queries
{
    public class AllAuthenticationLogsQuery : IRequest<List<AuthenticationLog>>
    {
        
    }
}