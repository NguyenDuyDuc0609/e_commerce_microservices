using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SagaCoordinator.Domain.Constracts.StartSaga
{
    public record StartForgotPasswordSagaCommand(
        Guid CorrelationId,
        string? Email
        ) : CorrelatedBy<Guid>;
}
