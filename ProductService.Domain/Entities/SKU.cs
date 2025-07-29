using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Domain.Entities
{
    public class SKU
    {
        public Guid SKUId { get; set; }
        public Guid ProductId { get; set; }
        public string? SKUCode { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string? ImageUrl { get; set; }
        public decimal? Weight { get; set; }
        public Product Product { get; set; } = null!;
        public ICollection<SKUAttribute> SKUAttributes { get; set; } = new List<SKUAttribute>();
    }
}
