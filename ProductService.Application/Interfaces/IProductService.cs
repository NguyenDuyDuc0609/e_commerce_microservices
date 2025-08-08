using ProductService.Domain.Entities;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Interfaces
{
    public interface IProductService
    {
        Task<List<Product>> GetAllProducts(int pageNumber, int pageSize);
        Task<T?> GetProductById<T>(Guid productId) where T : Product;
        Task<bool> AddProduct(Product product);
        Task<Product?> GetProductByName(string name);
        Task<List<Review>> GetReviews(string productId, int pageNumber, int pageSize);
        Task<bool> AddReview(Guid productId, string? review, string username, int rating);
        Task<List<Product>> ProductCateory(Guid categoryId, int pageNumber, int pageSize);
        Task<List<Product>> FilterProduct(string? brand, decimal? price, List<Guid>? list, int pageNumber, int pageSize);
    }
}
