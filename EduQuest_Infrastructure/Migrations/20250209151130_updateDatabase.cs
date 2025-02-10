using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduQuest_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShopItem_MascotInventory_UserMascotId",
                table: "ShopItem");

            migrationBuilder.DropIndex(
                name: "IX_MascotInventory_UserId",
                table: "MascotInventory");

            migrationBuilder.RenameColumn(
                name: "UserMascotId",
                table: "ShopItem",
                newName: "MascotItemId");

            migrationBuilder.RenameIndex(
                name: "IX_ShopItem_UserMascotId",
                table: "ShopItem",
                newName: "IX_ShopItem_MascotItemId");

            migrationBuilder.CreateIndex(
                name: "IX_MascotInventory_UserId",
                table: "MascotInventory",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShopItem_MascotInventory_MascotItemId",
                table: "ShopItem",
                column: "MascotItemId",
                principalTable: "MascotInventory",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShopItem_MascotInventory_MascotItemId",
                table: "ShopItem");

            migrationBuilder.DropIndex(
                name: "IX_MascotInventory_UserId",
                table: "MascotInventory");

            migrationBuilder.RenameColumn(
                name: "MascotItemId",
                table: "ShopItem",
                newName: "UserMascotId");

            migrationBuilder.RenameIndex(
                name: "IX_ShopItem_MascotItemId",
                table: "ShopItem",
                newName: "IX_ShopItem_UserMascotId");

            migrationBuilder.CreateIndex(
                name: "IX_MascotInventory_UserId",
                table: "MascotInventory",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ShopItem_MascotInventory_UserMascotId",
                table: "ShopItem",
                column: "UserMascotId",
                principalTable: "MascotInventory",
                principalColumn: "Id");
        }
    }
}
