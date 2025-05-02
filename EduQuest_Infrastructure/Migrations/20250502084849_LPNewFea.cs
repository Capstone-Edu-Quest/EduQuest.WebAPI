using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduQuest_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class LPNewFea : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                    LearningPathId = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enrollers", x => new { x.LearningPathId, x.UserId });
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
                name: "IX_Enrollers_UserId",
                table: "Enrollers",
                column: "UserId");
        }
    }
}
