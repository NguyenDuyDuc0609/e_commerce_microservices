using CartService.Application.Features.Dtos;
using CartService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartService.Application.Interfaces
{
    public interface IRepository
    {
        Task<CartDto> GetCartPage(Guid userId, int pageNumber, int pageSize);
        Task<bool> AddItemToCart(Guid userId, AddItemDto cartItem);
        Task<bool> DeleteItem(Guid userId, Guid itemId);
        Task<bool> ClearCart(Guid userId);
        Task<CartDto> GetCartPage(Guid userId);
    }
}
