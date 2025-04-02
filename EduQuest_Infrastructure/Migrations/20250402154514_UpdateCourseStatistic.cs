using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduQuest_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCourseStatistic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalRefund",
                table: "CourseStatistic");

            migrationBuilder.DropColumn(
                name: "TotalRevenue",
                table: "CourseStatistic");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "TotalRefund",
                table: "CourseStatistic",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "TotalRevenue",
                table: "CourseStatistic",
                type: "double precision",
                nullable: true);
        }
    }
}
