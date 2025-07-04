using MassTransit;
using MediatR;
using SagaCoordinator.Application.Commands;
using SagaCoordinator.Application.Dtos;
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
    public class ForgotPasswordSagaHandler(IUnitOfWork unitOfWork, IPublishEndpoint publishEndpoint) : IRequestHandler<ForgotPasswordSagaCommand, ModelResult>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;

        public async Task<ModelResult> Handle(ForgotPasswordSagaCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var command = new StartForgotPasswordSagaCommand(
                    Guid.NewGuid(),
                    request.Email
                );
                await _unitOfWork.SagaRedis!.SetSagaRedis(command.CorrelationId, command);
                await _unitOfWork.SagaRepository!.AddNewSaga(command.CorrelationId, TypeSaga.ForgotPassword, "Forgot password processing");
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
                return new ModelResult
                {
                    Message = ex.Message,
                    CorrelationId = null
                };
            }
        }
    }
}
