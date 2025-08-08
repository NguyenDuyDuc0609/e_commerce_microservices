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
    public class ProductReviewHandler(IProductService productService, ILogger<ProductReviewHandler> logger) : IRequestHandler<ReviewProductQuery, QueryDto>
    {
        private readonly IProductService _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        private readonly ILogger<ProductReviewHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public async Task<QueryDto> Handle(ReviewProductQuery request, CancellationToken cancellationToken)
        {
            if(request.ProductId == null)
            {
                return new QueryDto
                {
                    IsSuccess = false,
                    Message = "ProductId cannot be null or empty."
                };
            }
            try
            {
                var reviews = await _productService.GetReviews(request.ProductId, request.PageNumber, request.PageSize);
                if (reviews.Count != 0) {
                    return new QueryDto
                    {
                        IsSuccess = true,
                        Message = "Product reviews retrieved successfully.",
                        Data = reviews
                    };
                }
                return new QueryDto
                {
                    IsSuccess = false,
                    Message = "No reviews found for the specified product."
                };

            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to retrieve product reviews in {HandlerName} at {TimeUtc}",
                    nameof(ProductReviewHandler),
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
