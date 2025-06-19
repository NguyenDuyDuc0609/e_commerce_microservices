using MassTransit;
using MediatR;
using NotificationService.Application.Features.Dtos;
using NotificationService.Domain.Constracts;
using NotificationService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Infrastructure.Consumers
{
    public class NotificationConsumer : IConsumer<NotificationRegisterCommand>
    {
        private readonly IMediator _mediator;
        public NotificationConsumer(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task Consume(ConsumeContext<NotificationRegisterCommand> context)
        {
            var commandSendNotification = context.Message;
            var command = new NotificationMessage
                {
                UserName = commandSendNotification.Username,
                UserId = commandSendNotification.UserId,
                Email = commandSendNotification.Email,
                HashEmail = commandSendNotification.HashEmail,
                Type = NotificationType.RegisterEmail
                };
            var result = await _mediator.Send(command);
            }
        }
    }
