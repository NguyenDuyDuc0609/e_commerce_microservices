using MediatR;
using Microsoft.Extensions.Logging;
using ProductService.Application.Features.Dtos;
using ProductService.Application.Features.Products.ProductQueries;
using ProductService.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Features.ProductHandlers.ProductQueryHandlers
{
    public class ProductCategoryQueries(IProductService productService, ILogger<ProductCategoryQueries> logger) : IRequestHandler<ProductCategoryQuery, QueryDto>
    {
        private readonly IProductService _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        private readonly ILogger<ProductCategoryQueries> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public async Task<QueryDto> Handle(ProductCategoryQuery request, CancellationToken cancellationToken)
        {
            if(request.CategoryId == null || !Guid.TryParse(request.CategoryId, out var categoryId))
                return new QueryDto
                {
                    IsSuccess = false,
                    Message = "CategoryId cannot be null and must be a valid GUID."
                };
            try
            {
                var products = await _productService.ProductCateory(categoryId, request.PageNumber, request.PageSize);
                if(products.Count == 0)
                {
                    return new QueryDto
                    {
                        IsSuccess = false,
                        Message = "No products found for the specified category."
                    };
                }
                return new QueryDto
                {
                    IsSuccess = true,
                    Message = "Products retrieved successfully.",
                    Data = products
                };
            }
            catch (Exception ex) {
                _logger.LogError(
                    ex,
                    "Failed to retrieve product categories in {HandlerName} at {TimeUtc}",
                    nameof(ProductCategoryQueries),
                    DateTime.UtcNow
                );
                return new QueryDto
                {
                    IsSuccess = false,
                    Message = "An error occurred while processing your request.",
                };
            }
        }
    }
}
