using MediatR;
using Microsoft.Extensions.Logging;
using ProductService.Application.Features.Dtos;
using ProductService.Application.Features.Products.ProductCommands;
using ProductService.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Features.ProductHandlers.ProductCommandHandlers
{
    public class DeleteProductHandler(IProductService productService, ILogger<DeleteProductHandler> logger) : IRequestHandler<DeleteProductCommand, CommandDto>
    {
        private readonly IProductService _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        private readonly ILogger<DeleteProductHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public async Task<CommandDto> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            if (request.Equals(null) || !Guid.TryParse(request.ProductId, out var productId)) return new CommandDto
            {
                IsSuccess = false,
                Message = "Invalid request or ProductId format."
            };
            try
            {
                var result = await _productService.DeleteProduct(productId);
                if (result != null)
                {
                    return new CommandDto
                    {
                        IsSuccess = true,
                        Message = "Product deleted successfully"
                    };
                }
                return new CommandDto
                {
                    IsSuccess = false,
                    Message = "Product not found or could not be deleted"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to delete product in {HandlerName} at {TimeUtc}",
                    nameof(DeleteProductHandler),
                    DateTime.UtcNow
                );
                return new CommandDto
                {
                    IsSuccess = false,
                    Message = "Failed to delete product"
                };
            }
        }
    }
}
