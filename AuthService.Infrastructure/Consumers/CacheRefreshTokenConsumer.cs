using AuthService.Application.Features.Users.Dtos;
using AuthService.Application.Interfaces;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Infrastructure.Consumers
{
    public class CacheRefreshTokenConsumer(IAuthRedisCacheService authRedisCacheService) : IConsumer<UpdateCache>
    {
        private readonly IAuthRedisCacheService _authRedisCacheService = authRedisCacheService;

        public async Task Consume(ConsumeContext<UpdateCache> context)
        {
            await _authRedisCacheService.SetTokenAsync(context.Message.UserId.ToString(), context.Message.RefreshToken ?? string.Empty, TimeSpan.FromDays(7));
            await Task.CompletedTask;
        }
    }
}
