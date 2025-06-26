using AuthService.Application.Features.Users.Commands;
using AuthService.Application.Features.Users.Dtos;
using AuthService.Application.Interfaces;
using MassTransit;
using MassTransit.Introspection;
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
    public class DeleteRegisterConsumer(IMediator mediator, IPublishEndpoint publishEndpoint, IUnitOfWork unitOfWork) : IConsumer<DeleteRegisterCommand>
    {
        private readonly IMediator _mediator = mediator;
        private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

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
                    await _unitOfWork.SaveChangesAsync();
                    await _unitOfWork.CommitAsync();
                }
            }
        }
    }
}
