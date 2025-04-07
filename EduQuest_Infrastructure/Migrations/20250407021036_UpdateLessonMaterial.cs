using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduQuest_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLessonMaterial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LessonMaterial_Lesson_LessonsId",
                table: "LessonMaterial");

            migrationBuilder.DropForeignKey(
                name: "FK_LessonMaterial_Material_MaterialsId",
                table: "LessonMaterial");

            migrationBuilder.DropTable(
                name: "LeaderboardUser");

            migrationBuilder.DropTable(
                name: "Leaderboard");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LessonMaterial",
                table: "LessonMaterial");

            migrationBuilder.RenameColumn(
                name: "MaterialsId",
                table: "LessonMaterial",
                newName: "MaterialId");

            migrationBuilder.RenameColumn(
                name: "LessonsId",
                table: "LessonMaterial",
                newName: "LessonId");

            migrationBuilder.RenameIndex(
                name: "IX_LessonMaterial_MaterialsId",
                table: "LessonMaterial",
                newName: "IX_LessonMaterial_MaterialId");

            migrationBuilder.AddColumn<int>(
                name: "AttemptNo",
                table: "QuizAttempt",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Material",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "LessonMaterial",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "LessonMaterial",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "LessonMaterial",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Index",
                table: "LessonMaterial",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "LessonMaterial",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "LessonMaterial",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CurrentLessonId",
                table: "CourseLearner",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CurrentMaterialId",
                table: "CourseLearner",
                type: "text",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_LessonMaterial",
                table: "LessonMaterial",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_LessonMaterial_DeletedAt",
                table: "LessonMaterial",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_LessonMaterial_LessonId",
                table: "LessonMaterial",
                column: "LessonId");

            migrationBuilder.AddForeignKey(
                name: "FK_LessonMaterial_Lesson_LessonId",
                table: "LessonMaterial",
                column: "LessonId",
                principalTable: "Lesson",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LessonMaterial_Material_MaterialId",
                table: "LessonMaterial",
                column: "MaterialId",
                principalTable: "Material",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LessonMaterial_Lesson_LessonId",
                table: "LessonMaterial");

            migrationBuilder.DropForeignKey(
                name: "FK_LessonMaterial_Material_MaterialId",
                table: "LessonMaterial");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LessonMaterial",
                table: "LessonMaterial");

            migrationBuilder.DropIndex(
                name: "IX_LessonMaterial_DeletedAt",
                table: "LessonMaterial");

            migrationBuilder.DropIndex(
                name: "IX_LessonMaterial_LessonId",
                table: "LessonMaterial");

            migrationBuilder.DropColumn(
                name: "AttemptNo",
                table: "QuizAttempt");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Material");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "LessonMaterial");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "LessonMaterial");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "LessonMaterial");

            migrationBuilder.DropColumn(
                name: "Index",
                table: "LessonMaterial");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "LessonMaterial");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "LessonMaterial");

            migrationBuilder.DropColumn(
                name: "CurrentLessonId",
                table: "CourseLearner");

            migrationBuilder.DropColumn(
                name: "CurrentMaterialId",
                table: "CourseLearner");

            migrationBuilder.RenameColumn(
                name: "MaterialId",
                table: "LessonMaterial",
                newName: "MaterialsId");

            migrationBuilder.RenameColumn(
                name: "LessonId",
                table: "LessonMaterial",
                newName: "LessonsId");

            migrationBuilder.RenameIndex(
                name: "IX_LessonMaterial_MaterialId",
                table: "LessonMaterial",
                newName: "IX_LessonMaterial_MaterialsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LessonMaterial",
                table: "LessonMaterial",
                columns: new[] { "LessonsId", "MaterialsId" });

            migrationBuilder.CreateTable(
                name: "Leaderboard",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    NumOfCourse = table.Column<int>(type: "integer", nullable: false),
                    TotalTime = table.Column<string>(type: "text", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leaderboard", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LeaderboardUser",
                columns: table => new
                {
                    LeaderboardsId = table.Column<string>(type: "text", nullable: false),
                    UsersId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaderboardUser", x => new { x.LeaderboardsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_LeaderboardUser_Leaderboard_LeaderboardsId",
                        column: x => x.LeaderboardsId,
                        principalTable: "Leaderboard",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LeaderboardUser_User_UsersId",
                        column: x => x.UsersId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Leaderboard_DeletedAt",
                table: "Leaderboard",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_LeaderboardUser_UsersId",
                table: "LeaderboardUser",
                column: "UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_LessonMaterial_Lesson_LessonsId",
                table: "LessonMaterial",
                column: "LessonsId",
                principalTable: "Lesson",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LessonMaterial_Material_MaterialsId",
                table: "LessonMaterial",
                column: "MaterialsId",
                principalTable: "Material",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
