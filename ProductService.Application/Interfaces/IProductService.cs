using ProductService.Domain.Entities;
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
    }
}
