using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SagaCoordinator.Application.Interfaces
{
    public interface ISagaRedis
    {
        Task SetSagaRedis(Guid correlationId, object saga, TimeSpan? expiration = null);
        Task<T?> GetSagaRedis<T>(Guid correlationId) where T : class;

    }
}
