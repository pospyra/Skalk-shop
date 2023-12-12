using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Skalk.DAL.Migrations
{
    /// <inheritdoc />
    public partial class OfferIdItemCart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CompanyId",
                table: "ItemShoppingCarts",
                newName: "OfferId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OfferId",
                table: "ItemShoppingCarts",
                newName: "CompanyId");
        }
    }
}
