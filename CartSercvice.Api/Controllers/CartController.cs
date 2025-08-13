using CartService.Application.Features.Carts.Commands;
using CartService.Application.Features.Carts.Queries;
using CartService.Application.Features.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CartSercvice.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CartController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost("Add-item")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> AddToCart([FromBody] AddItemDto addItemDto)
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(token) || !token.StartsWith("Bearer "))
            {
                return Unauthorized("Authorization token is missing or invalid.");
            }
            var cleanToken = token.Replace("Bearer ", "");
            var result = await _mediator.Send(new AddItemCommand(cleanToken, addItemDto));
            if (result == null || !result.IsSuccess)
            {
                return BadRequest("Failed to add item to cart. Please try again.");
            }
            return Ok(result);
        }

        [HttpPost("clear-cart")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> ClearCart()
        {
            var result = await _mediator.Send(new ClearCartCommand(HttpContext.Request.Headers["Authorization"].ToString()));
            return Ok("Cart cleared successfully");
        }
        [HttpDelete("delete-cart-item/{itemId}")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> RemoveFromCart(string itemId)
        {
           var token = HttpContext.Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(token) || !token.StartsWith("Bearer "))
            {
                return Unauthorized("Authorization token is missing or invalid.");
            }
            var cleanToken = token.Replace("Bearer ", "");
            var result = await _mediator.Send(new DeleteItemCommand(cleanToken, itemId));
            if (result == null)
            {
                return NotFound("Item not found in cart.");
            }
            return Ok("Item removed from cart successfully");
        }
        [HttpGet("cart-items/pageNumber={pageNumer}/{pageSize}")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetCartItems([FromQuery] int pageNumer, [FromQuery] int pageSize)
        {
            if (pageNumer < 1 || pageSize < 1)
            {
                return BadRequest("Page number and page size must be greater than zero.");
            }
            var token = HttpContext.Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(token) || !token.StartsWith("Bearer "))
            {
                return Unauthorized("Authorization token is missing or invalid.");
            }
            var cleanToken = token.Replace("Bearer ", "");

            var result = await _mediator.Send(new CartQuery(cleanToken, pageNumer, pageSize));
            if (result == null)
            {
                return NotFound("Cart not found or is empty.");
            }
            return Ok(result);
        }
    }
}
