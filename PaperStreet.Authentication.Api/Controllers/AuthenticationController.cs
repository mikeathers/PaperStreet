using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaperStreet.Authentication.Application.Commands;
using PaperStreet.Authentication.Application.Queries;
using PaperStreet.Authentication.Domain.Models;

namespace PaperStreet.Authentication.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthenticationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(RegisterUser.Command command)
        {
            var user = await _mediator.Send(command);
            return Ok(user);
        }
        
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<User>> Login(LoginUser.Query query)
        {
            var user = await _mediator.Send(query);
            return Ok(user);
        }
        
        [AllowAnonymous]
        [HttpGet("confirm-email/{userId}/{emailConfirmationCode}")]
        public async Task<ActionResult<User>> ConfirmEmail(string userId, string emailConfirmationCode)
        {
            var confirmEmailQuery = new ConfirmEmail.Command
            {
                UserId = userId,
                EmailConfirmationCode = emailConfirmationCode
            };
            
            var user = await _mediator.Send(confirmEmailQuery);
            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost("forgot-password")]
        public async Task<ActionResult> ForgotPassword(ForgotPassword.Query forgotPasswordQuery)
        {
            var resetEmailSent = await _mediator.Send(forgotPasswordQuery);
            return Ok(resetEmailSent);
        }
    }
}