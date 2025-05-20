using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduQuest_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ItemShards : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TagId",
                table: "ShopItem",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ItemShards",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    TagId = table.Column<string>(type: "text", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemShards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemShards_Tag_TagId",
                        column: x => x.TagId,
                        principalTable: "Tag",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemShards_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShopItem_TagId",
                table: "ShopItem",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemShards_DeletedAt",
                table: "ItemShards",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ItemShards_TagId",
                table: "ItemShards",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemShards_UserId",
                table: "ItemShards",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShopItem_Tag_TagId",
                table: "ShopItem",
                column: "TagId",
                principalTable: "Tag",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShopItem_Tag_TagId",
                table: "ShopItem");

            migrationBuilder.DropTable(
                name: "ItemShards");

            migrationBuilder.DropIndex(
                name: "IX_ShopItem_TagId",
                table: "ShopItem");

            migrationBuilder.DropColumn(
                name: "TagId",
                table: "ShopItem");
        }
    }
}
