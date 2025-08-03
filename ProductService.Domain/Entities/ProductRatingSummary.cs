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
        public void UpdateSummary(int totalReviews, double averageRating, int fiveStarCount, int fourStarCount, int threeStarCount, int twoStarCount, int oneStarCount)
        {
            TotalReviews = totalReviews;
            AverageRating = averageRating;
            FiveStarCount = fiveStarCount;
            FourStarCount = fourStarCount;
            ThreeStarCount = threeStarCount;
            TwoStarCount = twoStarCount;
            OneStarCount = oneStarCount;
            LastReviewedAt = DateTime.UtcNow;
        }
        private void IncreaseRating(int rating) {
            switch (rating)
            {
                case 5:
                    FiveStarCount++;
                    break;
                case 4:
                    FourStarCount++;
                    break;
                case 3:
                    ThreeStarCount++;
                    break;
                case 2:
                    TwoStarCount++;
                    break;
                case 1:
                    OneStarCount++;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(rating), "Rating must be between 1 and 5.");
            }
            TotalReviews++;
        }
        private void DecreaseRating(int rating)
        {
            switch (rating)
            {
                case 5:
                    if (FiveStarCount > 0) FiveStarCount--;
                    break;
                case 4:
                    if (FourStarCount > 0) FourStarCount--;
                    break;
                case 3:
                    if (ThreeStarCount > 0) ThreeStarCount--;
                    break;
                case 2:
                    if (TwoStarCount > 0) TwoStarCount--;
                    break;
                case 1:
                    if (OneStarCount > 0) OneStarCount--;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(rating), "Rating must be between 1 and 5.");
            }
            TotalReviews--;
        }
        private void CalculatorAvgRating() {
            if (TotalReviews == 0)
            {
                AverageRating = 0;
                return;
            }
            double totalRating = (FiveStarCount * 5) + (FourStarCount * 4) + (ThreeStarCount * 3) + (TwoStarCount * 2) + OneStarCount;
            AverageRating = Math.Round(totalRating / TotalReviews, 2);
        }
        public void UpdateRating(int newRating, int? oldRating = 0)
        {
            if(oldRating > 0)
            {
                DecreaseRating(oldRating.Value);
            }
            IncreaseRating(newRating);
            CalculatorAvgRating();
            LastReviewedAt = DateTime.UtcNow;
        }
    }
}
