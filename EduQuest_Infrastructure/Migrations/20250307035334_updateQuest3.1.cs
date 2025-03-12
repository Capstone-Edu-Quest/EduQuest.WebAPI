using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduQuest_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateQuest31 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeToComplete",
                table: "Quest");

            migrationBuilder.CreateTable(
                name: "UserQuestQuestReward",
                columns: table => new
                {
                    UserQuestId = table.Column<string>(type: "text", nullable: false),
                    QuestRewardId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserQuestQuestReward", x => new { x.UserQuestId, x.QuestRewardId });
                    table.ForeignKey(
                        name: "FK_UserQuestQuestReward_QuestRewards_QuestRewardId",
                        column: x => x.QuestRewardId,
                        principalTable: "QuestRewards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserQuestQuestReward_UserQuests_UserQuestId",
                        column: x => x.UserQuestId,
                        principalTable: "UserQuests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserQuestQuestReward_QuestRewardId",
                table: "UserQuestQuestReward",
                column: "QuestRewardId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserQuestQuestReward");

            migrationBuilder.AddColumn<string>(
                name: "TimeToComplete",
                table: "Quest",
                type: "text",
                nullable: true);
        }
    }
}
