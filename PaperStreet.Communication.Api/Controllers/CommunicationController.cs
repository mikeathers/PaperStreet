using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PaperStreet.Communication.Application.Interfaces;
using PaperStreet.Domain.Core.Models;

namespace PaperStreet.Communication.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CommunicationController : ControllerBase
    {
       
        [HttpPost]
        public async Task<IActionResult> SendEmail(Email emailToSend, [FromServices] IEmailSender sender)
        {
            await sender.SendEmail(emailToSend);
            return Ok();
        }
    }
}