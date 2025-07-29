using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Domain.Entities
{
    public class ProductAttribute
    {
        public Guid ProductAttributeId { get; set; }
        public Guid ProductId { get; set; }
        public string? AttributeName { get; set; }
        public string? AttributeValue { get; set; }
        public virtual Product Product { get; set; } = null!;
    }
}
