using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PaperStreet.Authentication.Application.Commands;
using PaperStreet.Authentication.Domain.Models;

namespace PaperStreet.Authentication.Api.Controllers
{
    public class AuthenticationController : BaseController
    {
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(Register.Command command)
        {
            return await Mediator.Send(command);
        }
    }
}