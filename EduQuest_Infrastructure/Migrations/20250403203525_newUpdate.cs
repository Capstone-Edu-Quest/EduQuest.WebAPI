using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduQuest_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class newUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "StudyTimes",
                table: "StudyTimes",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "StudyTimes",
                table: "StudyTimes",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }
    }
}
