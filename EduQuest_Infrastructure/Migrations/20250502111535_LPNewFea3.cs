using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduQuest_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class LPNewFea3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DueDate",
                table: "LearningPathCourse");

            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "LearningPathCourse");

            migrationBuilder.DropColumn(
                name: "IsOverDue",
                table: "LearningPathCourse");

            migrationBuilder.CreateTable(
                name: "Enrollers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    LearningPathId = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    CourseId = table.Column<string>(type: "text", nullable: false),
                    CourseOrder = table.Column<int>(type: "integer", nullable: false),
                    DueDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsOverDue = table.Column<bool>(type: "boolean", nullable: false),
                    IsCompleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enrollers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Enrollers_Course_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Course",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Enrollers_LearningPath_LearningPathId",
                        column: x => x.LearningPathId,
                        principalTable: "LearningPath",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Enrollers_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Enrollers_CourseId",
                table: "Enrollers",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollers_DeletedAt",
                table: "Enrollers",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollers_LearningPathId",
                table: "Enrollers",
                column: "LearningPathId");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollers_UserId",
                table: "Enrollers",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Enrollers");

            migrationBuilder.AddColumn<DateTime>(
                name: "DueDate",
                table: "LearningPathCourse",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "LearningPathCourse",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsOverDue",
                table: "LearningPathCourse",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
