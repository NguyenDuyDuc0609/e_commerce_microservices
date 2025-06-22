using Automatonymous;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SagaCoordinator.Domain.Constracts.SagaStates
{
    public class RegisterSagaState : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public string CurrentState { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public Guid? UserId { get; set; } 
        public string? HashEmail { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
    }
}
