using AuthService.Application.Features.Handler;
using AuthService.Application.Features.Users.Commands;
using AuthService.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace AuthService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AuthController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginUserCommand loginUserCommand)
        {
            var result = await _mediator.Send(loginUserCommand);
            return Ok(result);
        }
        [HttpPost("refresh-token")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand refreshTokenCommand)
        {
            var result = await _mediator.Send(refreshTokenCommand);
            return Ok(result);
        }
        [HttpPost("verify-account/{token}")]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyAccount(string token)
        {
            var email = new VerifyAccountCommand(token);
            var result = await _mediator.Send(email);
            return Ok(result);
        }
        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand resetPasswordCommand)
        {
            var result = await _mediator.Send(resetPasswordCommand);
            return Ok(result);
        }
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand changePasswordCommand)
        {
            var result = await _mediator.Send(changePasswordCommand);
            return Ok(result);
        }
    }
}
