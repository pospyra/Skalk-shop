using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Skalk.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ShoppingCart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "ItemShoppingCarts",
                newName: "Quantity");

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "ItemShoppingCarts",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Mpn",
                table: "ItemShoppingCarts",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "ItemShoppingCarts");

            migrationBuilder.DropColumn(
                name: "Mpn",
                table: "ItemShoppingCarts");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "ItemShoppingCarts",
                newName: "ProductId");
        }
    }
}
