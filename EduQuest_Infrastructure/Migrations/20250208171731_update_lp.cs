using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduQuest_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class update_lp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EstimateDuration",
                table: "LearningPath");

            migrationBuilder.RenameColumn(
                name: "IsDuplicated",
                table: "LearningPath",
                newName: "IsEnrolled");

            migrationBuilder.AddColumn<int>(
                name: "TotalTimes",
                table: "LearningPath",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalTimes",
                table: "LearningPath");

            migrationBuilder.RenameColumn(
                name: "IsEnrolled",
                table: "LearningPath",
                newName: "IsDuplicated");

            migrationBuilder.AddColumn<string>(
                name: "EstimateDuration",
                table: "LearningPath",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
