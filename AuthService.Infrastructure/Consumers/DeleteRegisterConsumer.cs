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
    public class DeleteRegisterConsumer : IConsumer<DeleteRegisterCommand>
    {
        private readonly IMediator _mediator;
        public DeleteRegisterConsumer(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task Consume(ConsumeContext<DeleteRegisterCommand> context)
        {
            var command = context.Message;
            if (command != null)
            {
                var request = new DeleteCommad(command.UserId);
                var result = await _mediator.Send(request);
                if (result != null) {
                    await context.RespondAsync(new RegisterConsumerResponse
                    {
                        CorrelationId = context.Message.CorrelationId,
                        IsSuccess = result.IsSuccess,
                        Message = result.Message
                    });
                }
                else
                {
                    await context.RespondAsync(new RegisterConsumerResponse
                    {
                        CorrelationId = context.Message.CorrelationId,
                        IsSuccess = false,
                        Message = "User deletion failed or user does not exist."
                    });
                }
                await Task.CompletedTask;
            }
        }
    }
}
