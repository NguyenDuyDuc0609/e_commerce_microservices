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
        Task<bool> AddProduct(Product product);
        Task<bool> UpdateProduct(Guid productId, string name, decimal price);
        Task<bool> DeleteProduct(Guid productId);
        Task<T> GetProductById<T>(Guid productId);
        Task<IEnumerable<QueryDto>> GetAllProducts(int pageNumber, int pageSize);
        Task<bool> Rating(Guid guid, int rating);
        Task<bool> Review(Guid guid, string review, string username);
        Task<bool> AddSKU(Guid guid, string sku, string description, decimal price, int stock);
        Task<T> GetProductById<T>(int productId) where T : class;
        Task<T> ProductReview<T>(int pageNumber, int pageSize, Guid productId);
        Task<T> FilterProduct<T>(int pageNumber, int pageSize, ProductType? productType, int? price);
        Task<bool> DeleteProduct<T>(Guid productId);
    }
}
