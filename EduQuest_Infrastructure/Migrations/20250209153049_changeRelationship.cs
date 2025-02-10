using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduQuest_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changeRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShopItem_MascotInventory_MascotItemId",
                table: "ShopItem");

            migrationBuilder.DropIndex(
                name: "IX_ShopItem_MascotItemId",
                table: "ShopItem");

            migrationBuilder.DropColumn(
                name: "MascotItemId",
                table: "ShopItem");

            migrationBuilder.CreateIndex(
                name: "IX_MascotInventory_ShopItemId",
                table: "MascotInventory",
                column: "ShopItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_MascotInventory_ShopItem_ShopItemId",
                table: "MascotInventory",
                column: "ShopItemId",
                principalTable: "ShopItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MascotInventory_ShopItem_ShopItemId",
                table: "MascotInventory");

            migrationBuilder.DropIndex(
                name: "IX_MascotInventory_ShopItemId",
                table: "MascotInventory");

            migrationBuilder.AddColumn<string>(
                name: "MascotItemId",
                table: "ShopItem",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShopItem_MascotItemId",
                table: "ShopItem",
                column: "MascotItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShopItem_MascotInventory_MascotItemId",
                table: "ShopItem",
                column: "MascotItemId",
                principalTable: "MascotInventory",
                principalColumn: "Id");
        }
    }
}
