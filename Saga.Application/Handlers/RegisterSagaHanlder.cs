using MassTransit;
using MassTransit.Transports;
using MediatR;
using RegisterConstracts.Commands;
using SagaCoordinator.Application.Commands;
using SagaCoordinator.Application.Dtos;
using SagaCoordinator.Application.HealthChecks;
using SagaCoordinator.Application.Interfaces;
using SagaCoordinator.Domain.Constracts.StartSaga;
using SagaCoordinator.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SagaCoordinator.Application.Handlers
{
    public class RegisterSagaHanlder(IUnitOfWork unitOfWork, IPublishEndpoint publishEndpoint, IExternalHealthChecker externalHealthChecker) : IRequestHandler<RegisterUserCommandSaga, ModelResult>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;
        private readonly IExternalHealthChecker _externalHealthChecker = externalHealthChecker;
        public async Task<ModelResult> Handle(RegisterUserCommandSaga request, CancellationToken cancellationToken)
        {
            var result = await _externalHealthChecker.CheckHealthAsync();
            if (!result)
            {
                return new ModelResult
                {
                    Message = "Auth service is not ready!",
                    CorrelationId = null
                };
            }
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
                await _unitOfWork.SagaRepository!.AddNewSaga(command.CorrelationId, TypeSaga.Register, "Register processing");

                await _publishEndpoint.Publish(command, cancellationToken);

                await _unitOfWork.SaveChangesAsync(cancellationToken); 
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
