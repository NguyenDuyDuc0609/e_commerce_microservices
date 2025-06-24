using SagaCoordinator.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SagaCoordinator.Domain.Constracts.UpdateStatus
{
    public class UpdateStatusSaga
    {
        public Guid CorrelationId { get; set; }
        public TypeSaga TypeSaga { get; set; }
        public StatusSaga Status { get; private set; }
    }
}
