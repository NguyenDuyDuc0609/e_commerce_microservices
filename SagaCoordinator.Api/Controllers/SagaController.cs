
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SagaCoordinator.Domain.Constracts.Register;
using System.Runtime.InteropServices;

namespace SagaCoordinator.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SagaController : ControllerBase
    {
        private IMediator _mediator;
        public SagaController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet("status")]
        public IActionResult GetStatus()
        {
            return Ok("Saga Coordinator is running");
        }
        [HttpPost("register-saga")]
        public async Task<IActionResult> RegisterSaga([FromBody] RegisterUserCommand registerUserCommand)
        {
            var result = await _mediator.Send(registerUserCommand);
            return Ok(result);
        }
    }
}
