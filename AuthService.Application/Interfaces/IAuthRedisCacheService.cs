using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Interfaces
{
    public interface IAuthRedisCacheService
    {
        Task SetTokenAsync(string sessionId, string token, TimeSpan? expiration = null);
        Task<T?> GetTokenAsync<T>(string sessionId);
        Task RemoveTokenAsync(string sessionId);
    }
}
