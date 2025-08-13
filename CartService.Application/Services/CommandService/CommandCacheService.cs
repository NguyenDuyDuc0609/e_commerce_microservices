using CartService.Application.Features.Dtos;
using CartService.Application.Interfaces;
using CartService.Domain.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CartService.Application.Services.CommandService
{
    public class CommandCacheService(ICommandService commandService, IDistributedCache cache, IConnectionMultiplexer connectionMultiplexer) : ICommandService
    {
        private readonly ICommandService _commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
        private readonly IDistributedCache _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        private readonly IDatabase _redis = connectionMultiplexer.GetDatabase() ?? throw new ArgumentNullException(nameof(connectionMultiplexer));

        public async Task<bool> AddItemToCart(Guid userId, AddItemDto cartItem)
        {
            try
            {
                var result = await _commandService.AddItemToCart(userId, cartItem);
                if (!result)
                {
                    throw new Exception("Failed to add item to the cart in the database.");
                }

                var keyCartItems = $"cart:items:{userId}";
                var keyCartInfo = $"cart:info:{userId}";

                var tran = _redis.CreateTransaction();

                var existingItems = await tran.SortedSetRangeByRankWithScoresAsync(keyCartItems, 0, -1);
                var existingItemEntry = existingItems.FirstOrDefault(
                    e => JsonSerializer.Deserialize<CartItem>(e.Element!)!.ProductId == Guid.Parse(cartItem.ProductId!)
                );

                if (existingItemEntry.Element.HasValue)
                {
                    _ = tran.SortedSetRemoveAsync(keyCartItems, existingItemEntry.Element);

                    var existingItem = JsonSerializer.Deserialize<CartItem>(existingItemEntry.Element!);
                    var totalAmountTask = tran.HashGetAsync(keyCartInfo, "TotalAmount");
                    totalAmountTask.Wait();
                    decimal oldTotalAmount = totalAmountTask.Result.HasValue ? decimal.Parse(totalAmountTask.Result!) : 0;
                    decimal newTotalAmount = oldTotalAmount - (existingItem!.Price * existingItem.Quantity) + (cartItem.Price * cartItem.Quantity);
                    _ = tran.HashSetAsync(keyCartInfo, "TotalAmount", newTotalAmount.ToString());
                    await tran.SortedSetAddAsync(keyCartItems, JsonSerializer.Serialize(cartItem), DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
                }
                else
                {
                    var totalAmountTask = tran.HashGetAsync(keyCartInfo, "TotalAmount");
                    totalAmountTask.Wait();
                    decimal oldTotalAmount = totalAmountTask.Result.HasValue ? decimal.Parse(totalAmountTask.Result!) : 0;
                    decimal newTotalAmount = oldTotalAmount + (cartItem.Price * cartItem.Quantity);
                    _ = tran.HashSetAsync(keyCartInfo, "TotalAmount", newTotalAmount.ToString());
                    _ = tran.SortedSetAddAsync(keyCartItems, JsonSerializer.Serialize(cartItem), DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
                }
                var transactionResult = await tran.ExecuteAsync();

                if (!transactionResult)
                {
                    throw new Exception("Failed to perform Redis transaction for adding item.");
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding item to cart: {ex.Message}");
                throw new Exception("An error occurred while adding item to the cart.", ex);
            }
        }

        public async Task<bool> ClearCart(Guid userId)
        {
            try
            {
                var result = await _commandService.ClearCart(userId);
                if (result)
                {
                    var keyCartItems = $"cart:items:{userId}";
                    var keyCartInfo = $"cart:info:{userId}";
                    await _redis.KeyDeleteAsync(keyCartItems);
                    await _redis.KeyDeleteAsync(keyCartInfo);
                    await _cache.RemoveAsync(keyCartItems);
                    await _cache.RemoveAsync(keyCartInfo);
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while clearing the cart.", ex);
            }
        }

        public async Task<bool> DeleteItem(Guid userId, Guid itemId)
        {
            try
            {
                var result = await _commandService.DeleteItem(userId, itemId);
                if (!result)
                {
                    throw new Exception("Failed to delete item from the cart in the database.");
                }
                var keyCartItems = $"cart:items:{userId}";
                var keyCartInfo = $"cart:info:{userId}";
                await _redis.KeyDeleteAsync(keyCartItems);
                await _redis.KeyDeleteAsync(keyCartInfo);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while clearing the cart.", ex);
            }
        }
    }
}
