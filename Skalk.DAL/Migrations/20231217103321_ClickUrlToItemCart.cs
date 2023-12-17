using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Skalk.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ClickUrlToItemCart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClickUrl",
                table: "ItemShoppingCarts",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClickUrl",
                table: "ItemShoppingCarts");
        }
    }
}
