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
        Task<List<CartDto>> GetCartPage(Guid userId, int pageNumber, int pageSize);
        Task<bool> CreateCart(Cart cart);
        Task<bool> UpdateCart(Cart cart);
        Task<bool> DeleteCart(Guid cartId);
        Task<bool> ClearCart(Guid userId);
    }
}
