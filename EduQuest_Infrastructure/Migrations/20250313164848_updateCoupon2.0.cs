using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduQuest_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateCoupon20 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Coupon_Course_CourseId",
                table: "Coupon");

            migrationBuilder.DropIndex(
                name: "IX_Coupon_CourseId",
                table: "Coupon");

            migrationBuilder.DropColumn(
                name: "UsedAt",
                table: "UserCoupon");

            migrationBuilder.DropColumn(
                name: "DiscountType",
                table: "Coupon");

            migrationBuilder.DropColumn(
                name: "IsCourseExclusive",
                table: "Coupon");

            migrationBuilder.RenameColumn(
                name: "RemainUsage",
                table: "Coupon",
                newName: "Limit");

            migrationBuilder.RenameColumn(
                name: "ExpireAt",
                table: "Coupon",
                newName: "StartTime");

            migrationBuilder.RenameColumn(
                name: "DiscountValue",
                table: "Coupon",
                newName: "Discount");

            migrationBuilder.RenameColumn(
                name: "CourseId",
                table: "Coupon",
                newName: "Description");

            migrationBuilder.AddColumn<int>(
                name: "AllowUsage",
                table: "UserCoupon",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RemainUsage",
                table: "UserCoupon",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AllowUsagePerUser",
                table: "Coupon",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpireTime",
                table: "Coupon",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CouponCourse",
                columns: table => new
                {
                    CouponsId = table.Column<string>(type: "text", nullable: false),
                    CourseId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CouponCourse", x => new { x.CouponsId, x.CourseId });
                    table.ForeignKey(
                        name: "FK_CouponCourse_Coupon_CouponsId",
                        column: x => x.CouponsId,
                        principalTable: "Coupon",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CouponCourse_Course_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Course",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CouponCourse_CourseId",
                table: "CouponCourse",
                column: "CourseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CouponCourse");

            migrationBuilder.DropColumn(
                name: "AllowUsage",
                table: "UserCoupon");

            migrationBuilder.DropColumn(
                name: "RemainUsage",
                table: "UserCoupon");

            migrationBuilder.DropColumn(
                name: "AllowUsagePerUser",
                table: "Coupon");

            migrationBuilder.DropColumn(
                name: "ExpireTime",
                table: "Coupon");

            migrationBuilder.RenameColumn(
                name: "StartTime",
                table: "Coupon",
                newName: "ExpireAt");

            migrationBuilder.RenameColumn(
                name: "Limit",
                table: "Coupon",
                newName: "RemainUsage");

            migrationBuilder.RenameColumn(
                name: "Discount",
                table: "Coupon",
                newName: "DiscountValue");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Coupon",
                newName: "CourseId");

            migrationBuilder.AddColumn<DateTime>(
                name: "UsedAt",
                table: "UserCoupon",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DiscountType",
                table: "Coupon",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsCourseExclusive",
                table: "Coupon",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Coupon_CourseId",
                table: "Coupon",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Coupon_Course_CourseId",
                table: "Coupon",
                column: "CourseId",
                principalTable: "Course",
                principalColumn: "Id");
        }
    }
}
