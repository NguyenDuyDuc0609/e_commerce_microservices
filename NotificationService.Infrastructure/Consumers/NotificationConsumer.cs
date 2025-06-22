using MailKit.Search;
using MassTransit;
using MassTransit.Transports;
using MediatR;
using NotificationService.Application.Features.Dtos;
using NotificationService.Application.Features.Notification.Commands;
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
        public readonly IPublishEndpoint _publishEndpoint;
        public NotificationConsumer(IMediator mediator, IPublishEndpoint publishEndpoint)
        {
            _mediator = mediator;
            _publishEndpoint = publishEndpoint;
        }
        public async Task Consume(ConsumeContext<NotificationRegisterCommand> context)
        {
            var commandNotification = context.Message;
            var command = new NotificationSendMailCommand
                (
                new NotificationMessage(
                    NotificationType.RegisterEmail,
                    commandNotification.Username,
                    commandNotification.UserId,
                    commandNotification.HashEmail,
                    commandNotification.Email
)
                );
            var result = await _mediator.Send(command);
            if (result.IsSuccess)
            {
                await _publishEndpoint.Publish(new NotificationSuccess
                {
                    CorrelationId = context.Message.CorrelationId,
                });
            }
            else
            {
                await _publishEndpoint.Publish(new NotificationFailed
                {
                    CorrelationId = context.Message.CorrelationId,
                    Message = result.Message,
                    UserId = context.Message.UserId
                });

            }
        }
    }
}
