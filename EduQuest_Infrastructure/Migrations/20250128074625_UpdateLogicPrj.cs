using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduQuest_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLogicPrj : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Leaderboard_Course_CourseId",
                table: "Leaderboard");

            migrationBuilder.DropTable(
                name: "AchievementUser");

            migrationBuilder.DropTable(
                name: "CourseAchievement");

            migrationBuilder.DropTable(
                name: "Reward");

            migrationBuilder.DropTable(
                name: "Achievement");

            migrationBuilder.DropIndex(
                name: "IX_Leaderboard_CourseId",
                table: "Leaderboard");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "Leaderboard");

            migrationBuilder.DropColumn(
                name: "Rank",
                table: "Leaderboard");

            migrationBuilder.RenameColumn(
                name: "Score",
                table: "Leaderboard",
                newName: "NumOfCourse");

            migrationBuilder.AlterColumn<int>(
                name: "TotalActiveDay",
                table: "UserStatistic",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "MaxStudyStreakDay",
                table: "UserStatistic",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastLearningDay",
                table: "UserStatistic",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<int>(
                name: "CompletedCourses",
                table: "UserStatistic",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "Exp",
                table: "UserStatistic",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Gold",
                table: "UserStatistic",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Level",
                table: "UserStatistic",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StudyTime",
                table: "UserStatistic",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TotalTime",
                table: "Leaderboard",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Level",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpLevelReward = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpPerLevel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LevelRequirement = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Level", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Quest",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RewardType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RewardValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Condition = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quest", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShopItem",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Slot = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopItem", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserMascot",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ShopItemId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserMascot", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserMascot_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuestUser",
                columns: table => new
                {
                    QuestsId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UsersId = table.Column<string>(type: "nvarchar(450)", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "ShopItemUserMascot",
                columns: table => new
                {
                    ShopItemsId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserMascotId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShopItemUserMascot", x => new { x.ShopItemsId, x.UserMascotId });
                    table.ForeignKey(
                        name: "FK_ShopItemUserMascot_ShopItem_ShopItemsId",
                        column: x => x.ShopItemsId,
                        principalTable: "ShopItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShopItemUserMascot_UserMascot_UserMascotId",
                        column: x => x.UserMascotId,
                        principalTable: "UserMascot",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Level_DeletedAt",
                table: "Level",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Quest_DeletedAt",
                table: "Quest",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_QuestUser_UsersId",
                table: "QuestUser",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_ShopItem_DeletedAt",
                table: "ShopItem",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ShopItemUserMascot_UserMascotId",
                table: "ShopItemUserMascot",
                column: "UserMascotId");

            migrationBuilder.CreateIndex(
                name: "IX_UserMascot_DeletedAt",
                table: "UserMascot",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_UserMascot_UserId",
                table: "UserMascot",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Level");

            migrationBuilder.DropTable(
                name: "QuestUser");

            migrationBuilder.DropTable(
                name: "ShopItemUserMascot");

            migrationBuilder.DropTable(
                name: "Quest");

            migrationBuilder.DropTable(
                name: "ShopItem");

            migrationBuilder.DropTable(
                name: "UserMascot");

            migrationBuilder.DropColumn(
                name: "Exp",
                table: "UserStatistic");

            migrationBuilder.DropColumn(
                name: "Gold",
                table: "UserStatistic");

            migrationBuilder.DropColumn(
                name: "Level",
                table: "UserStatistic");

            migrationBuilder.DropColumn(
                name: "StudyTime",
                table: "UserStatistic");

            migrationBuilder.DropColumn(
                name: "TotalTime",
                table: "Leaderboard");

            migrationBuilder.RenameColumn(
                name: "NumOfCourse",
                table: "Leaderboard",
                newName: "Score");

            migrationBuilder.AlterColumn<int>(
                name: "TotalActiveDay",
                table: "UserStatistic",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MaxStudyStreakDay",
                table: "UserStatistic",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastLearningDay",
                table: "UserStatistic",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CompletedCourses",
                table: "UserStatistic",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CourseId",
                table: "Leaderboard",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Rank",
                table: "Leaderboard",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Achievement",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Achievement", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CourseAchievement",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CourseId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseAchievement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseAchievement_Course_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Course",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reward",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StageId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RewardType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RewardValue = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reward", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reward_Stage_StageId",
                        column: x => x.StageId,
                        principalTable: "Stage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AchievementUser",
                columns: table => new
                {
                    AchievementsId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UsersId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AchievementUser", x => new { x.AchievementsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_AchievementUser_Achievement_AchievementsId",
                        column: x => x.AchievementsId,
                        principalTable: "Achievement",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AchievementUser_User_UsersId",
                        column: x => x.UsersId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Leaderboard_CourseId",
                table: "Leaderboard",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Achievement_DeletedAt",
                table: "Achievement",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_AchievementUser_UsersId",
                table: "AchievementUser",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseAchievement_CourseId",
                table: "CourseAchievement",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseAchievement_DeletedAt",
                table: "CourseAchievement",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Reward_DeletedAt",
                table: "Reward",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Reward_StageId",
                table: "Reward",
                column: "StageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Leaderboard_Course_CourseId",
                table: "Leaderboard",
                column: "CourseId",
                principalTable: "Course",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
