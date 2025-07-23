using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Distributed;
using SagaCoordinator.Application.Interfaces;
using SagaCoordinator.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SagaCoordinator.Infrastructure.Repositories
{
    public class SagaRedis(IDistributedCache distributedCache) : ISagaRedis
    {
        private readonly IDistributedCache _distributedCache = distributedCache;

        public async Task ChangeSagaStatus(Guid correlationId, object saga)
        {
            await _distributedCache.RemoveAsync(correlationId.ToString());
            await SetSagaRedis(correlationId, saga);
        }

        public async Task<T?> GetSagaRedis<T>(Guid correlationId) where T : class
        {
            string? saga = await _distributedCache.GetStringAsync(correlationId.ToString());
            return saga == null ? null : JsonSerializer.Deserialize<T>(saga);
        }

        public async Task SetSagaRedis(Guid correlationId, object saga, TimeSpan? expiration = null)
        {
            var cacheOption = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromMinutes(30)
            };
            string data = JsonSerializer.Serialize(saga);
            await _distributedCache.SetStringAsync(correlationId.ToString(), data, cacheOption);
        }
    }
}
