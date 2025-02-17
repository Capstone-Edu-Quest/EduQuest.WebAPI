using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduQuest_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class coupons1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastUpdated",
                table: "Course");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Course",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Coupon",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    DiscountValue = table.Column<double>(type: "double precision", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    IsCourseExclusive = table.Column<bool>(type: "boolean", nullable: false),
                    CourseId = table.Column<string>(type: "text", nullable: true),
                    ExpireAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Usage = table.Column<int>(type: "integer", nullable: false),
                    RemainUsage = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coupon", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Coupon_Course_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Course",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Coupon_CourseId",
                table: "Coupon",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Coupon_DeletedAt",
                table: "Coupon",
                column: "DeletedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Coupon");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Course");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdated",
                table: "Course",
                type: "timestamp with time zone",
                nullable: true);
        }
    }
}
