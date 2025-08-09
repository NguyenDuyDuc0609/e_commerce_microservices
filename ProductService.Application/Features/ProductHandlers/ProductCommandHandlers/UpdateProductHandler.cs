using MediatR;
using Microsoft.Extensions.Logging;
using ProductService.Application.Features.Dtos;
using ProductService.Application.Features.Products.ProductCommands;
using ProductService.Application.Interfaces;
using ProductService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Features.ProductHandlers.ProductCommandHandlers
{
    public class UpdateProductHandler(IProductService productService, ILogger<UpdateProductHandler> logger) : IRequestHandler<UpdateProductCommand, CommandDto>
    {
        private readonly IProductService _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        private readonly ILogger<UpdateProductHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public async Task<CommandDto> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            if(request.UpdateProductDto == null || string.IsNullOrEmpty(request.UpdateProductDto.Id) || !Guid.TryParse(request.UpdateProductDto.Id, out var productId))
            {
                return new CommandDto
                {
                    IsSuccess = false,
                    Message = "Invalid or missing ProductId or UpdateProductDto."
                };
            }
            try
            {
                var result = await _productService.UpdateProduct(
                    productId,
                    request.UpdateProductDto.Name,
                    request.UpdateProductDto.Price,
                    request.UpdateProductDto.Description,
                    request.UpdateProductDto.Slug,
                    request.UpdateProductDto.Brand,
                    request.UpdateProductDto.ImageUrl
                );
                if(result)
                {
                    return new CommandDto
                    {
                        IsSuccess = true,
                        Message = "Product updated successfully"
                    };
                }
                return new CommandDto
                {
                    IsSuccess = false,
                    Message = "Failed to update product"
                };
            }
            catch (Exception ex) {
                _logger.LogError(
                    ex,
                    "Failed to update product in {HandlerName} at {TimeUtc}",
                    nameof(UpdateProductHandler),
                    DateTime.UtcNow
                );
                return new CommandDto
                {
                    IsSuccess = false,
                    Message = "Failed to update product"
                };
            }
        }
    }
}
