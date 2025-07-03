using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SagaCoordinator.Domain.Constracts.SagaStates
{
    public class ForgotPasswordSagaState : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public string CurrentState { get; set; } = default!;
        public string? Email { get; set; } = default!;
        public string? Token { get; set; } = default!;
        public bool IsTokenCreated { get; set; } = false;
        public bool IsTokenSent { get; set; } = false;
        public DateTime? ExpireAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ErrorMessage { get; set; } = default!;

        public ForgotPasswordSagaState(Guid correlationId)
        {
            CorrelationId = correlationId;
        }

        public ForgotPasswordSagaState()
        {
        }
    }
}
