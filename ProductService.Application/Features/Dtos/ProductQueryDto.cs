using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Features.Dtos
{
    public class ProductQueryDto
    {
        public Guid ProductId { get; set; }
        public  string? Name { get; set; }
        public  string? Description { get; set; }
        public  string? Slug { get; set; }
        public  string? Brand { get; set; }
        public  string? ImageUrl { get; set; }
        public  decimal Price { get; set; }
        public List<SKUDto> SKUDtos { get; set; } = [];
    }
}
