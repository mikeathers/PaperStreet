using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaperStreet.Authentication.Application.Commands;
using PaperStreet.Authentication.Application.Interfaces;
using PaperStreet.Authentication.Application.Queries;
using PaperStreet.Authentication.Domain.Models;

namespace PaperStreet.Authentication.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthenticationController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ITokenHandler _tokenHandler;

        public AuthenticationController(IMediator mediator, ITokenHandler tokenHandler)
        {
            _mediator = mediator;
            _tokenHandler = tokenHandler;
        }
        
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(RegisterUserCommand command)
        {
            var user = await _mediator.Send(command);
            return Ok(user);
        }
        
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<User>> Login(LoginUserQuery query)
        {
            var user = await _mediator.Send(query);
            return Ok(user);
        }
        
        [AllowAnonymous]
        [HttpGet("confirm-email/{userId}/{emailConfirmationCode}")]
        public async Task<ActionResult<User>> ConfirmEmail(string userId, string emailConfirmationCode)
        {
            var confirmEmailQuery = new ConfirmEmailCommand
            {
                UserId = userId,
                EmailConfirmationCode = emailConfirmationCode
            };
            
            var user = await _mediator.Send(confirmEmailQuery);
            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost("forgot-password")]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordQuery forgotPasswordQuery)
        {
            var resetEmailSent = await _mediator.Send(forgotPasswordQuery);
            return Ok(resetEmailSent);
        }
        
        
        [HttpPost("reset-password")]
        public async Task<ActionResult> ResetPassword(ResetPasswordCommand resetPasswordQuery)
        {
            var user = await _mediator.Send(resetPasswordQuery);
            return Ok(user);
        }

        [HttpPost("change-password")]
        public async Task<ActionResult> ChangePassword(ChangePasswordCommand changePasswordCommand)
        {
            var user = await _mediator.Send(changePasswordCommand);
            return Ok(user);
        }
        
        [AllowAnonymous]
        [HttpPost("refresh")]
        public async Task<ActionResult<User>> Refresh(RefreshTokenQuery query)
        {
            var principal = _tokenHandler.GetPrincipalFromExpiredToken(query.Token);
            query.Email = principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            return await _mediator.Send(query);
        }
    }
}