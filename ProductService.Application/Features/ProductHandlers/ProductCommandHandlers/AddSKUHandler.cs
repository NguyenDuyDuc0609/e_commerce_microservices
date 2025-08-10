using MediatR;
using Microsoft.Extensions.Logging;
using ProductService.Application.Features.Dtos;
using ProductService.Application.Features.Products.ProductCommands;
using ProductService.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Features.ProductHandlers.ProductCommandHandlers
{
    public class AddSKUHandler(IProductService productService, ILogger<AddSKUHandler> logger) : IRequestHandler<AddSKUCommand, CommandDto>
    {
        private readonly IProductService _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        private readonly ILogger<AddSKUHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public async Task<CommandDto> Handle(AddSKUCommand request, CancellationToken cancellationToken)
        {
            if(request.SKUDto.ProductId is null || string.IsNullOrEmpty(request.SKUDto.ProductId) || !Guid.TryParse(request.SKUDto.ProductId, out var productId))
            {
                return new CommandDto
                {
                    IsSuccess = false,
                    Message = "Invalid SKU data provided."
                };
            }
            try
            {
                var result = await _productService.AddSKU(productId,
                    request.SKUDto.SKUCode,
                    request.SKUDto.Price,
                    request.SKUDto.StockQuantity,
                    request.SKUDto.ImageUrl,
                    request.SKUDto.Weight);
                if (!result)
                {
                    return new CommandDto
                    {
                        IsSuccess = false,
                        Message = "Failed to add SKU. Please check the provided data."
                    };
                }
                return new CommandDto
                {
                    IsSuccess = true,
                    Message = "SKU added successfully."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(
                                    ex,
                                    "Failed to add SKU in {HandlerName} at {TimeUtc}",
                                    nameof(AddSKUHandler),
                                    DateTime.UtcNow
                                );
                return new CommandDto
                {
                    IsSuccess = false,
                    Message = "An error occurred while adding the SKU."
                };
            }
        }
    }
}
