using SagaCoordinator.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SagaCoordinator.Application.Interfaces
{
    public interface ISagaRepository
    {
        Task<object> AddNewSaga(Guid correlationId, TypeSaga typeSaga, string? message);
        Task<bool> UpdateSagaStatus(Guid correlationId, TypeSaga typeSaga, StatusSaga status, string? message);
        Task<bool> SagaExists(Guid correlationId, TypeSaga typeSaga);
        Task<StatusSaga?> GetSagaStatus(Guid correlationId, TypeSaga typeSaga);

    }
}
