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

        public async Task<bool> AddItemToCart(Guid userId, AddItemDto itemDto)
        {
            if(itemDto.ProductId == null || !Guid.TryParse(itemDto.ProductId, out var productId))
            {
                throw new ArgumentException("Invalid ProductId", nameof(itemDto.ProductId));
            }

            try
            {
                var cart = await _context.Carts
                    .Include(c => c.CartItems)
                    .FirstOrDefaultAsync(c => c.UserId == userId);
                if (cart == null)
                {
                    cart = new Cart
                    {
                        UserId = userId,
                    };
                    _context.Carts.Add(cart);
                }


                cart.AddItem(productId, itemDto.ProductName!, itemDto.Price, itemDto.Quantity);

                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to add item to cart", ex);
            }
        }

        public async Task<bool> ClearCart(Guid userId)
        {
            try
            {
                var cartItems = await _context.CartItems
                    .Where(ci => ci.Cart.UserId == userId)
                    .ToListAsync();

                if (cartItems.Count == 0) return false;

                _context.CartItems.RemoveRange(cartItems);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to clear cart", ex);
            }
        }

        private async Task<bool> CreateCart(Cart cart)
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

        public async Task<bool> DeleteItem(Guid userId, Guid itemId)
        {
            try
            {
                var cart = await _context.Carts
                    .Include(c => c.CartItems)
                    .FirstOrDefaultAsync(c => c.UserId == userId);
                if (cart == null)
                {
                    throw new InvalidOperationException("Cart not found.");
                }
                cart.RemoveItem(itemId);
                _context.Carts.Update(cart);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to delete item from cart", ex);
            }
        }


        public async Task<CartDto> GetCartPage(Guid userId, int pageNumber, int pageSize)
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
                    }).FirstOrDefaultAsync();
            return carts ?? throw new InvalidOperationException("Cart not found for the user.");
        }

        public async Task<CartDto> GetCartPage(Guid userId)
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
                            .ToList()
                    }).FirstOrDefaultAsync();
            return carts ?? throw new InvalidOperationException("Cart not found for the user.");
        }
    }
}
