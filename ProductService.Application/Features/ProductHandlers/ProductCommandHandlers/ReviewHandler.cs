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
    public class ReviewHandler(IProductService productService, ILogger<ReviewHandler> logger) : IRequestHandler<ReviewCommand, CommandDto>
    {
        private readonly IProductService _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        private readonly ILogger<ReviewHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public async Task<CommandDto> Handle(ReviewCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.ReviewDto.ProductId) || !Guid.TryParse(request.ReviewDto.ProductId, out var productId))
            {
                return new CommandDto
                {
                    IsSuccess = false,
                    Message = "Invalid or missing ProductId."
                };
            }
            try
            {
                var result = await _productService.AddReview(productId, request.ReviewDto.Comment,request.ReviewDto.UserName, request.ReviewDto.Rating);
                if(result)
                {
                    return new CommandDto
                    {
                        IsSuccess = true,
                        Message = "Review added successfully."
                    };
                }
                else
                {
                    _logger.LogError(
                        "Failed to add review in {HandlerName} at {TimeUtc}",
                        nameof(ReviewHandler),
                        DateTime.UtcNow
                    );
                    return new CommandDto
                    {
                        IsSuccess = false,
                        Message = "Failed to add review."
                    };
                }
            }
            catch (Exception ex) {
                _logger.LogError(
                    ex,
                    "Failed to handle review in {HandlerName} at {TimeUtc}",
                    nameof(ReviewHandler),
                    DateTime.UtcNow
                );
                return new CommandDto
                {
                    IsSuccess = false,
                    Message = "An error occurred while processing your request."
                };
            }
        }
    }
}
