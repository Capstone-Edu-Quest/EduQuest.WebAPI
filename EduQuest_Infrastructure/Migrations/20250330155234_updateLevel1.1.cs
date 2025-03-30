using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduQuest_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateLevel11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LevelNumber",
                table: "Levels");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LevelNumber",
                table: "Levels",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
