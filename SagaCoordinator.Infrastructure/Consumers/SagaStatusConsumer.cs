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
        public SagaStatusConsumer(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task Consume(ConsumeContext<UpdateStatusSaga> context)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var saga = await _unitOfWork.SagaRepository.AddNewSaga(
                    context.Message.CorrelationId,
                    context.Message.TypeSaga,
                    context.Message.Message
                );

                if (saga == null)
                {
                    throw new Exception("AddNewSaga returned null");
                }

                await _unitOfWork.SagaRedis.SetSagaRedis(context.Message.CorrelationId, saga);

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
