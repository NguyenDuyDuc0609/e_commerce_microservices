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
    public class GetSKUHandler(IProductService productService, ILogger<GetSKUHandler> logger) : IRequestHandler<GetSKUQuery, QueryDto>
    {
        private readonly IProductService _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        private readonly ILogger<GetSKUHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public async Task<QueryDto> Handle(GetSKUQuery request, CancellationToken cancellationToken)
        {
            if(string.IsNullOrEmpty(request.ProductId) || !Guid.TryParse(request.ProductId, out var productId))
            {
                _logger.LogError("Invalid ProductId provided: {ProductId}", request.ProductId);
                return new QueryDto
                {
                    IsSuccess = false,
                    Message = "Invalid Product ID",
                    Data = null
                };
            }
            try
            {
                var result = await _productService.GetSKUs(productId);
                if (result == null)
                {
                    return new QueryDto
                    {
                        IsSuccess = false,
                        Message = "SKU not found",
                        Data = null
                    };
                }
                return new QueryDto
                {
                    IsSuccess = true,
                    Message = "SKU retrieved successfully",
                    Data = result
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting SKU for product with ID {ProductId}", request.ProductId);
                return new QueryDto
                {
                    IsSuccess = false,
                    Message = "Failed to retrieve SKU",
                    Data = null
                };
            }
        }
    }
}
