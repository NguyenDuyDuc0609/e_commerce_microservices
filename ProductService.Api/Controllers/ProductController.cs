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
        public async Task<IActionResult> AddProduct([FromBody] AddProductDto addProductDto)
        {
            var result = await _mediator.Send(addProductDto);
            return Ok(result);
        }

    }
}
