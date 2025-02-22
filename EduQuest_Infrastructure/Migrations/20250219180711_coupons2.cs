using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduQuest_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class coupons2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Coupon",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Coupon_CreatedBy",
                table: "Coupon",
                column: "CreatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_Coupon_User_CreatedBy",
                table: "Coupon",
                column: "CreatedBy",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Coupon_User_CreatedBy",
                table: "Coupon");

            migrationBuilder.DropIndex(
                name: "IX_Coupon_CreatedBy",
                table: "Coupon");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Coupon");
        }
    }
}
