using MailKit.Search;
using MassTransit;
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
        public NotificationConsumer(IMediator mediator)
        {
            _mediator = mediator;
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
                await context.RespondAsync(new NotificationConsumerResponse
                {
                    CorrelationId = context.Message.CorrelationId,
                    IsSuccess = true,
                    Message = result.Message
                });
            }
            else
            {
                await context.RespondAsync(new NotificationConsumerResponse
                {
                    CorrelationId = context.Message.CorrelationId,
                    IsSuccess = false,
                    Message = result.Message
                });

            }
        }
    }
}
