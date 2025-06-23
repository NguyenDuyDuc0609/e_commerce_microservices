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
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AuthController(IMediator mediator) {
            _mediator = mediator;
        }
        [HttpPost("Login")]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return Ok("Login success");
        }
    }
}
