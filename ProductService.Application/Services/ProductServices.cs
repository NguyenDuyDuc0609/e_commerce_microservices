using ProductService.Application.Features.Dtos;
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

        public async Task<string> AddProduct(Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product), "Product cannot be null");
            }
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var result = await _unitOfWork.Repository!.AddProduct(product);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitAsync();
                return result;
            }
            catch(Exception ex)
            {
                await _unitOfWork.Rollback();
                throw new Exception("Failed to add product", ex);
            }
        }

        public async Task<bool> AddReview(Guid productId, string? review, string username, int rating)
        {

            if(review == null) return false;
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _unitOfWork.Repository!.AddReview(productId, review, username, rating);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await _unitOfWork.Rollback();
                throw new Exception("Failed to add review", ex);
            }
        }

      
        public async Task<Product> DeleteProduct(Guid productId)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                var product = await _unitOfWork.Repository!.DeleteProduct(productId) ?? throw new InvalidOperationException("Product not found");
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitAsync();
                return product;
            }
            catch (Exception ex)
            {
                await _unitOfWork.Rollback();
                throw new Exception("Failed to delete product", ex);
            }
        }

        public async Task<List<Product>> FilterProduct(string? brand, decimal? price, List<Guid>? list, int pageNumber , int pageSize)
        {
            try
            {
                if(list == null || list.Count == 0)
                {
                    return await _unitOfWork.Repository!.FilterProduct(brand, price);
                }
                return await _unitOfWork.Repository!.FilterProduct(list);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to filter products", ex);
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

        public async Task<List<Review>> GetReviews(string productId, int pageNumber, int pageSize)
        {
            try
            {
                if(string.IsNullOrEmpty(productId) || !Guid.TryParse(productId, out Guid productGuid))
                {
                    throw new ArgumentException("Invalid product ID", nameof(productId));
                }
                var reviews = await _unitOfWork.Repository!.GetReviewPage(productGuid, pageNumber, pageSize);
                return reviews ?? [];

            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve reviews", ex);
            }
        }

        public async Task<List<Product>> ProductCateory(Guid categoryId, int pageNumber, int pageSize)
        {
            try
            {
                var products = await _unitOfWork.Repository!.GetProductByCategory(categoryId, pageNumber, pageSize);
                return products ?? [];
            }
            catch(Exception ex)
            {
                throw new Exception("Failed to retrieve products by category", ex);
            }
        }

        public async Task<bool> UpdateProduct(Guid productId, string? name, decimal? price, string? description, string? slug, string? brand, string? imgUrl)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                var result = await _unitOfWork.Repository!.UpdateProduct(productId, name, price, description, slug, brand, imgUrl);
                if (!result)
                {
                    throw new InvalidOperationException("Failed to update product");
                }
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to update product", ex);
            }
        }
        public async Task<bool> AddSKU(Guid productId, string? skuCode, decimal price, int stockQuantity, string? imageUrl, decimal? weight)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                var result = await _unitOfWork.Repository!.AddSKU(productId, skuCode, price, stockQuantity, imageUrl, weight);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitAsync();
                return result;
            }
            catch (Exception ex)
            {
                await _unitOfWork.Rollback();
                throw new Exception("Failed to add SKU", ex);
            }
        }

        public async Task<List<SKUDto>> GetSKUs(Guid ProductId)
        {
            try
            {
                var skus = await _unitOfWork.Repository!.GetSKUsByProductId(ProductId);
                return skus ?? [];
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve SKUs", ex);
            }
        }

        public async Task<bool> AddCategory(Category category)
        {
            try
            {
                if (category == null)
                {
                    throw new ArgumentNullException(nameof(category), "Category cannot be null");
                }
                return await _unitOfWork.Repository!.AddCategory(category);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to add category", ex);
            }
        }

        public async Task<List<ProductQueryDto>> ProductBySlug(string? slug, List<Guid>? productIds, int pageNumber, int pageSize)
        {
            try
            {
                if (slug != null)
                    return await _unitOfWork.Repository!.ProductBySlug(slug);
                return await _unitOfWork.Repository!.ProductBySlug(productIds!);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve products by slug", ex);
            }
        }
    }
}
