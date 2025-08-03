using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateNavigation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProductRatingSummaries_TwoStarCount",
                table: "Products",
                newName: "_ratingSummary_TwoStarCount");

            migrationBuilder.RenameColumn(
                name: "ProductRatingSummaries_TotalReviews",
                table: "Products",
                newName: "_ratingSummary_TotalReviews");

            migrationBuilder.RenameColumn(
                name: "ProductRatingSummaries_ThreeStarCount",
                table: "Products",
                newName: "_ratingSummary_ThreeStarCount");

            migrationBuilder.RenameColumn(
                name: "ProductRatingSummaries_OneStarCount",
                table: "Products",
                newName: "_ratingSummary_OneStarCount");

            migrationBuilder.RenameColumn(
                name: "ProductRatingSummaries_LastReviewedAt",
                table: "Products",
                newName: "_ratingSummary_LastReviewedAt");

            migrationBuilder.RenameColumn(
                name: "ProductRatingSummaries_FourStarCount",
                table: "Products",
                newName: "_ratingSummary_FourStarCount");

            migrationBuilder.RenameColumn(
                name: "ProductRatingSummaries_FiveStarCount",
                table: "Products",
                newName: "_ratingSummary_FiveStarCount");

            migrationBuilder.RenameColumn(
                name: "ProductRatingSummaries_AverageRating",
                table: "Products",
                newName: "_ratingSummary_AverageRating");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "_ratingSummary_TwoStarCount",
                table: "Products",
                newName: "ProductRatingSummaries_TwoStarCount");

            migrationBuilder.RenameColumn(
                name: "_ratingSummary_TotalReviews",
                table: "Products",
                newName: "ProductRatingSummaries_TotalReviews");

            migrationBuilder.RenameColumn(
                name: "_ratingSummary_ThreeStarCount",
                table: "Products",
                newName: "ProductRatingSummaries_ThreeStarCount");

            migrationBuilder.RenameColumn(
                name: "_ratingSummary_OneStarCount",
                table: "Products",
                newName: "ProductRatingSummaries_OneStarCount");

            migrationBuilder.RenameColumn(
                name: "_ratingSummary_LastReviewedAt",
                table: "Products",
                newName: "ProductRatingSummaries_LastReviewedAt");

            migrationBuilder.RenameColumn(
                name: "_ratingSummary_FourStarCount",
                table: "Products",
                newName: "ProductRatingSummaries_FourStarCount");

            migrationBuilder.RenameColumn(
                name: "_ratingSummary_FiveStarCount",
                table: "Products",
                newName: "ProductRatingSummaries_FiveStarCount");

            migrationBuilder.RenameColumn(
                name: "_ratingSummary_AverageRating",
                table: "Products",
                newName: "ProductRatingSummaries_AverageRating");
        }
    }
}
