using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduQuest_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class minorMinor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsLocked",
                table: "LearningPath");

            migrationBuilder.AddColumn<string>(
                name: "ExpertiseTagId",
                table: "User",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_ExpertiseTagId",
                table: "User",
                column: "ExpertiseTagId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Tag_ExpertiseTagId",
                table: "User",
                column: "ExpertiseTagId",
                principalTable: "Tag",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Tag_ExpertiseTagId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_ExpertiseTagId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "ExpertiseTagId",
                table: "User");

            migrationBuilder.AddColumn<bool>(
                name: "IsLocked",
                table: "LearningPath",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
