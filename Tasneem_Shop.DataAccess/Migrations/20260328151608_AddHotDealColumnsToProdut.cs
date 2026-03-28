using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tasneem_Shop.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddHotDealColumnsToProdut : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EndTime",
                table: "Products",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsHotDeal",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "OfferPrice",
                table: "Products",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IsHotDeal",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "OfferPrice",
                table: "Products");
        }
    }
}
