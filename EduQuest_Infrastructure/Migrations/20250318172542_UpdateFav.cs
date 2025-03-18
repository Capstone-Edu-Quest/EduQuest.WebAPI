using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduQuest_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFav : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "FavoriteList",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteList_UserId1",
                table: "FavoriteList",
                column: "UserId1",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FavoriteList_User_UserId1",
                table: "FavoriteList",
                column: "UserId1",
                principalTable: "User",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FavoriteList_User_UserId1",
                table: "FavoriteList");

            migrationBuilder.DropIndex(
                name: "IX_FavoriteList_UserId1",
                table: "FavoriteList");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "FavoriteList");
        }
    }
}
