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
        public string? Comment { get; private set; }
        public int Rating { get; private set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public virtual Product Product { get; set; } = null!;
        public Review(Guid productId, string? userName, string? comment, int rating)
        {
            ReviewId = Guid.NewGuid();
            ProductId = productId;
            UserName = userName;
            Comment = comment;
            Rating = rating;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }
        public void UpdateReview(string? userName, string? comment, int rating)
        {
            UserName = userName;
            Comment = comment;
            Rating = rating;
            UpdatedAt = DateTime.UtcNow;
        }

    }
}
