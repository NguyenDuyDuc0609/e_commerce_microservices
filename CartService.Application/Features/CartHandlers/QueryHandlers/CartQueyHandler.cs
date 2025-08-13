using CartService.Application.Features.Carts.Queries;
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

namespace CartService.Application.Features.CartHandlers.QueryHandlers
{
    public class CartQueyHandler(IQueryService queryService, ILogger<CartQueyHandler> logger, IAuthHelper authHelper) : IRequestHandler<CartQuery, CartServiceResult>
    {
        private readonly IQueryService _queryService = queryService ?? throw new ArgumentNullException(nameof(queryService));
        private readonly ILogger<CartQueyHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly IAuthHelper _authHelper = authHelper ?? throw new ArgumentNullException(nameof(authHelper));
        public async Task<CartServiceResult> Handle(CartQuery request, CancellationToken cancellationToken)
        {
            if(request == null)
            {
                throw new ArgumentNullException(nameof(request), "Request cannot be null.");
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
                var result = await _queryService.GetCartPage(userId, request.PageNumber, request.PageSize);
                if(result == null)
                {
                    return new CartServiceResult
                    {
                        IsSuccess = false,
                        Message = "Cart not found or empty."
                    };
                }
                return new CartServiceResult
                {
                    IsSuccess = true,
                    Message = "Cart retrieved successfully.",
                    Data = result
                };

            }
            catch (Exception ex) {
                _logger.LogError(ex, "An error occurred while handling the CartQuery request.");
                return new CartServiceResult
                {
                    IsSuccess = false,
                    Message = "An error occurred while processing your request."
                };
            }
        }
    }
}
