using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduQuest_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateStripe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BankAccountId",
                table: "User",
                newName: "StripeAccountId");

            migrationBuilder.AddColumn<string>(
                name: "TransferGoup",
                table: "TransactionDetail",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransferGoup",
                table: "TransactionDetail");

            migrationBuilder.RenameColumn(
                name: "StripeAccountId",
                table: "User",
                newName: "BankAccountId");
        }
    }
}
