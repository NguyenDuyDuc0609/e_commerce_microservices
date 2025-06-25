using AuthService.Application.Features.Users.Commands;
using AuthService.Application.Features.Users.Dtos;
using MassTransit;
using MediatR;
using RegisterConstracts.Commands;
using RegisterConstracts.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Infrastructure.Consumers
{
    public class RegisterConsumer : IConsumer<RegisterUserCommand>
    {
        private readonly IMediator _mediator;
        private readonly IPublishEndpoint _publishEndpoint;
        public RegisterConsumer(IMediator mediator, IPublishEndpoint publishEndpoint)
        {
            _mediator = mediator;
            _publishEndpoint = publishEndpoint;
        }
        public async Task Consume(ConsumeContext<RegisterUserCommand> context)
        {
            var commandRegister = context.Message;
            if (commandRegister != null)
            {
                var command = new RegisterUserCommands(
                commandRegister.Username,
                commandRegister.Email,
                commandRegister.PasswordHash,
                commandRegister.PhoneNumber,
                commandRegister.Address
                );
                var result = await _mediator.Send(command);
                if (result.IsSuccess)
                {
                    await _publishEndpoint.Publish(new UserCreatedEvent
                    {
                        UserId = result.UserId,
                        CorrelationId = context.Message.CorrelationId,
                        Username = commandRegister.Username,
                        Email = commandRegister.Email,
                        HashEmail = result.HashEmail,
                        PhoneNumber = commandRegister.PhoneNumber,
                        Address = commandRegister.Address
                    });
                }
                else
                {
                    await _publishEndpoint.Publish(new UserCreationFailedEvent
                    {
                        CorrelationId = context.Message.CorrelationId,
                        Message = result.Message,
                    });
                }
            }
            else
            {
                await _publishEndpoint.Publish(new UserCreationFailedEvent
                {
                    CorrelationId = context.Message.CorrelationId,
                    Message = "Invalid Register Command"
                });
            }
        }
    }
}
