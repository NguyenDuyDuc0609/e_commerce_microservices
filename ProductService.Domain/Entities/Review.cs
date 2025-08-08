using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Domain.Entities
{
    public class Review(Guid productId, string? userName, string? comment, int rating)
    {
        public Guid ReviewId { get; set; } = Guid.NewGuid();
        public Guid ProductId { get; set; } = productId;
        public string? UserName { get; set; } = userName;
        public string? Comment { get; private set; } = comment;
        public int Rating { get; private set; } = rating;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public virtual Product Product { get; set; } = null!;
        public void UpdateReview(string? userName, string? comment, int rating)
        {
            UserName = userName;
            Comment = comment;
            Rating = rating;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
