using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PaperStreet.Logging.Application.Queries.User;
using PaperStreet.Logging.Domain.Models;

namespace PaperStreet.Logging.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class LoggingController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LoggingController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpGet, Route("all-authentication-logs")]
        public async Task<ActionResult<List<AuthenticationLog>>> GetAllAuthenticationLogs()
        {
            return await _mediator.Send(new AllUserLogs.Query());
        }
    }
}