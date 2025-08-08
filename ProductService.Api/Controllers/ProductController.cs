using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductService.Application.Features.Dtos;
using ProductService.Application.Features.Products.ProductQueries;

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
        [HttpGet("get-all-products/{pageNumber}/{pageSize}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> GetAllProducts(int pageNumber = 1, int pageSize = 10)
        {
            var pagination = new AdminGetProduct
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            var result = await _mediator.Send(pagination);
            return Ok(result);
        }
        [HttpGet("get-product-by-id/{productId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProductById(string productId)
        {
            var result = await _mediator.Send(new GetProductByIdQuery(productId));
            return Ok(result);
        }
        [HttpGet("filter-products/{brand}/{price}/{pageNumber}/{pageSize}")]
        [AllowAnonymous]
        public async Task<IActionResult> FilterProducts(string? brand, decimal? price, int pageNumber , int pageSize)
        {
            var result = await _mediator.Send(new FilterProductQuery(brand, price, pageNumber, pageSize));
            return Ok(result);
        }
        [HttpGet("product-reviews/{productId}/{pageNumber}/{pageSize}")]
        [AllowAnonymous]
        public async Task<IActionResult> ProductReviews(string productId, int pageNumber = 1, int pageSize = 5)
        {
            var result = await _mediator.Send(new ReviewProductQuery(productId, pageNumber, pageSize));
            return Ok(result);
        }
        [HttpPost("search-product/{productName}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProductByName(string productName)
        {
            if (string.IsNullOrWhiteSpace(productName))
            {
                return BadRequest("Product name cannot be empty.");
            }
            var result = await _mediator.Send(new FindProductByNameQuery(productName));
            return Ok(result);
        }
        //[HttpPut("update-product/{productId}")]
        //[Authorize(Roles = "Admin")]
        //public async Task<IActionResult> UpdateProduct(Guid productId, [FromBody] UpdateProductDto updateProductDto)
        //{
        //    if (productId == Guid.Empty || updateProductDto == null)
        //    {
        //        return BadRequest("Invalid product ID or update data.");
        //    }
        //    var result = await _mediator.Send(new UpdateProductCommand(productId, updateProductDto));
        //    if (result.IsSuccess)
        //    {
        //        return Ok(result);
        //    }
        //    return BadRequest(result);
        //}
        //[HttpDelete("delete-product/{productId}")]
        //[Authorize(Roles = "Admin")]
        //public async Task<IActionResult> DeleteProduct(Guid productId)
        //{
        //    if (productId == Guid.Empty)
        //    {
        //        return BadRequest("Invalid product ID.");
        //    }
        //    var result = await _mediator.Send(new DeleteProductCommand(productId));
        //    if (result.IsSuccess)
        //    {
        //        return Ok(result);
        //    }
        //    return BadRequest(result);
        //}
        [HttpGet("products-by-category/{categoryId}/{pageNumber}/{pageSize}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProductsByCategory(string categoryId, int pageNumber = 1, int pageSize = 10)
        {
            var result = await _mediator.Send(new ProductCategoryQuery(categoryId, pageNumber, pageSize));
            return Ok(result);
        }
        //[HttpGet("top-rated-products/{pageNumber}/{pageSize}")]
        //[AllowAnonymous]
        //public async Task<IActionResult> GetTopRatedProducts(int pageNumber = 1, int pageSize = 10)
        //{
        //    var result = await _mediator.Send(new GetTopRatedProductsQuery(pageNumber, pageSize));
        //    return Ok(result);
        //}
    }
}
