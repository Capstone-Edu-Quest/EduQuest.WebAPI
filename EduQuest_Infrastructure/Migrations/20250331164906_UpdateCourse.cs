using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduQuest_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCourse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Color",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "CouponDiscount",
                table: "Cart");

            migrationBuilder.DropColumn(
                name: "OriginalPrice",
                table: "Cart");

            migrationBuilder.RenameColumn(
                name: "Level",
                table: "Lesson",
                newName: "Index");

            migrationBuilder.RenameColumn(
                name: "Feature",
                table: "Course",
                newName: "OriginalCourseId");

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "Course",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Version",
                table: "Course");

            migrationBuilder.RenameColumn(
                name: "Index",
                table: "Lesson",
                newName: "Level");

            migrationBuilder.RenameColumn(
                name: "OriginalCourseId",
                table: "Course",
                newName: "Feature");

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "Course",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CouponDiscount",
                table: "Cart",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "OriginalPrice",
                table: "Cart",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
