using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduQuest_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateQuest3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuestReward_Quest_QuestId",
                table: "QuestReward");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestReward_UserQuest_UserQuestId",
                table: "QuestReward");

            migrationBuilder.DropForeignKey(
                name: "FK_UserQuest_User_UserId",
                table: "UserQuest");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserQuest",
                table: "UserQuest");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QuestReward",
                table: "QuestReward");

            migrationBuilder.RenameTable(
                name: "UserQuest",
                newName: "UserQuests");

            migrationBuilder.RenameTable(
                name: "QuestReward",
                newName: "QuestRewards");

            migrationBuilder.RenameIndex(
                name: "IX_UserQuest_UserId",
                table: "UserQuests",
                newName: "IX_UserQuests_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserQuest_DeletedAt",
                table: "UserQuests",
                newName: "IX_UserQuests_DeletedAt");

            migrationBuilder.RenameIndex(
                name: "IX_QuestReward_UserQuestId",
                table: "QuestRewards",
                newName: "IX_QuestRewards_UserQuestId");

            migrationBuilder.RenameIndex(
                name: "IX_QuestReward_QuestId",
                table: "QuestRewards",
                newName: "IX_QuestRewards_QuestId");

            migrationBuilder.RenameIndex(
                name: "IX_QuestReward_DeletedAt",
                table: "QuestRewards",
                newName: "IX_QuestRewards_DeletedAt");

            migrationBuilder.AddColumn<bool>(
                name: "IsDaily",
                table: "Quest",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PointToComplete",
                table: "Quest",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDaily",
                table: "UserQuests",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserQuests",
                table: "UserQuests",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuestRewards",
                table: "QuestRewards",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_QuestRewards_Quest_QuestId",
                table: "QuestRewards",
                column: "QuestId",
                principalTable: "Quest",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_QuestRewards_UserQuests_UserQuestId",
                table: "QuestRewards",
                column: "UserQuestId",
                principalTable: "UserQuests",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserQuests_User_UserId",
                table: "UserQuests",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuestRewards_Quest_QuestId",
                table: "QuestRewards");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestRewards_UserQuests_UserQuestId",
                table: "QuestRewards");

            migrationBuilder.DropForeignKey(
                name: "FK_UserQuests_User_UserId",
                table: "UserQuests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserQuests",
                table: "UserQuests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QuestRewards",
                table: "QuestRewards");

            migrationBuilder.DropColumn(
                name: "IsDaily",
                table: "Quest");

            migrationBuilder.DropColumn(
                name: "PointToComplete",
                table: "Quest");

            migrationBuilder.DropColumn(
                name: "IsDaily",
                table: "UserQuests");

            migrationBuilder.RenameTable(
                name: "UserQuests",
                newName: "UserQuest");

            migrationBuilder.RenameTable(
                name: "QuestRewards",
                newName: "QuestReward");

            migrationBuilder.RenameIndex(
                name: "IX_UserQuests_UserId",
                table: "UserQuest",
                newName: "IX_UserQuest_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserQuests_DeletedAt",
                table: "UserQuest",
                newName: "IX_UserQuest_DeletedAt");

            migrationBuilder.RenameIndex(
                name: "IX_QuestRewards_UserQuestId",
                table: "QuestReward",
                newName: "IX_QuestReward_UserQuestId");

            migrationBuilder.RenameIndex(
                name: "IX_QuestRewards_QuestId",
                table: "QuestReward",
                newName: "IX_QuestReward_QuestId");

            migrationBuilder.RenameIndex(
                name: "IX_QuestRewards_DeletedAt",
                table: "QuestReward",
                newName: "IX_QuestReward_DeletedAt");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserQuest",
                table: "UserQuest",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_QuestReward",
                table: "QuestReward",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_QuestReward_Quest_QuestId",
                table: "QuestReward",
                column: "QuestId",
                principalTable: "Quest",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_QuestReward_UserQuest_UserQuestId",
                table: "QuestReward",
                column: "UserQuestId",
                principalTable: "UserQuest",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserQuest_User_UserId",
                table: "UserQuest",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id");
        }
    }
}
