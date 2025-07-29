using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Domain.Entities
{
    public class SKUAttribute
    {
        public Guid SKUAttributeId { get; set; }
        public Guid SKUId { get; set; }
        public string? AttributeName { get; set; }
        public string? AttributeValue { get; set; }
        public virtual SKU SKU { get; set; } = null!;
    }
}
