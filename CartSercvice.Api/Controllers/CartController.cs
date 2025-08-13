using CartService.Application.Features.Carts.Commands;
using CartService.Application.Features.Carts.Queries;
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
        public IActionResult AddToCart([FromBody] string item)
        {
            return Ok($"Item {item} added to cart");
        }
        [HttpPut("update-item/{id}")]
        [Authorize(Roles = "Customer")]
        public IActionResult UpdateCartItem(int id, [FromBody] string item)
        {
            return Ok($"Item {id} updated to {item}");
        }
        [HttpPost("checkout")]
        [Authorize(Roles = "Customer")]
        public IActionResult Checkout([FromBody] string paymentInfo)
        {
            return Ok($"Checkout completed with payment info: {paymentInfo}");
        }
        [HttpPost("clear-cart")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> ClearCart()
        {
            var result = await _mediator.Send(new ClearCartCommand(HttpContext.Request.Headers["Authorization"].ToString()));
            return Ok("Cart cleared successfully");
        }
        [HttpDelete("delete-cart-item/{id}")]
        [Authorize(Roles = "Customer")]
        public async Task< IActionResult> RemoveFromCart(int id)
        {
            return Ok($"Item {id} removed from cart");
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
