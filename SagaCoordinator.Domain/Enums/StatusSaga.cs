using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SagaCoordinator.Domain.Enums
{
    public enum StatusSaga
    {
        Pending,
        InProgres,
        Completed,
        Failed,
        Cancelled
    }
}
