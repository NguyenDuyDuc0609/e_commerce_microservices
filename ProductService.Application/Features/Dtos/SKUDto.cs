using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Features.Dtos
{
    public class SKUDto
    {
        public string? ProductId { get; set; }
        public string? SKUCode { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string? ImageUrl { get; set; }
        public decimal? Weight { get; set; }
    }
}
