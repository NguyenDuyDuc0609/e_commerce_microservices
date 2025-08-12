using CartService.Application.Features.Dtos;
using CartService.Application.Interfaces;
using CartService.Domain.Entities;
using CartService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartService.Infrastructure.Repositories
{
    public class Repository(CartContext context) : IRepository
    {
        private readonly CartContext _context = context ?? throw new ArgumentNullException(nameof(context));

        public Task<bool> ClearCart(Guid userId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> CreateCart(Cart cart)
        {
            if (cart == null)
            {
                throw new ArgumentNullException(nameof(cart), "Cart cannot be null");
            }
            try
            {
                var cartExit = _context.Carts.Any(c => c.UserId == cart.UserId);
                if (cartExit)
                {
                    throw new InvalidOperationException("Cart already exists for this user.");
                }
                _context.Carts.Add(cart);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to create cart", ex);
            }
        }

        public Task<bool> DeleteCart(Guid cartId)
        {
            throw new NotImplementedException();
        }


        public async Task<List<CartDto>> GetCartPage(Guid userId, int pageNumber, int pageSize)
        {
            var carts = await _context.Carts
                    .Where(c => c.UserId == userId)
                    .Select(c => new CartDto
                    {
                        CartId = c.CartId,
                        UserId = c.UserId,
                        TotalAmount = c.TotalAmount,
                        Items = c.CartItems
                            .Select(i => new ItemDto
                            {
                                CartItemId = i.CartItemId,
                                CartId = i.CartId,
                                ProductId = i.ProductId,
                                ProductName = i.ProductName,
                                Quantity = i.Quantity,
                                Price = i.Price,
                                StatusItem = i.StatusItem
                            })
                            .Skip((pageNumber - 1) * pageSize)
                            .Take(pageSize)
                            .ToList()
                    })
                    .ToListAsync();
            if(carts == null || carts.Count == 0)
            {
                throw new InvalidOperationException("No carts found for the specified user.");
            }
            return carts;
        }

        public Task<bool> UpdateCart(Cart cart)
        {
            throw new NotImplementedException();
        }
    }
}
