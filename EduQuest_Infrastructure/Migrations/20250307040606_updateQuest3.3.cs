using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduQuest_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateQuest33 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "QuestId",
                table: "UserQuests",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserQuests_QuestId",
                table: "UserQuests",
                column: "QuestId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserQuests_Quest_QuestId",
                table: "UserQuests",
                column: "QuestId",
                principalTable: "Quest",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserQuests_Quest_QuestId",
                table: "UserQuests");

            migrationBuilder.DropIndex(
                name: "IX_UserQuests_QuestId",
                table: "UserQuests");

            migrationBuilder.DropColumn(
                name: "QuestId",
                table: "UserQuests");
        }
    }
}
