using CartService.Application.Features.Carts.Commands;
using CartService.Application.Features.Dtos;
using CartService.Application.Interfaces;
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
    public class ClearCartHandler(ICommandService commandService, IAuthHelper authHelper, ILogger<ClearCartHandler> logger) : IRequestHandler<ClearCartCommand, CartServiceResult>
    {
        private readonly ICommandService _commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
        private readonly IAuthHelper _authHelper = authHelper ?? throw new ArgumentNullException(nameof(authHelper));
        private readonly ILogger<ClearCartHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public async Task<CartServiceResult> Handle(ClearCartCommand request, CancellationToken cancellationToken)
        {
            if(request.Token == null)
            {
                throw new ArgumentNullException(nameof(request.Token), "Token cannot be null.");
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
                var result = await _commandService.ClearCart(userId);
                if (!result)
                {
                    return new CartServiceResult
                    {
                        IsSuccess = false,
                        Message = "Failed to clear the cart."
                    };
                }
                return new CartServiceResult
                {
                    IsSuccess = true,
                    Message = "Cart cleared successfully."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while clearing the cart.");
                return new CartServiceResult
                {
                    IsSuccess = false,
                    Message = "An error occurred while clearing the cart."
                };
            }
        }
    }
}
