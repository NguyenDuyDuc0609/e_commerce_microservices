using MediatR;
using Microsoft.Extensions.Logging;
using ProductService.Application.Features.Dtos;
using ProductService.Application.Features.ProductHandlers.ProductCommandHandlers;
using ProductService.Application.Features.Products.ProductQueries;
using ProductService.Application.Interfaces;
using ProductService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Features.ProductHandlers.ProductQueryHandlers
{
    public class GetProductByIdHandler(IProductService productService, ILogger<GetProductByIdHandler> logger) : IRequestHandler<GetProductByIdQuery, QueryDto>
    {
        private readonly IProductService _productService = productService  ?? throw new ArgumentNullException(nameof(productService));
        private readonly ILogger<GetProductByIdHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public async Task<QueryDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.ProductId)){
                return new QueryDto
                {
                    IsSuccess = false,
                    Message = "ProductId cannot be null or empty."
                };
            }
            try
            {
                if (!Guid.TryParse(request.ProductId, out var productId))
                {
                    return new QueryDto
                    {
                        IsSuccess = false,
                        Message = "Invalid ProductId format."
                    };
                }
                var product = await _productService.GetProductById<Product>(productId);
                if (product == null)
                {
                    return new QueryDto
                    {
                        IsSuccess = false,
                        Message = "Product not found."
                    };
                }
                return new QueryDto
                {
                    IsSuccess = true,
                    Message = "Product retrieved successfully.",
                    Data = product
                };
            }
            catch(Exception ex) {
                _logger.LogError(
                    ex,
                        "Failed to add product in {HandlerName} at {TimeUtc}",
                        nameof(AddProductHandler),
                        DateTime.UtcNow
                    );
                return new QueryDto
                {
                    IsSuccess = false,
                    Message = "An error occurred while processing your request."
                };
            }

        }
    }
}
