using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PaperStreet.Logging.Application.Queries;
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
        
        [HttpGet("all-authentication-logs")]
        public async Task<ActionResult<List<AuthenticationLog>>> GetAllAuthenticationLogs()
        {
            return await _mediator.Send(new AllAuthenticationLogsQuery());
        }

        [HttpGet("all-error-logs")]
        public async Task<ActionResult<List<ErrorLog>>> GetAllErrorLogs()
        {
            return await _mediator.Send(new AllErrorLogsQuery());
        }
    }
}