using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GatewayController : ControllerBase
    {
        // This controller can be used to route requests to different services
        // For example, you can use it to forward requests to AuthService or SagaCoordinator

        [HttpGet("status")]
        public IActionResult GetStatus()
        {
            return Ok("Gateway is running");
        }

        // Add more endpoints as needed to route requests to other services
    }
}
