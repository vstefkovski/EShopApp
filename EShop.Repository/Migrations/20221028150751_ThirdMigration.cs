using Microsoft.EntityFrameworkCore.Migrations;

namespace EShop.Repository.Migrations
{
    public partial class ThirdMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductInShoppingCarts",
                table: "ProductInShoppingCarts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductInOrder",
                table: "ProductInOrder");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductInShoppingCarts",
                table: "ProductInShoppingCarts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductInOrder",
                table: "ProductInOrder",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ProductInShoppingCarts_ProductId",
                table: "ProductInShoppingCarts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductInOrder_ProductId",
                table: "ProductInOrder",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductInShoppingCarts",
                table: "ProductInShoppingCarts");

            migrationBuilder.DropIndex(
                name: "IX_ProductInShoppingCarts_ProductId",
                table: "ProductInShoppingCarts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductInOrder",
                table: "ProductInOrder");

            migrationBuilder.DropIndex(
                name: "IX_ProductInOrder_ProductId",
                table: "ProductInOrder");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductInShoppingCarts",
                table: "ProductInShoppingCarts",
                columns: new[] { "ProductId", "ShoppingCartId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductInOrder",
                table: "ProductInOrder",
                columns: new[] { "ProductId", "OrderId" });
        }
    }
}
