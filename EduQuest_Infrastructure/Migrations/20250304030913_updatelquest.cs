using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduQuest_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatelquest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuestUser");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "Quest");

            migrationBuilder.DropColumn(
                name: "Condition",
                table: "Quest");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Quest");

            migrationBuilder.RenameColumn(
                name: "RewardValue",
                table: "Quest",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "RewardType",
                table: "Quest",
                newName: "TimeToComplete");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Quest",
                newName: "CreatedBy");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Quest");

            migrationBuilder.CreateTable(
                name: "UserQuest",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<int>(type: "integer", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DueDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PointToComplete = table.Column<int>(type: "integer", nullable: true),
                    CurrentPoint = table.Column<int>(type: "integer", nullable: true),
                    IsCompleted = table.Column<bool>(type: "boolean", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserQuest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserQuest_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "QuestReward",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    RewardType = table.Column<string>(type: "text", nullable: true),
                    RewardValue = table.Column<string>(type: "text", nullable: true),
                    QuestId = table.Column<string>(type: "text", nullable: true),
                    UserQuestId = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestReward", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestReward_Quest_QuestId",
                        column: x => x.QuestId,
                        principalTable: "Quest",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_QuestReward_UserQuest_UserQuestId",
                        column: x => x.UserQuestId,
                        principalTable: "UserQuest",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Quest_CreatedBy",
                table: "Quest",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_QuestReward_DeletedAt",
                table: "QuestReward",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_QuestReward_QuestId",
                table: "QuestReward",
                column: "QuestId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestReward_UserQuestId",
                table: "QuestReward",
                column: "UserQuestId");

            migrationBuilder.CreateIndex(
                name: "IX_UserQuest_DeletedAt",
                table: "UserQuest",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_UserQuest_UserId",
                table: "UserQuest",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Quest_User_CreatedBy",
                table: "Quest",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quest_User_CreatedBy",
                table: "Quest");

            migrationBuilder.DropTable(
                name: "QuestReward");

            migrationBuilder.DropTable(
                name: "UserQuest");

            migrationBuilder.DropIndex(
                name: "IX_Quest_CreatedBy",
                table: "Quest");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Quest",
                newName: "RewardValue");

            migrationBuilder.RenameColumn(
                name: "TimeToComplete",
                table: "Quest",
                newName: "RewardType");

            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "Quest",
                newName: "Name");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Quest",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "Quest",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Condition",
                table: "Quest",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Quest",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "QuestUser",
                columns: table => new
                {
                    QuestsId = table.Column<string>(type: "text", nullable: false),
                    UsersId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestUser", x => new { x.QuestsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_QuestUser_Quest_QuestsId",
                        column: x => x.QuestsId,
                        principalTable: "Quest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuestUser_User_UsersId",
                        column: x => x.UsersId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuestUser_UsersId",
                table: "QuestUser",
                column: "UsersId");
        }
    }
}
