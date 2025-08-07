using MediatR;
using Microsoft.Extensions.Logging;
using ProductService.Application.Features.Dtos;
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
    public class AdminGetProductHandler(IProductService productService, ILogger<AdminGetProductHandler> logger) : IRequestHandler<AdminGetAllProductQuery , QueryDto>
    {
        private readonly IProductService _productService =  productService ?? throw new ArgumentNullException(nameof(productService));
        private readonly ILogger<AdminGetProductHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public async Task<QueryDto> Handle(AdminGetAllProductQuery request, CancellationToken cancellationToken)
        {
            if(request.AdminGetProduct == null || request.AdminGetProduct.PageNumber == null || request.AdminGetProduct.PageSize == null)
            {
                return new QueryDto
                {
                    IsSuccess = false,
                    Message = "AdminGetProduct cannot be null and PageNumber and PageSize must be provided."
                };
            }
            try
            {
                var result = await _productService.GetAllProducts(
                    request.AdminGetProduct.PageNumber.Value,
                    request.AdminGetProduct.PageSize.Value
                );
                return new QueryDto
                {
                    IsSuccess = true,
                    Message = "Products retrieved successfully.",
                    Data = result
                };
            }
            catch(Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to retrieve products in {HandlerName} at {TimeUtc}",
                    nameof(AdminGetProductHandler),
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
