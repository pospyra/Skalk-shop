using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Skalk.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ChangeShoppingCartInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "ItemShoppingCarts",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalAmount",
                table: "ItemShoppingCarts",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "ItemShoppingCarts");

            migrationBuilder.DropColumn(
                name: "TotalAmount",
                table: "ItemShoppingCarts");
        }
    }
}
