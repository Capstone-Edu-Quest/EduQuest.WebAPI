using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduQuest_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTagNd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Grade",
                table: "Tag");

            migrationBuilder.DropColumn(
                name: "Level",
                table: "Tag");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Grade",
                table: "Tag",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Level",
                table: "Tag",
                type: "integer",
                nullable: true);
        }
    }
}
