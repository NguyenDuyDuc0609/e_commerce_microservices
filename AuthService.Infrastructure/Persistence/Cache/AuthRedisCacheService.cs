using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AuthService.Application.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
namespace AuthService.Infrastructure.Persistence.Cache
{
    public class AuthRedisCacheService : IAuthRedisCacheService
    {
        private readonly IDistributedCache _cache;
        public AuthRedisCacheService(IDistributedCache cache)
        {
            _cache = cache;
        }
        public async Task<T?> GetTokenAsync<T>(string sessionId)
        {
            string key = $"token:{sessionId}";
            var jsonToken = await _cache.GetStringAsync(key);
            if (jsonToken is null) return default;

            if (typeof(T) == typeof(string))
                return (T)(object)jsonToken;

            return JsonSerializer.Deserialize<T>(jsonToken);
        }

        public async Task RemoveTokenAsync(string sessionId)
        {
            string key = $"token:{sessionId}";
            await _cache.RemoveAsync(key);
        }

        public async Task SetTokenAsync(string sessionId, string token, TimeSpan? expiration = null)
        {
            string key = $"token:{sessionId}";
            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromDays(7)
            };
            string jsonToken = token is string str ? str : JsonSerializer.Serialize(token);
            await _cache.SetStringAsync(key, jsonToken, cacheOptions);
        }
    }
}
