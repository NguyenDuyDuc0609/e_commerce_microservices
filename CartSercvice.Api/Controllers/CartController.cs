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
            
            return Ok("Cart cleared successfully");
        }
        [HttpDelete("delete-cart-item/{id}")]
        [Authorize(Roles = "Customer")]
        public IActionResult RemoveFromCart(int id)
        {
            return Ok($"Item {id} removed from cart");
        }
        [HttpGet("cart-items/pageNumber={pageNumer}/{pageSize}")]
        [Authorize(Roles = "Customer")]
        public IActionResult GetCartItems(int pageNumer, int pageSize)
        {
            return Ok("Retrieved cart items successfully");
        }
    }
}
