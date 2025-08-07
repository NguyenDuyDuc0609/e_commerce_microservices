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
    public class FindProductByNameHandler(IProductService productService, ILogger<FindProductByNameHandler> logger) : IRequestHandler<FindProductByNameQuery, QueryDto>
    {
        private readonly IProductService _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        private readonly ILogger<FindProductByNameHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public async Task<QueryDto> Handle(FindProductByNameQuery request, CancellationToken cancellationToken)
        {
            if(request == null) return new QueryDto
            {
                IsSuccess = false,
                Message = "Request cannot be null."
            };
            try
            {
                var result = await _productService.GetProductByName(request.Name);
                if(result == null)
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
                    Message = "Product found successfully.",
                    Data = result
                };
            }
            catch (Exception ex) {
                _logger.LogError(
                    ex,
                    "Failed to find product by name in {HandlerName} at {TimeUtc}",
                    nameof(FindProductByNameHandler),
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
