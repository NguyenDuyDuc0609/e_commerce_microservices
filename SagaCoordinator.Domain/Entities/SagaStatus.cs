using SagaCoordinator.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SagaCoordinator.Domain.Entities
{
    public class SagaStatus
    {
        public Guid CorrelationId { get; set; }
        public TypeSaga TypeSaga { get; set; }
        public StatusSaga Status { get; private set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; private set; }
        public SagaStatus(Guid correlationId, TypeSaga typeSaga, StatusSaga status)
        {
            CorrelationId = correlationId;
            TypeSaga = typeSaga;
            Status = status;
            CreatedAt = DateTime.UtcNow;
        }
        public SagaStatus()
        {
        }
        public void UpdateStatus(StatusSaga newStatus)
        {
            Status = newStatus;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
