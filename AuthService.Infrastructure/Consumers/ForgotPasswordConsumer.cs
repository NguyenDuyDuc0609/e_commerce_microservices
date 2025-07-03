using AuthService.Application.Features.Users.Commands;
using AuthService.Application.Interfaces;
using MassTransit;
using MediatR;
using RegisterConstracts.Commands;
using RegisterConstracts.Events.ForgotPasswordEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Infrastructure.Consumers
{
    public class ForgotPasswordConsumer(IMediator mediator, IPublishEndpoint publishEndpoint, IUnitOfWork unitOfWork) : IConsumer<ForgotPasswordCommand>
    {
        private readonly IMediator _mediator = mediator;
        readonly IPublishEndpoint _publishEndpoint = publishEndpoint;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task Consume(ConsumeContext<ForgotPasswordCommand> context)
        {
            if (context.Message.Email != null)
            {
                var result = await _mediator.Send(new PasswordForgotCommand(context.Message.Email));
                if (result.IsSuccess)
                {
                    await _publishEndpoint.Publish(new CreateTokenEvent
                    {
                        CorrelationId = context.Message.CorrelationId,
                        Email = context.Message.Email,
                        Token = result.Message,
                    });
                    await _unitOfWork.SaveChangesAsync();
                }
                else
                {
                    await _publishEndpoint.Publish(new CreateTokenFailed
                    {
                        CorrelationId = context.Message.CorrelationId,
                        Message = result.Message,
                    });
                    await _unitOfWork.SaveChangesAsync();
                }
            }
            else
            {
                await _publishEndpoint.Publish(new CreateTokenFailed
                {
                    CorrelationId = context.Message.CorrelationId,
                    Message = "Email is null",
                });
                await _unitOfWork.SaveChangesAsync();
            }
        }
    }
}
