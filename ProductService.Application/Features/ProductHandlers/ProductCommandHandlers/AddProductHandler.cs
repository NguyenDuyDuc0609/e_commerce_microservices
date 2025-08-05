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
    public class AddProductHandler(IUnitOfWork unitOfWork, ILogger<AddProductHandler> logger) : IRequestHandler<AddProductCommand, CommandDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        private readonly ILogger<AddProductHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public async Task<CommandDto> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            if (request.AddProductDto == null)
            {
                return new CommandDto
                {
                    IsSuccess = false,
                    Message = "AddProductDto cannot be null"
                };
            }
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                Guid categoryId = Guid.Parse(request.AddProductDto.CategoryId);
                var product = new Product(
                    categoryId,
                    request.AddProductDto.Name,
                    request.AddProductDto.Description,
                    request.AddProductDto.Slug,
                    request.AddProductDto.Brand,
                    request.AddProductDto.ImageUrl,
                    request.AddProductDto.Price);
                var result = await _unitOfWork.Repository!.AddProduct(product);
                if(result)
                {
                    await _unitOfWork.CommitAsync();
                    return new CommandDto
                    {
                        IsSuccess = true,
                        Message = "Product added successfully"
                    };
                }
                else
                {
                    await _unitOfWork.Rollback();
                    _logger.LogError(
                        "Failed to add product in {HandlerName} at {TimeUtc}",
                        nameof(AddProductHandler),
                        DateTime.UtcNow
                    );
                    return new CommandDto
                    {
                        IsSuccess = false,
                        Message = "Failed to add product"
                    };
                }
            }
            catch (Exception ex)
            {
                await _unitOfWork.Rollback();
                _logger.LogError(
                    ex,
                    "Request failure in {HandlerName} at {TimeUtc}",
                    nameof(AddProductHandler),
                    DateTime.UtcNow
                );
                return new CommandDto
                {
                    IsSuccess = false,
                    Message = $"An error occurred while adding the product: {ex.Message}"
                };
            }
        }
    }
}
