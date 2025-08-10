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
    public class AddCategoryHandler(IProductService productService, ILogger<AddCategoryHandler> logger) : IRequestHandler<AddCategoryCommand, CommandDto>
    {
        private readonly IProductService _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        private readonly ILogger<AddCategoryHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public async Task<CommandDto> Handle(AddCategoryCommand request, CancellationToken cancellationToken)
        {
            if (request.CategoryDto.Name == null || request.CategoryDto.Description == null )
            {
                return new CommandDto
                {
                    IsSuccess = false,
                    Message = "Category name and description cannot be null."
                };
            }
            try
            {
                var categoryDto = new Category(request.CategoryDto.Name, request.CategoryDto.Description, true);
                var result = await _productService.AddCategory(categoryDto);
                if (result)
                {
                    return new CommandDto
                    {
                        IsSuccess = false,
                        Message = "Failed to add category."
                    };
                }
                return new CommandDto
                {
                    IsSuccess = true,
                    Message = "Category added successfully."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a category.");
                return new CommandDto
                {
                    IsSuccess = false,
                    Message = "An error occurred while adding the category."
                };
            }
        }
    }
}
