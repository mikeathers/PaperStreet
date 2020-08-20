using Microsoft.AspNetCore.Mvc;

namespace PaperStreet.Communication.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CommunicationController : ControllerBase
    {
        // GET
        public IActionResult Get()
        {
            return Ok();
        }
    }
}