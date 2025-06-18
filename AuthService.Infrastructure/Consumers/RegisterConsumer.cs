using AuthService.Application.Features.Users.Commands;
using AuthService.Application.Features.Users.Dtos;
using AuthService.Domain.Constracts;
using MassTransit;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Infrastructure.Consumers
{
    public class RegisterConsumer : IConsumer<RegisterCommand>
    {
        private readonly IMediator _mediator;
        public RegisterConsumer(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task Consume(ConsumeContext<RegisterCommand> context)
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
                    await context.RespondAsync(new RegisterConsumerResponse
                    {
                        CorrelationId = context.Message.CorrelationId,
                        IsSuccess = true,
                        Message = result.Message,
                        UserId = result.UserId
                    });
                }
                else
                {
                    await context.RespondAsync(new RegisterConsumerResponse
                    {
                        CorrelationId = context.Message.CorrelationId,
                        IsSuccess = false,
                        Message = result.Message
                    });
                }
            }
            else
            {
                await context.RespondAsync(new RegisterConsumerResponse
                {
                    CorrelationId = context.Message.CorrelationId,
                    IsSuccess = false,
                    Message = "Invalid registration request."
                });
            }
            await Task.CompletedTask;
        }
    }
}
