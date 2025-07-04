using MassTransit;
using MediatR;
using NotificationService.Application.Features.Dtos;
using NotificationService.Application.Features.Notification.Commands;
using NotificationService.Application.Interfaces;
using NotificationService.Domain.Enums;
using RegisterConstracts.Commands;
using RegisterConstracts.Events;
using RegisterConstracts.Events.ForgotPasswordEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Infrastructure.Consumers
{
    public class SendMailForgotPasswordConsumer(IUnitOfWork unitOfWork, IMediator mediator, IPublishEndpoint publishEndpoint) : IConsumer<SendTokenCommand>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMediator _mediator = mediator;
        private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;

        public async Task Consume(ConsumeContext<SendTokenCommand> context)
        {
            var result = await _mediator.Send(new NotificationSendMailCommand(
                    new NotificationMessage(
                        NotificationType.RegisterEmail,
                        null,
                        null,
                        context.Message.Token!,
                        context.Message.Email!
                    )
                ));
            if (result.IsSuccess)
            {
                await _publishEndpoint.Publish(new SendTokenEvent
                {
                    CorrelationId = context.Message.CorrelationId,
                    Message = result.Message,
                });
                await _unitOfWork.SaveChangesAsync();
            }
            else
            {
                await _publishEndpoint.Publish(new SendTokenFailed
                {
                    CorrelationId = context.Message.CorrelationId,
                    Message = result.Message,
                });
                await _unitOfWork.SaveChangesAsync();
            }
        }
    }
}
