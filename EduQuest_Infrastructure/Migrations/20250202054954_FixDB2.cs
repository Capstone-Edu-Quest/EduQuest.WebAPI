using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduQuest_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixDB2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompletedAt",
                table: "LearningPathCourse");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "LearningPathCourse");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "LearningPathCourse");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "LearningPathCourse");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "LearningPathCourse");

            migrationBuilder.AddColumn<DateTime>(
                name: "CompletedAt",
                table: "LearningPath",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "LearningPath",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompletedAt",
                table: "LearningPath");

            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "LearningPath");

            migrationBuilder.AddColumn<DateTime>(
                name: "CompletedAt",
                table: "LearningPathCourse",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "LearningPathCourse",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "LearningPathCourse",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "LearningPathCourse",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "LearningPathCourse",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
