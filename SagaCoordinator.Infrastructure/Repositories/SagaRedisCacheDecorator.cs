using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using SagaCoordinator.Application.Interfaces;
using SagaCoordinator.Domain.Entities;
using SagaCoordinator.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static MassTransit.Logging.OperationName;

namespace SagaCoordinator.Infrastructure.Repositories
{
    public class SagaRedisCacheDecorator(ISagaRepository sagaRepository, IDistributedCache distributedCache) : ISagaRepository
    {
        private readonly ISagaRepository _sagaRepository = sagaRepository;
        private readonly IDistributedCache _distributedCache = distributedCache;

        public async Task<object> AddNewSaga(Guid correlationId, TypeSaga typeSaga, string? message)
        {
            await SetSagaRedis(correlationId, StatusSaga.Pending);
            var saga = new SagaStatus(correlationId, typeSaga, StatusSaga.Pending, message);
            await _sagaRepository.AddNewSaga(correlationId, typeSaga, message);
            return saga;
        }

        public async Task<StatusSaga?> GetSagaStatus(Guid correlationId)
        {
            string? cachedData = await _distributedCache.GetStringAsync(correlationId.ToString());
            if (!string.IsNullOrEmpty(cachedData))
            {
                try
                {
                    return JsonSerializer.Deserialize<StatusSaga>(cachedData);
                }
                catch (JsonException)
                {
                    return null;
                }
            }
            var sagaStatus = await _sagaRepository.GetSagaStatus(correlationId);
            if (sagaStatus.HasValue)
            {
                await SetSagaRedis(correlationId, sagaStatus.Value);
            }
            return sagaStatus;
        }

        public async Task<bool> UpdateSagaStatus(Guid correlationId, TypeSaga typeSaga, StatusSaga status, string? message)
        {
            var saga = await _sagaRepository.UpdateSagaStatus(correlationId, typeSaga, status, message);
            if(!saga) return false;
            await _distributedCache.RemoveAsync(correlationId.ToString());
            await SetSagaRedis(correlationId, status);
            return true;
        }
        private async Task SetSagaRedis(Guid correlationId, StatusSaga status, TimeSpan? expiration = null)
        {
            var cacheOption = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromMinutes(30)
            };
            string data = JsonSerializer.Serialize(status);
            await _distributedCache.SetStringAsync(correlationId.ToString(), data, cacheOption);
        }
    }
}
