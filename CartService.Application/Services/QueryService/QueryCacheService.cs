using CartService.Application.Features.Dtos;
using CartService.Application.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CartService.Application.Services.QueryService
{
    public class QueryCacheService(IQueryService queryService, IDistributedCache distributedCache, IConnectionMultiplexer connectionMultiplexer) : IQueryService
    {
        private readonly IQueryService _queryService = queryService ?? throw new ArgumentNullException(nameof(queryService));
        private readonly IDistributedCache _cache = distributedCache ?? throw new ArgumentNullException(nameof(distributedCache));
        private readonly IDatabase _redisDb = connectionMultiplexer.GetDatabase() ?? throw new ArgumentNullException(nameof(connectionMultiplexer));

        public async Task<CartDto> GetCartPage(Guid userId, int pageNumber, int pageSize)
        {
            var keyCartItems = $"cart:items:{userId}";
            long start = (long)(pageNumber - 1) * pageSize;
            long end = start + pageSize - 1;
            var cachedItemsJson = await _redisDb.SortedSetRangeByRankWithScoresAsync(keyCartItems, start, end, Order.Ascending);
            if (cachedItemsJson.Length == 0)
            {
                var cartDtos = await _queryService.GetCartPage(userId, pageNumber, pageSize) ?? throw new InvalidOperationException("Cart not found.");
                await SetCacheAsync(cartDtos);

                var pagedItems = cartDtos.Items
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                return new CartDto
                {
                    CartId = cartDtos.CartId,
                    UserId = cartDtos.UserId,
                    TotalAmount = cartDtos.TotalAmount,
                    Items = pagedItems
                };
            }
            var pagedItemsFromCache = new List<ItemDto>();
            foreach (var itemEntry in cachedItemsJson)
            {
                var item = JsonSerializer.Deserialize<ItemDto>(itemEntry.Element!);
                if (item != null)
                {
                    pagedItemsFromCache.Add(item);
                }
            }
            var keyCartInfo = $"cart:info:{userId}";
            var totalAmountString = await _redisDb.HashGetAsync(keyCartInfo, "TotalAmount");
            decimal totalAmount = totalAmountString.HasValue ? decimal.Parse(totalAmountString!) : pagedItemsFromCache.Sum(i => i.Price * i.Quantity);

            return new CartDto
            {
                CartId = Guid.Parse(await _redisDb.HashGetAsync(keyCartInfo, "CartId")),
                UserId = userId,
                TotalAmount = totalAmount,
                Items = pagedItemsFromCache
            };
        }
        private async Task SetCacheAsync(CartDto cartDto)
        {
            var keyCartItems = $"cart:items:{cartDto.UserId}";
            var keyCartInfo = $"cart:info:{cartDto.UserId}";

            await _redisDb.KeyDeleteAsync(keyCartItems);
            await _redisDb.KeyDeleteAsync(keyCartInfo);

            var sortedSet = new List<SortedSetEntry>();
            foreach (var item in cartDto.Items)
            {
                var score = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                sortedSet.Add(new SortedSetEntry(JsonSerializer.Serialize(item), score));
            }
            if (sortedSet.Count > 0)
                await _redisDb.SortedSetAddAsync(keyCartItems, [.. sortedSet]);

            var hashEntries = new HashEntry[]
            {
                new("CartId", cartDto.CartId.ToString()),
                new("UserId", cartDto.UserId.ToString()),
                new("TotalAmount", cartDto.TotalAmount.ToString())
            };
            await _redisDb.HashSetAsync(keyCartInfo, hashEntries);
        }
    }
}
