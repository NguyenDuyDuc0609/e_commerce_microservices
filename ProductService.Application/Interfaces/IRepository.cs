using ProductService.Application.Features.Dtos;
using ProductService.Domain.Entities;
using ProductService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Interfaces
{
    public interface IRepository
    {
        Task<bool> AddCategory(string name, string description);
        Task<bool> AddProduct(Product product);
        Task<bool> UpdateProduct(Guid productId, string? name, decimal? price, string? description, string? slug, string? brand, string? imgUrl);
        Task<Product> DeleteProduct(Guid productId);
        Task<T?> GetProductById<T>(Guid productId) where T : Product;
        Task<T?> GetProductByName<T>(string name) where T : Product;
        Task<List<Product>> GetProductByCategory(Guid categoryId, int pageNumber, int pageSize);
        Task<List<Product>> GetAllProducts(int pageNumber, int pageSize);
        Task<bool> AddReview(Guid productId, string? review, string username, int rating);
        Task<List<Review>> GetReviewPage(Guid productId, int pageNumber, int pageSize); 
        Task<List<Product>> FilterProduct(string? brand, decimal? price);
        Task<List<Product>> FilterProduct(List<Guid>? list);
        Task<bool> AddSKU(Guid productId, string? skuCode, decimal price, int stockQuantity, string? imageUrl, decimal? weight);
        Task<List<SKUDto>> GetSKUsByProductId(Guid productId);
    }
}
