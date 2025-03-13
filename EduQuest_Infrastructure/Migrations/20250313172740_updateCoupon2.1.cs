using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduQuest_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateCoupon21 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CouponId",
                table: "User",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_CouponId",
                table: "User",
                column: "CouponId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Coupon_CouponId",
                table: "User",
                column: "CouponId",
                principalTable: "Coupon",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Coupon_CouponId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_CouponId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "CouponId",
                table: "User");
        }
    }
}
