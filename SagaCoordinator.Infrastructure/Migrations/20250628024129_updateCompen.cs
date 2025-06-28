using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SagaCoordinator.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateCompen : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ExpireAt",
                table: "RegisterSagaStates",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpireAt",
                table: "RegisterSagaStates");
        }
    }
}
