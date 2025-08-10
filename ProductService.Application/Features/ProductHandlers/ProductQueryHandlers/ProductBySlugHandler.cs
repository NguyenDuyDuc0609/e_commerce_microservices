using MediatR;
using Microsoft.Extensions.Logging;
using ProductService.Application.Features.Dtos;
using ProductService.Application.Features.ProductHandlers.ProductCommandHandlers;
using ProductService.Application.Features.Products.ProductQueries;
using ProductService.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Features.ProductHandlers.ProductQueryHandlers
{
    public class ProductBySlugHandler(IProductService productService, ILogger<ProductBySlugHandler> logger) : IRequestHandler<ProductBySlugQuery, QueryDto>
    {
        private readonly IProductService _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        private readonly ILogger<ProductBySlugHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public async Task<QueryDto> Handle(ProductBySlugQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Slug))
            {
                return new QueryDto
                {
                    IsSuccess = false,
                    Message = "Slug cannot be null or empty",
                    Data = null
                };
            }
            try
            {

            }
            catch (Exception ex)
            {
                _logger.LogError(
                        ex,
                        "Request failure in {HandlerName} at {TimeUtc}",
                        nameof(AddProductHandler),
                        DateTime.UtcNow
                    );
                return new QueryDto
                {
                    IsSuccess = false,
                    Message = "An error occurred while processing your request.",
                    Data = null
                };
            }
        }
    }
}
