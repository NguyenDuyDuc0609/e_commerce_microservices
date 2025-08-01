using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Domain.Entities
{
    [Owned]
    public class ProductRatingSummary
    {
        public int TotalReviews { get; set; }
        public double AverageRating { get; set; }
        public int FiveStarCount { get; set; }
        public int FourStarCount { get; set; }
        public int ThreeStarCount { get; set; }
        public int TwoStarCount { get; set; }
        public int OneStarCount { get; set; }
        public DateTime LastReviewedAt { get; set; }
        private ProductRatingSummary() { }
        public ProductRatingSummary(int totalReviews, double averageRating, int fiveStarCount, int fourStarCount, int threeStarCount, int twoStarCount, int oneStarCount, DateTime lastReviewedAt)
        {
            TotalReviews = totalReviews;
            AverageRating = averageRating;
            FiveStarCount = fiveStarCount;
            FourStarCount = fourStarCount;
            ThreeStarCount = threeStarCount;
            TwoStarCount = twoStarCount;
            OneStarCount = oneStarCount;
            LastReviewedAt = lastReviewedAt;
        }
    }
}
