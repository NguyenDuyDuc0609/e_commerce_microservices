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
        public async Task<string> AddProduct(Product product)
        {
            try
            {
                await _context.Products.AddAsync(product);

                var categorySlug = await _context.Categories
                    .Where(c => c.CategoryId == product.CategoryId)
                    .Select(c => c.Slug)
                    .FirstOrDefaultAsync();
                return categorySlug ?? throw new InvalidOperationException("Category not found for the product");
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to add product", ex);
            }
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

        public async Task<Product> DeleteProduct(Guid productId)
        {
            var product = await _context.Products.FindAsync(productId) ?? throw new InvalidOperationException("Product not found");
            product.Deactivate();
            _context.Products.Update(product);
            return product;
        }

        public async Task<bool> UpdateProduct(Guid productId, string? name, decimal? price, string? description, string? slug, string? brand, string? imgUrl)
        {
            var product = await _context.Products.FindAsync(productId) ?? throw new InvalidOperationException("Product not found");
            product.UpdateProduct(name, description, slug, brand, imgUrl, price ?? product.Price);
            _context.Products.Update(product);
            return true;
        }

        public Task<bool> AddSKU(Guid productId, string? skuCode, decimal price, int stockQuantity, string? imageUrl, decimal? weight)
        {
            throw new NotImplementedException();
        }

        public async Task<List<SKUDto>> GetSKUsByProductId(Guid productId)
        {
            var sku = await _context.SKUs
                .Where(s => s.ProductId == productId)
                .Select(s => new SKUDto
                {
                    SKUCode = s.SKUCode,
                    Price = s.Price,
                    StockQuantity = s.StockQuantity,
                    ImageUrl = s.ImageUrl,
                    Weight = s.Weight
                })
                .ToListAsync();
            return sku;
        }

        public async Task<bool> AddCategory(Category category)
        {
            if (category == null)
            {
                throw new ArgumentNullException(nameof(category), "Category cannot be null");
            }
            await _context.Categories.AddAsync(category);
            return true;
        }

        public async Task<List<ProductQueryDto>> ProductBySlug(string slug)
        {
            var categoryId = await _context.Categories
                .Where(c => c.Slug == slug)
                .Select(c => c.CategoryId)
                .FirstOrDefaultAsync();
            if (categoryId == Guid.Empty)
                throw new InvalidOperationException("Category not found for the given slug");
            var products = await _context.Products.Where(p => p.CategoryId == categoryId)
                .Select(p => new ProductQueryDto
                {
                    ProductId = p.ProductId,
                    Name = p.Name,
                    Price = p.Price,
                    Description = p.Description,
                    Slug = p.Slug,
                    Brand = p.Brand,
                    ImageUrl = p.ImageUrl,
                    SKUDtos = p.SKUs.Select(s => new SKUDto
                    {
                        SKUCode = s.SKUCode,
                        Price = s.Price,
                        StockQuantity = s.StockQuantity,
                        ImageUrl = s.ImageUrl,
                        Weight = s.Weight
                    }).ToList(),
                })
                .ToListAsync();
            if (products == null || products.Count == 0)
            {
                throw new InvalidOperationException("No products found for the given slug");
            }
            return products;
        }

        public async Task<List<ProductQueryDto>> ProductBySlug(List<Guid> productId)
        {
            try
            {
                var products = await _context.Products.Where(p => productId.Contains(p.ProductId))
                    .Select(p => new ProductQueryDto
                    {
                        ProductId = p.ProductId,
                        Name = p.Name,
                        Price = p.Price,
                        Description = p.Description,
                        Slug = p.Slug,
                        Brand = p.Brand,
                        ImageUrl = p.ImageUrl,
                        SKUDtos = p.SKUs.Select(s => new SKUDto
                        {
                            SKUCode = s.SKUCode,
                            Price = s.Price,
                            StockQuantity = s.StockQuantity,
                            ImageUrl = s.ImageUrl,
                            Weight = s.Weight
                        }).ToList(),
                    })
                    .ToListAsync();
                return products ?? [];
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve products by slug", ex);
            }
        }
    }
}
