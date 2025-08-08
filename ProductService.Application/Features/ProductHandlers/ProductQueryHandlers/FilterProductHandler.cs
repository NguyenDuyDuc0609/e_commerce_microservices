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
    public class FilterProductHandler(IProductService productService, ILogger<FilterProductHandler> logger) : IRequestHandler<FilterProductQuery, QueryDto>
    {
        private readonly IProductService _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        private readonly ILogger<FilterProductHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public async Task<QueryDto> Handle(FilterProductQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var products = await _productService.FilterProduct(request.Brand, request.Price, null, request.PageNumber, request.PageSize);
                if (products == null || products.Count == 0)
                {
                    return new QueryDto
                    {
                        IsSuccess = false,
                        Message = "No products found matching the criteria.",
                    };
                }
                return new QueryDto
                {
                    IsSuccess = true,
                    Message = "Products filtered successfully.",
                    Data = products
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to filter products in {HandlerName} at {TimeUtc}",
                    nameof(FilterProductHandler),
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
