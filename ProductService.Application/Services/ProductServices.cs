using ProductService.Application.Interfaces;
using ProductService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Services
{
    public class ProductServices(IUnitOfWork unitOfWork) : IProductService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<bool> AddProduct(Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product), "Product cannot be null");
            }
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var result = await _unitOfWork.Repository!.AddProduct(product);
                if (!result)
                    throw new InvalidOperationException("Failed to add product");

                await _unitOfWork.CommitAsync();
                return true;
            }
            catch(Exception ex)
            {
                await _unitOfWork.Rollback();
                throw new Exception("Failed to add product", ex);
            }
        }

        public async Task<List<Product>> GetAllProducts(int pageNumber, int pageSize)
        {
            return await _unitOfWork.Repository!.GetAllProducts(pageNumber, pageSize);
        }

        public async Task<T?> GetProductById<T>(Guid productId) where T : Product
        {
            return await _unitOfWork.Repository!.GetProductById<T>(productId);
        }

        public async Task<Product?> GetProductByName(string name)
        {
            if(string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Product name cannot be null or empty", nameof(name));
            }
            var product = await _unitOfWork.Repository!.GetProductByName<Product>(name);
            if (product == null)
                return null;
            return product;
        }
    }
}
