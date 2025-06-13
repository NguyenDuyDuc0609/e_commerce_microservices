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
        public async Task<T?> GetTokenAsync<T>(string key)
        {
            string? jsonToken = await _cache.GetStringAsync(key);
            return jsonToken is null ? default : JsonSerializer.Deserialize<T>(jsonToken);
        }

        public async Task RemoveTokenAsync(string key)
        {
            await _cache.RemoveAsync(key);
        }

        public async Task SetTokenAsync(string key, string token, TimeSpan? expiration = null)
        {
            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromDays(7)
            };
            string jsonToken = JsonSerializer.Serialize(token);
            await _cache.SetStringAsync(key, jsonToken, cacheOptions);
        }
    }
}
