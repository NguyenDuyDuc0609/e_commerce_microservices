using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateRatingSumary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "_ratingSummary_TwoStarCount",
                table: "Products",
                newName: "RatingSummary_TwoStarCount");

            migrationBuilder.RenameColumn(
                name: "_ratingSummary_TotalReviews",
                table: "Products",
                newName: "RatingSummary_TotalReviews");

            migrationBuilder.RenameColumn(
                name: "_ratingSummary_ThreeStarCount",
                table: "Products",
                newName: "RatingSummary_ThreeStarCount");

            migrationBuilder.RenameColumn(
                name: "_ratingSummary_OneStarCount",
                table: "Products",
                newName: "RatingSummary_OneStarCount");

            migrationBuilder.RenameColumn(
                name: "_ratingSummary_LastReviewedAt",
                table: "Products",
                newName: "RatingSummary_LastReviewedAt");

            migrationBuilder.RenameColumn(
                name: "_ratingSummary_FourStarCount",
                table: "Products",
                newName: "RatingSummary_FourStarCount");

            migrationBuilder.RenameColumn(
                name: "_ratingSummary_FiveStarCount",
                table: "Products",
                newName: "RatingSummary_FiveStarCount");

            migrationBuilder.RenameColumn(
                name: "_ratingSummary_AverageRating",
                table: "Products",
                newName: "RatingSummary_AverageRating");

            migrationBuilder.AlterColumn<int>(
                name: "RatingSummary_TwoStarCount",
                table: "Products",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RatingSummary_TotalReviews",
                table: "Products",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RatingSummary_ThreeStarCount",
                table: "Products",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RatingSummary_OneStarCount",
                table: "Products",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "RatingSummary_LastReviewedAt",
                table: "Products",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RatingSummary_FourStarCount",
                table: "Products",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RatingSummary_FiveStarCount",
                table: "Products",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "RatingSummary_AverageRating",
                table: "Products",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RatingSummary_TwoStarCount",
                table: "Products",
                newName: "_ratingSummary_TwoStarCount");

            migrationBuilder.RenameColumn(
                name: "RatingSummary_TotalReviews",
                table: "Products",
                newName: "_ratingSummary_TotalReviews");

            migrationBuilder.RenameColumn(
                name: "RatingSummary_ThreeStarCount",
                table: "Products",
                newName: "_ratingSummary_ThreeStarCount");

            migrationBuilder.RenameColumn(
                name: "RatingSummary_OneStarCount",
                table: "Products",
                newName: "_ratingSummary_OneStarCount");

            migrationBuilder.RenameColumn(
                name: "RatingSummary_LastReviewedAt",
                table: "Products",
                newName: "_ratingSummary_LastReviewedAt");

            migrationBuilder.RenameColumn(
                name: "RatingSummary_FourStarCount",
                table: "Products",
                newName: "_ratingSummary_FourStarCount");

            migrationBuilder.RenameColumn(
                name: "RatingSummary_FiveStarCount",
                table: "Products",
                newName: "_ratingSummary_FiveStarCount");

            migrationBuilder.RenameColumn(
                name: "RatingSummary_AverageRating",
                table: "Products",
                newName: "_ratingSummary_AverageRating");

            migrationBuilder.AlterColumn<int>(
                name: "_ratingSummary_TwoStarCount",
                table: "Products",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "_ratingSummary_TotalReviews",
                table: "Products",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "_ratingSummary_ThreeStarCount",
                table: "Products",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "_ratingSummary_OneStarCount",
                table: "Products",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<DateTime>(
                name: "_ratingSummary_LastReviewedAt",
                table: "Products",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<int>(
                name: "_ratingSummary_FourStarCount",
                table: "Products",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "_ratingSummary_FiveStarCount",
                table: "Products",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<double>(
                name: "_ratingSummary_AverageRating",
                table: "Products",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision");
        }
    }
}
