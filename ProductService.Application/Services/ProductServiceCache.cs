using Microsoft.Extensions.Caching.Distributed;
using ProductService.Application.Interfaces;
using ProductService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace ProductService.Application.Services
{
    public class ProductServiceCache(IProductService productService, IDistributedCache distributedCache, IConnectionMultiplexer connectionMultiplexer) : IProductService
    {
        private readonly IProductService _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        private readonly IDistributedCache _distributedCache = distributedCache ?? throw new ArgumentNullException(nameof(distributedCache));
        private readonly IDatabase _redisDb = connectionMultiplexer.GetDatabase() ?? throw new ArgumentNullException(nameof(connectionMultiplexer));
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
            if (product != null)
            {
                await SetProductRedis(cacheKey, product);
                return product;
            }
            await SetNullProductId(cacheKey);
            return null;
        }

        public async Task<List<Review>> GetReviews(string productId, int pageNumber, int pageSize)
        {
            long start = (pageNumber - 1) * pageSize;
            long end = start + pageSize - 1;
            string key = $"product:{productId}:reviews";
            try
            {
                var reviews = await _redisDb.SortedSetRangeByRankAsync(key, start, end, Order.Descending);
                if (reviews.Length != 0)
                {
                    var reviewList = reviews.Select(r => JsonSerializer.Deserialize<Review>(r!)).Where(r => r != null).ToList();
                    return reviewList!;
                }
                var productReviews = await _productService.GetReviews(productId, pageNumber, pageSize);
                if (productReviews == null || productReviews.Count == 0) return [];
                await _redisDb.SortedSetAddAsync(key, productReviews.Select(r => new SortedSetEntry(JsonSerializer.Serialize(r), new DateTimeOffset(r.CreatedAt).ToUnixTimeSeconds())).ToArray());
                return productReviews;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get product reviews", ex);
            }
        }



        public async Task<bool> AddReview(Guid productId, string? review, string username, int rating)
        {
            try
            {
                if (string.IsNullOrEmpty(review) || string.IsNullOrEmpty(username))
                {
                    throw new ArgumentException("Review and username cannot be null or empty");
                }
                var result = await _productService.AddReview(productId, review, username, rating);
                if (!result) return false;

                var key = $"product:{productId}:reviews";
                var json = JsonSerializer.Serialize(new { review, username, rating });
                var score = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
                await _redisDb.SortedSetAddAsync(key, json, score);
                await _redisDb.SortedSetRemoveRangeByRankAsync(key, 0, -16);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to add review", ex);
            }
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

        public Task<List<Product>> ProductCateory(Guid categoryId, int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Product>> FilterProduct(string? brand, decimal? price, List<Guid>? list, int pageNumber, int pageSize)
        {
            if(brand == "all") brand = null;
            if(price == 0) price = null;
            try
            {
                if (brand != null && !price.HasValue)
                {
                    var brandKey = $"product:brand:{brand.ToLower()}";
                    if(!await _redisDb.KeyExistsAsync(brandKey))
                    {
                        await SetBrandKey(brand, pageNumber, pageSize);
                    }
                    RedisValue[] brandValues = await _redisDb.SetMembersAsync(brandKey);
                    if (brandValues.Length == 0)
                    {
                        return [];
                    }
                    var productIds = brandValues.Select(id => Guid.Parse(id.ToString())).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                    return await _productService.FilterProduct(brand, null, productIds, pageNumber, pageSize);

                }
                if (price.HasValue && brand == null)
                {
                    var priceKey = $"product:price:{price.Value}";
                    if (!await _redisDb.KeyExistsAsync(priceKey))
                    {
                        await SetPriceKey(price.Value, pageNumber, pageSize);
                    }
                    RedisValue[] priceValues = await _redisDb.SetMembersAsync(priceKey);
                    if (priceValues.Length == 0)
                    {
                        return [];
                    }
                    var productIds = priceValues.Select(id => Guid.Parse(id.ToString())).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                    return await _productService.FilterProduct(null, price, productIds, pageNumber, pageSize);
                }
                var guids = await FilterProduct(brand!, price!.Value, pageNumber, pageSize);
                if (guids == null || guids.Count == 0)
                {
                    return [];
                }
                return await _productService.FilterProduct(brand, price, guids, pageNumber, pageSize);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to filter products", ex);
            }
        }
        private async Task SetBrandKey(string brand, int pageNumber, int pageSize)
        {
            try
            {
                var brandKey = $"product:brand:{brand.ToLower()}";
                var products = await _productService.FilterProduct(brand, null, null, pageNumber, pageSize);
                if (products != null && products.Count > 0)
                {
                    RedisValue[] redisValues = products.Select(id => (RedisValue)id.ToString()).ToArray();
                    await _redisDb.SetAddAsync(brandKey, redisValues);
                    await _redisDb.KeyExpireAsync(brandKey, TimeSpan.FromMinutes(30));
                }
                else
                {
                    await _redisDb.SetAddAsync(brandKey, []);
                    await _redisDb.KeyExpireAsync(brandKey, TimeSpan.FromMinutes(30));
                }
                return;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to filter products by brand", ex);
            }
        }
        private async Task SetPriceKey(decimal price, int pageNumber, int pageSize)
        {
            try
            {
                var priceKey = $"product:price:{price}";
                var products = await _productService.FilterProduct(null, price, null, pageNumber, pageSize);
                if (products != null && products.Count > 0)
                {
                    RedisValue[] redisValues = products.Select(id => (RedisValue)id.ToString()).ToArray();
                    await _redisDb.SetAddAsync(priceKey, redisValues);
                    await _redisDb.KeyExpireAsync(priceKey, TimeSpan.FromMinutes(30));
                }
                else
                {
                    await _redisDb.SetAddAsync(priceKey, []);
                    await _redisDb.KeyExpireAsync(priceKey, TimeSpan.FromMinutes(30));
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to filter products by price", ex);
            }
        }
        private async Task<List<Guid>> FilterProduct(string brand, decimal price, int pageNumber, int pageSize)
        {
            try
            {
                var brandKey = $"product:brand:{brand.ToLower()}";
                var priceKey = $"product:price:{price}";
                if (!await _redisDb.KeyExistsAsync(brandKey))
                    await SetBrandKey(brand, pageNumber, pageSize);
                if (!await _redisDb.KeyExistsAsync(priceKey))
                    await SetPriceKey(price, pageNumber, pageSize);
                var finalKey = $"temp:intersection";
                await _redisDb.SortedSetCombineAndStoreAsync(
                    SetOperation.Intersect,
                    finalKey,
                    brandKey,
                    priceKey
                );
                var start = (pageNumber - 1) * pageSize;
                var end = start + pageSize - 1;
                var setIds= await _redisDb.SortedSetRangeByRankAsync(finalKey, start, end);
                await _redisDb.KeyDeleteAsync(finalKey);
                return  setIds.Select(id => Guid.Parse(id.ToString())).ToList() ?? [];

            }
            catch(Exception ex)
            {
                throw new Exception("Failed to filter products", ex);
            }
        }

        public async Task<Product> DeleteProduct(Guid productId)
        {
            try
            {
                var product = await _productService.DeleteProduct(productId) ?? throw new InvalidOperationException("Product not found");
                var productIdString = productId.ToString();
                var brandKey = $"product:brand:{product.Brand.ToLower()}";
                var priceKey = $"product:price:{product.Price}";
                await _redisDb.SetRemoveAsync(brandKey, productIdString);
                await _redisDb.SetRemoveAsync(priceKey, productIdString);
                return product;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to delete product", ex);
            }
        }

        public async Task<bool> UpdateProduct(Guid productId, string? name, decimal? price, string? description, string? slug, string? brand, string? imgUrl)
        {
            if (productId == Guid.Empty)
            {
                throw new ArgumentException("Product ID cannot be empty", nameof(productId));
            }
            if(price <= 0)
            {
                throw new ArgumentException("Product price must be greater than zero", nameof(price));
            }
            try
            {
                return await _productService.UpdateProduct(productId, name, price, description, slug, brand, imgUrl);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to update product", ex);
            }
        }
    }
}
