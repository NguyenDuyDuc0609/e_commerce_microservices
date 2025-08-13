using CartService.Application.Features.Dtos;
using CartService.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartService.Application.Services.QueryService
{
    public sealed class QueryService(IUnitOfWork unitOfWork) : IQueryService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));


        public async Task<CartDto> GetCartPage(Guid userId, int pageNumber, int pageSize)
        {
            if (userId == Guid.Empty)
            {
                throw new ArgumentException("User ID cannot be empty.", nameof(userId));
            }
            if (pageNumber < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(pageNumber), "Page number must be greater than or equal to 1.");
            }

            try
            {
                var cart = await _unitOfWork.Repository!.GetCartPage(userId);
                return cart;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while retrieving the cart page.", ex);
            }
        }
    }
}
