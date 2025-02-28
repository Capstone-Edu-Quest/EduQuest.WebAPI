using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduQuest_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateCart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalAmount",
                table: "Payment");

            migrationBuilder.RenameColumn(
                name: "TotalPrice",
                table: "Cart",
                newName: "OriginalPrice");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Transaction",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<decimal>(
                name: "DiscountValue",
                table: "Coupon",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AddColumn<string>(
                name: "DiscountType",
                table: "Coupon",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "CouponDiscount",
                table: "Cart",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Total",
                table: "Cart",
                type: "numeric",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "DiscountType",
                table: "Coupon");

            migrationBuilder.DropColumn(
                name: "CouponDiscount",
                table: "Cart");

            migrationBuilder.DropColumn(
                name: "Total",
                table: "Cart");

            migrationBuilder.RenameColumn(
                name: "OriginalPrice",
                table: "Cart",
                newName: "TotalPrice");

            migrationBuilder.AddColumn<decimal>(
                name: "TotalAmount",
                table: "Payment",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<double>(
                name: "DiscountValue",
                table: "Coupon",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");
        }
    }
}
