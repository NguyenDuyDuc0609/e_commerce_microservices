using MassTransit;
using MediatR;
using SagaCoordinator.Application.Interfaces;
using SagaCoordinator.Domain.Constracts.UpdateStatus;
using SagaCoordinator.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SagaCoordinator.Infrastructure.Consumers
{
    public class SagaStatusConsumer : IConsumer<UpdateStatusSaga>
    {
        private readonly IUnitOfWork _unitOfWork;
        public SagaStatusConsumer(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;
        public async Task Consume(ConsumeContext<UpdateStatusSaga> context)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _unitOfWork.SagaRepository!.UpdateSagaStatus(
                    context.Message.CorrelationId,
                    context.Message.TypeSaga,
                    context.Message.Status,
                    context.Message.Message
                );
                await _unitOfWork.SagaRepository!.UpdateSagaStatus(context.Message.CorrelationId,context.Message.TypeSaga, context.Message.Status, context.Message.Message);

                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                throw new Exception("Error occurred while processing saga status update", ex);
            }
        }

    }
}
