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
        private readonly IPublishEndpoint _publishEndpoint;
        public DeleteRegisterConsumer(IMediator mediator, IPublishEndpoint publishEndpoint)
        {
            _mediator = mediator;
            _publishEndpoint = publishEndpoint;
        }
        public async Task Consume(ConsumeContext<DeleteRegisterCommand> context)
        {
            var command = context.Message;
            if (command != null)
            {
                var request = new DeleteCommad(command.UserId);
                var result = await _mediator.Send(request);
                if (result != null) {
                    await _publishEndpoint.Publish(new UserDeletedEvent
                    {
                        CorrelationId = context.Message.CorrelationId
                    });
                }
                await Task.CompletedTask;
            }
        }
    }
}
