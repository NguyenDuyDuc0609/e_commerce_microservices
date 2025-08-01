using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Domain.Entities
{
    public class ProductDiscount
    {
        public Guid ProductDiscountId { get; set; }
        public Guid ProductId { get; set; }
        public Guid DiscountId { get; set; }
        public virtual Product Product { get; set; } = null!;
        public virtual Discount Discount { get; set; } = null!;
    }
}
