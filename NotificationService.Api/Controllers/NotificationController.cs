using MassTransit;
using MassTransit.Transports;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NotificationService.Application.Features.Dtos;
using NotificationService.Application.Features.Notification.Commands;
using NotificationService.Domain.Enums;
using RegisterConstracts.Commands;

namespace NotificationService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {

        [HttpGet("status")]
        public IActionResult GetStatus()
        {
            return Ok("Notification Service is running");
        }
    }
}
