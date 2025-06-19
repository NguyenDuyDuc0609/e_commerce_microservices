using MediatR;
using NotificationService.Application.Features.Dtos;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Application.Features.Notification.Commands
{
    public record NotificationSendMailCommand(NotificationMessage NotificationMessage) : IRequest<NotificationResult>;
}
