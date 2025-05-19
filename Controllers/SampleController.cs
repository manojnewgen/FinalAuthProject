using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinalAuthProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SampleController : ControllerBase
    {
        [HttpGet("admin")]
        [Authorize(Policy = "AdminPolicy")]
        public IActionResult AdminEndpoint()
        {
            return Ok("This endpoint is accessible only to Admins.");
        }

        [HttpGet("user")]
        [Authorize(Policy = "UserPolicy")]
        public IActionResult UserEndpoint()
        {
            return Ok("This endpoint is accessible only to Users.");
        }

        [HttpGet("guest")]
        [Authorize(Policy = "GuestPolicy")]
        public IActionResult GuestEndpoint()
        {
            return Ok("This endpoint is accessible only to Guests.");
        }
    }
}
