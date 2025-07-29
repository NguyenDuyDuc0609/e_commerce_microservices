using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Domain.Entities
{
    public class ProductRatingSummary
    {
        public Guid ProductRatingSummaryId { get; set; }
        public Guid ProductId { get; set; }
        public int TotalReviews { get; set; }
        public double AverageRating { get; set; }
        public int FiveStarCount { get; set; }
        public int FourStarCount { get; set; }
        public int ThreeStarCount { get; set; }
        public int TwoStarCount { get; set; }
        public int OneStarCount { get; set; }
        public DateTime LastReviewedAt { get; set; }
        public virtual Product Product { get; set; } = null!;
    }
}
