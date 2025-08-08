using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProductService.Application.Features.Dtos;
using ProductService.Application.Interfaces;
using ProductService.Domain.Entities;
using ProductService.Domain.Enums;
using ProductService.Infrastructure.Persistences;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Infrastructure.Repositories
{
    public class Repository(ProductContext context) : IRepository
    {
        private readonly ProductContext _context = context;
        public async Task<bool> AddProduct(Product product)
        {
            try
            {
                await _context.Products.AddAsync(product);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to add product", ex);
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


        public async Task<List<Product>> GetAllProducts(int pageNumber, int pageSize)
        {
            try
            {
                return await _context.Products
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve products", ex);
            }
        }

        public async Task<T?> GetProductById<T>(Guid productId) where T : Product
        {
            var product = await _context.Products
                .Where(p => p.ProductId == productId).FirstOrDefaultAsync();
            if (product == null)
            {
                return null;
            }
            return product as T;
        }

        public async Task<T?> GetProductByName<T>(string name) where T : Product
        {
            var product = await _context.Products.Where(p => p.Name.Contains(name)).FirstOrDefaultAsync();
            if (product == null) return default;
            return product as T;
        }

        public Task<T> ProductReview<T>(int pageNumber, int pageSize, Guid productId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Rating(Guid guid, int rating)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> AddReview(Guid productId, string? review, string username, int rating)
        {
            try
            {
                var productEntity = await _context.Products.FindAsync(productId) ?? throw new InvalidOperationException("Product not found");
                var newReview = new Review(productId, username, review, rating);
                productEntity.UpdateRatingSummary(rating);
                await _context.Reviews.AddAsync(newReview);
                _context.Products.Update(productEntity);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to add review", ex);
            }
        }

        public Task<bool> UpdateProduct(Guid productId, string name, decimal price)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Review>> GetReviewPage(Guid productId, int pageNumber, int pageSize)
        {
            return await _context.Reviews
                .Where(r => r.ProductId == productId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Product>> GetProductByCategory(Guid categoryId, int pageNumber, int pageSize)
        {
            return await _context.Products
                .Where(p => p.CategoryId == categoryId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<List<Product>> FilterProduct(string? brand, decimal? price)
        {
            IQueryable<Product> query = _context.Products.AsQueryable();
            if (!string.IsNullOrEmpty(brand))
            {
                query = query.Where(p => p.Brand == brand).AsQueryable();
            }
            if (price.HasValue)
            {
                query = query.Where(p => p.Price <= price.Value).AsQueryable();
            }
            return await query.ToListAsync();
        }

        public async Task<List<Product>> FilterProduct(List<Guid>? list)
        {
            return await _context.Products
                .Where(p => list!.Contains(p.ProductId))
                .ToListAsync();
        }
    }
}
