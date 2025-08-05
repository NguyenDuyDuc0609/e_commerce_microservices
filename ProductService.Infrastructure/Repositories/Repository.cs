using Microsoft.Extensions.Logging;
using ProductService.Application.Features.Dtos;
using ProductService.Application.Interfaces;
using ProductService.Domain.Entities;
using ProductService.Domain.Enums;
using ProductService.Infrastructure.Persistences;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Infrastructure.Repositories
{
    public class Repository(ProductContext context, ILogger<Repository> logger) : IRepository
    {
        private readonly ProductContext _context = context;
        private readonly ILogger<Repository> _logger = logger;
        public async Task<bool> AddProduct(Product product)
        {
            try
            {
                await _context.Products.AddAsync(product);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding product at {TimeUtc}", DateTime.UtcNow);
                return false;
            }
        }

        public Task<bool> AddSKU(Guid guid, string sku, string description, decimal price, int stock)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteProduct(Guid productId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteProduct<T>(Guid productId)
        {
            throw new NotImplementedException();
        }

        public Task<T> FilterProduct<T>(int pageNumber, int pageSize, ProductType? productType, int? price)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<QueryDto>> GetAllProducts(int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetProductById<T>(Guid productId)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetProductById<T>(int productId) where T : class
        {
            throw new NotImplementedException();
        }

        public Task<T> ProductReview<T>(int pageNumber, int pageSize, Guid productId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Rating(Guid guid, int rating)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Review(Guid guid, string review, string username)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateProduct(Guid productId, string name, decimal price)
        {
            throw new NotImplementedException();
        }
    }
}
