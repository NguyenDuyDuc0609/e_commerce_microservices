using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Interfaces
{
    public interface IAuthRedisCacheService
    {
        Task SetTokenAsync(string key, string token, TimeSpan? expiration = null);
        Task<T?> GetTokenAsync<T>(string key);
        Task RemoveTokenAsync(string key);
    }
}
