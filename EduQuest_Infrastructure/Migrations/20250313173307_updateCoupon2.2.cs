using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduQuest_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateCoupon22 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "CouponUser",
                columns: table => new
                {
                    CouponId = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CouponUser", x => new { x.CouponId, x.UserId });
                    table.ForeignKey(
                        name: "FK_CouponUser_Coupon_CouponId",
                        column: x => x.CouponId,
                        principalTable: "Coupon",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CouponUser_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CouponUser_UserId",
                table: "CouponUser",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CouponUser");

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
    }
}
