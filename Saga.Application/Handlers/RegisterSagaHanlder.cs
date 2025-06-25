using MassTransit;
using MediatR;
using RegisterConstracts.Commands;
using SagaCoordinator.Application.Commands;
using SagaCoordinator.Application.Dtos;
using SagaCoordinator.Domain.Constracts.StartSaga;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SagaCoordinator.Application.Handlers
{
    public class RegisterSagaHanlder : IRequestHandler<RegisterUserCommandSaga, ModelResult>
    {
        private readonly IPublishEndpoint _publishEndpoint;
        public RegisterSagaHanlder(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }
        public async Task<ModelResult> Handle(RegisterUserCommandSaga request, CancellationToken cancellationToken)
        {
            try
            {
                var command = new StartRegisterSagaCommand(
                    Guid.NewGuid(),
                    request.Username,
                    request.Email,
                    request.PasswordHash,
                    request.PhoneNumber,
                    request.Address
                );
                await _publishEndpoint.Publish(command, cancellationToken);
                return new ModelResult
                {
                    Message = "User registration is processing",
                    CorrelationId = command.CorrelationId
                };
            }
            catch (Exception ex)
            {
                var notification = new ModelResult
                {
                    Message = ex.Message,
                    CorrelationId = null
                };
                return notification;
            }
        }
    }
}
