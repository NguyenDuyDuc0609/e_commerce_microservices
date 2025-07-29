using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Domain.Entities
{
    public class Review
    {
        public Guid ReviewId { get; set; }
        public Guid ProductId { get; set; }
        public string? UserName { get; set; }
        public string? Comment { get; set; }
        public int Rating { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public virtual Product Product { get; set; } = null!;
    }
}
