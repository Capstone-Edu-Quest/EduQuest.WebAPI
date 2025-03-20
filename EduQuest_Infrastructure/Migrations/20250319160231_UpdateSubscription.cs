using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduQuest_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSubscription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BenefitsJson",
                table: "Subscription");

            migrationBuilder.DropColumn(
                name: "MonthlyPrice",
                table: "Subscription");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Subscription");

            migrationBuilder.DropColumn(
                name: "YearlyPrice",
                table: "Subscription");

            migrationBuilder.RenameColumn(
                name: "Package",
                table: "Subscription",
                newName: "RoleId");

            migrationBuilder.AlterColumn<decimal>(
                name: "Value",
                table: "Subscription",
                type: "numeric",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Config",
                table: "Subscription",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PackageType",
                table: "Subscription",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Subscription_RoleId",
                table: "Subscription",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subscription_Role_RoleId",
                table: "Subscription",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subscription_Role_RoleId",
                table: "Subscription");

            migrationBuilder.DropIndex(
                name: "IX_Subscription_RoleId",
                table: "Subscription");

            migrationBuilder.DropColumn(
                name: "Config",
                table: "Subscription");

            migrationBuilder.DropColumn(
                name: "PackageType",
                table: "Subscription");

            migrationBuilder.RenameColumn(
                name: "RoleId",
                table: "Subscription",
                newName: "Package");

            migrationBuilder.AlterColumn<decimal>(
                name: "Value",
                table: "Subscription",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AddColumn<string>(
                name: "BenefitsJson",
                table: "Subscription",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MonthlyPrice",
                table: "Subscription",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Subscription",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "YearlyPrice",
                table: "Subscription",
                type: "numeric",
                nullable: true);
        }
    }
}
