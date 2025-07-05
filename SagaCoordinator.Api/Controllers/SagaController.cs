
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SagaCoordinator.Application.Commands;
using SagaCoordinator.Application.Dtos;
using System.Runtime.InteropServices;

namespace SagaCoordinator.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SagaController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet("status")]
        public IActionResult GetStatus()
        {
            return Ok("Saga Coordinator is running");
        }
        [HttpPost("register-saga")]
        public async Task<IActionResult> RegisterSaga([FromBody] RegisterUserCommandSaga registerDto)
        {
            var result = await _mediator.Send(registerDto);
            return Ok(result);
        }
        [HttpPost("forgot-password-saga")]
        public async Task<IActionResult> ForgotPasswordSaga([FromBody] ForgotPasswordSagaCommand forgotPasswordDto)
        {
            var result = await _mediator.Send(forgotPasswordDto);
            return Ok(result);
        }
    }
}
