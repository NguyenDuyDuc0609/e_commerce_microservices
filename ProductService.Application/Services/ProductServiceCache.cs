using Microsoft.Extensions.Caching.Distributed;
using ProductService.Application.Interfaces;
using ProductService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProductService.Application.Services
{
    public class ProductServiceCache(IProductService productService, IDistributedCache distributedCache) : IProductService
    {
        private readonly IProductService _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        private readonly IDistributedCache _distributedCache = distributedCache ?? throw new ArgumentNullException(nameof(distributedCache));
        public Task<List<Product>> GetAllProducts(int pageNumber, int pageSize)
        {
            try
            {
                var products = _productService.GetAllProducts(pageNumber, pageSize);
                return products;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get all products", ex);
            }
        }
        public async Task<T?> GetProductById<T>(Guid productId) where T : Product
        {
            string productIdString = productId.ToString();
            string? cachedData = await _distributedCache.GetStringAsync(productIdString);
            if (cachedData == "null") return default;
            if (!string.IsNullOrEmpty(cachedData))
            {
                try
                {
                    return JsonSerializer.Deserialize<T>(cachedData);
                }
                catch (JsonException)
                {
                    throw new InvalidOperationException("Cached data is not in the expected format.");
                }
            }

            var product = await _productService.GetProductById<Product>(productId);

            if (product != null)
            {
                await SetProductRedis(productIdString, product);
                return product as T;
            }
            await SetNullProductId(productIdString);
            return default;
        }
        public async Task<bool> AddProduct(Product product)
        {
            var result = await _productService.AddProduct(product);
            if (result)
            {
                return true;
            }
            return false;
        }
        public async Task<Product?> GetProductByName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Product name cannot be null or empty", nameof(name));
            }
            string cacheKey = $"ProductName:{name}";
            string? cachedData = await _distributedCache.GetStringAsync(cacheKey);
            if (!string.IsNullOrEmpty(cachedData))
            {
                try
                {
                    return JsonSerializer.Deserialize<Product>(cachedData);
                }
                catch (JsonException)
                {
                    throw new InvalidOperationException("Cached data is not in the expected format.");
                }
            }
            var product = await _productService.GetProductByName(name);
            if (product != null) {
                await SetProductRedis(cacheKey, product);
                return product;
            }
            await SetNullProductId(cacheKey);
            return null;
        }
        private async Task SetProductRedis(string productId, Product product, TimeSpan? expiration = null)
        {
            var cacheOption = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromMinutes(30)
            };
            string data = JsonSerializer.Serialize(product);
            await _distributedCache.SetStringAsync(productId.ToString(), data, cacheOption);
        }
        private async Task SetNullProductId(string productId, TimeSpan? expiration = null)
        {
            var cacheOption = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromMinutes(30)
            };
            await _distributedCache.SetStringAsync(productId, "null", cacheOption);
        }

        
    }
}
