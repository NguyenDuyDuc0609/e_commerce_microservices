using MailKit.Search;
using MassTransit;
using MassTransit.Transports;
using MediatR;
using NotificationService.Application.Features.Dtos;
using NotificationService.Application.Features.Notification.Commands;
using NotificationService.Application.Interfaces;
using NotificationService.Domain.Enums;
using RegisterConstracts.Commands;
using RegisterConstracts.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Infrastructure.Consumers
{
    public class NotificationConsumer(IMediator mediator, IPublishEndpoint publishEndpoint, IUnitOfWork unitOfWork) : IConsumer<NotificationRegisterCommand>
    {
        private readonly IMediator _mediator = mediator;
        public readonly IPublishEndpoint _publishEndpoint = publishEndpoint;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task Consume(ConsumeContext<NotificationRegisterCommand> context)
        {
            var commandNotification = context.Message;
            var command = new NotificationSendMailCommand
                (
                new NotificationMessage(
                    NotificationType.RegisterEmail,
                    commandNotification.Username,
                    commandNotification.UserId,
                    commandNotification.HashEmail!,
                    commandNotification.Email!
)
                );
            var result = await _mediator.Send(command);
            if (result.IsSuccess)
            {
                await _publishEndpoint.Publish(new NotificationEvent
                {
                    CorrelationId = context.Message.CorrelationId,
                    Message = result.Message,
                });
                await _unitOfWork.CommitAsync();
            }
            else
            {
                await _publishEndpoint.Publish(new NotificationFailed
                {
                    CorrelationId = context.Message.CorrelationId,
                    Message = result.Message,
                    UserId = context.Message.UserId
                });
                await _unitOfWork.CommitAsync();
            }
        }
    }
}
