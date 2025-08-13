using CartService.Application.Features.Carts.Commands;
using CartService.Application.Features.Dtos;
using CartService.Application.Interfaces;
using CartService.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CartService.Application.Features.CartHandlers.CommandHandlers
{
    public class AddItemHandler(ICommandService commandService, IAuthHelper authHelper, ILogger<AddItemHandler> logger) : IRequestHandler<AddItemCommand, CartServiceResult>
    {
        private readonly ICommandService _commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
        private readonly ILogger<AddItemHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly IAuthHelper _authHelper = authHelper ?? throw new ArgumentNullException(nameof(authHelper));

        public  async Task<CartServiceResult> Handle(AddItemCommand request, CancellationToken cancellationToken)
        {
            if(request.Token == null || request.AddItemDto == null)
            {
                return new CartServiceResult
                {
                    IsSuccess = false,
                    Message = "Invalid request data."
                };
            }
            try
            {
                var principal = _authHelper.GetClaimsPrincipalToken(request.Token);
                if (principal == null)
                {
                    return new CartServiceResult
                    {
                        IsSuccess = false,
                        Message = "Invalid token."
                    };
                }
                var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
                {
                    return new CartServiceResult
                    {
                        IsSuccess = false,
                        Message = "Invalid user ID in token."
                    };
                }
                var result = await _commandService.AddItemToCart(userId, request.AddItemDto);
                if (!result)
                {
                    return new CartServiceResult
                    {
                        IsSuccess = false,
                        Message = "Failed to add item to the cart."
                    };
                }
                return new CartServiceResult
                {
                    IsSuccess = true,
                    Message = "Item added to the cart successfully."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding an item to the cart.");
                return new CartServiceResult
                {
                    IsSuccess = false,
                    Message = "An error occurred while adding an item to the cart."
                };
            }
        }
    }
}
