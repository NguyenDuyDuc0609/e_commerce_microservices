using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SagaCoordinator.Domain.Constracts.StartSaga
{
    public record StartRegisterSagaCommand(
        Guid CorrelationId,
        string? Username,
        string? Email,
        string? PasswordHash,
        string? PhoneNumber,
        string? Address
    ) : CorrelatedBy<Guid>;
}