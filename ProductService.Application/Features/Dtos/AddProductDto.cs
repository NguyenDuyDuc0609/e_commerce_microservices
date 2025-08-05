using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Features.Dtos
{
    public class AddProductDto
    {
        public required string CategoryId { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required string Slug { get; set; }
        public required string Brand { get; set; }
        public required string ImageUrl { get; set; }
        public required decimal Price { get; set; }
    }
}
