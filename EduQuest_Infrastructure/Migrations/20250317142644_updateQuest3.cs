using System;
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
            migrationBuilder.DropTable(
                name: "UserQuestReward");

            migrationBuilder.DropTable(
                name: "QuestRewards");

            migrationBuilder.AddColumn<string>(
                name: "RewardTypes",
                table: "UserQuest",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RewardValues",
                table: "UserQuest",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RewardTypes",
                table: "Quest",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RewardValues",
                table: "Quest",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RewardTypes",
                table: "UserQuest");

            migrationBuilder.DropColumn(
                name: "RewardValues",
                table: "UserQuest");

            migrationBuilder.DropColumn(
                name: "RewardTypes",
                table: "Quest");

            migrationBuilder.DropColumn(
                name: "RewardValues",
                table: "Quest");

            migrationBuilder.CreateTable(
                name: "QuestRewards",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    QuestId = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RewardType = table.Column<int>(type: "integer", nullable: true),
                    RewardValue = table.Column<int>(type: "integer", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestRewards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestRewards_Quest_QuestId",
                        column: x => x.QuestId,
                        principalTable: "Quest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserQuestReward",
                columns: table => new
                {
                    UserQuestId = table.Column<string>(type: "text", nullable: false),
                    RewardId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Id = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserQuestReward", x => new { x.UserQuestId, x.RewardId });
                    table.ForeignKey(
                        name: "FK_UserQuestReward_QuestRewards_RewardId",
                        column: x => x.RewardId,
                        principalTable: "QuestRewards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserQuestReward_UserQuest_UserQuestId",
                        column: x => x.UserQuestId,
                        principalTable: "UserQuest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuestRewards_DeletedAt",
                table: "QuestRewards",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_QuestRewards_QuestId",
                table: "QuestRewards",
                column: "QuestId");

            migrationBuilder.CreateIndex(
                name: "IX_UserQuestReward_DeletedAt",
                table: "UserQuestReward",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_UserQuestReward_RewardId",
                table: "UserQuestReward",
                column: "RewardId",
                unique: true);
        }
    }
}
