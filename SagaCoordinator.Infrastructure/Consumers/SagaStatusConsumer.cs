using MassTransit;
using SagaCoordinator.Domain.Constracts.UpdateStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SagaCoordinator.Infrastructure.Consumers
{
    public class SagaStatusConsumer : IConsumer<UpdateStatusSaga>
    {
        public Task Consume(ConsumeContext<UpdateStatusSaga> context)
        {
            throw new NotImplementedException();
        }
    }
}
