using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductService.Application.Features.Dtos;

namespace ProductService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProductController(IMediator mediator, ILogger<ProductController> logger) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        private readonly ILogger<ProductController> _logger = logger;
        [HttpGet("test-product-api")]
        [AllowAnonymous]
        public IActionResult TestProductApi()
        {
            _logger.LogInformation("Product API is working!");
            return Ok("Product API is working!");
        }
        [HttpPost("add-product")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddProduct([FromBody] AddProductDto addProductDto)
        {
            var result = await _mediator.Send(addProductDto);
            return Ok(result);
        }
        [HttpPost("review-product")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> ReviewProduct([FromBody] ReviewDto reviewDto)
        {
            var result = await _mediator.Send(reviewDto);
            return Ok(result);
        }
        [HttpGet("get-all-products")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllProducts([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _mediator.Send(new GetAllProductsQuery(pageNumber, pageSize));
            return Ok(result);
        }
        [HttpGet("get-product-by-id/{productId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProductById(Guid productId)
        {
            if (productId == Guid.Empty)
            {
                return BadRequest("Invalid product ID.");
            }
            var result = await _mediator.Send(new GetProductByIdQuery(productId));
            if (result == null)
            {
                return NotFound("Product not found.");
            }
            return Ok(result);
        }
        [HttpGet("filter-products/{brand}/{price}/{pageNumber}/{pageSize}")]
        [AllowAnonymous]
        public async Task<IActionResult> FilterProducts(string? brand, int? price, int pageNumber = 1, int pageSize = 10)
        {
            var result = await _mediator.Send(new FilterProductsQuery(brand, price, pageNumber, pageSize));
            return Ok(result);
        }
        [HttpGet("product-reviews/{productId}/{pageNumber}/{pageSize}")]
        [AllowAnonymous]
        public async Task<IActionResult> ProductReviews(Guid productId, int pageNumber = 1, int pageSize = 10)
        {
            if (productId == Guid.Empty)
            {
                return BadRequest("Invalid product ID.");
            }
            var result = await _mediator.Send(new ProductReviewQuery(productId, pageNumber, pageSize));
            return Ok(result);
        }
        [HttpPost("get-product-by-name/{productName}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProductByName(string productName)
        {
            if (string.IsNullOrWhiteSpace(productName))
            {
                return BadRequest("Product name cannot be empty.");
            }
            var result = await _mediator.Send(productName);
            return Ok(result);
        }
        [HttpPut("update-product/{productId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProduct(Guid productId, [FromBody] UpdateProductDto updateProductDto)
        {
            if (productId == Guid.Empty || updateProductDto == null)
            {
                return BadRequest("Invalid product ID or update data.");
            }
            var result = await _mediator.Send(new UpdateProductCommand(productId, updateProductDto));
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpDelete("delete-product/{productId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProduct(Guid productId)
        {
            if (productId == Guid.Empty)
            {
                return BadRequest("Invalid product ID.");
            }
            var result = await _mediator.Send(new DeleteProductCommand(productId));
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
