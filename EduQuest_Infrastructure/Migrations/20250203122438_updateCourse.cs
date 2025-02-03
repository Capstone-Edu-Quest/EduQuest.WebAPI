using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduQuest_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateCourse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LearningMaterial_StageId",
                table: "LearningMaterial");

            migrationBuilder.AddColumn<double>(
                name: "Rating",
                table: "UserStatistic",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TotalCourseCreated",
                table: "UserStatistic",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TotalLearner",
                table: "UserStatistic",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TotalReview",
                table: "UserStatistic",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TotalTime",
                table: "Stage",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Duration",
                table: "LearningMaterial",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Feature",
                table: "Course",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CourseStatistic",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CourseId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TotalLesson = table.Column<int>(type: "int", nullable: true),
                    TotalTime = table.Column<int>(type: "int", nullable: true),
                    TotalLearner = table.Column<int>(type: "int", nullable: true),
                    Rating = table.Column<double>(type: "float", nullable: true),
                    TotalReview = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseStatistic", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseStatistic_Course_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Course",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LearningMaterial_StageId",
                table: "LearningMaterial",
                column: "StageId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseStatistic_CourseId",
                table: "CourseStatistic",
                column: "CourseId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CourseStatistic_DeletedAt",
                table: "CourseStatistic",
                column: "DeletedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseStatistic");

            migrationBuilder.DropIndex(
                name: "IX_LearningMaterial_StageId",
                table: "LearningMaterial");

            migrationBuilder.DropColumn(
                name: "Rating",
                table: "UserStatistic");

            migrationBuilder.DropColumn(
                name: "TotalCourseCreated",
                table: "UserStatistic");

            migrationBuilder.DropColumn(
                name: "TotalLearner",
                table: "UserStatistic");

            migrationBuilder.DropColumn(
                name: "TotalReview",
                table: "UserStatistic");

            migrationBuilder.DropColumn(
                name: "TotalTime",
                table: "Stage");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "LearningMaterial");

            migrationBuilder.DropColumn(
                name: "Feature",
                table: "Course");

            migrationBuilder.CreateIndex(
                name: "IX_LearningMaterial_StageId",
                table: "LearningMaterial",
                column: "StageId",
                unique: true);
        }
    }
}
