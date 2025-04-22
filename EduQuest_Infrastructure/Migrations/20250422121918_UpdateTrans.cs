using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduQuest_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTrans : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BaseTransactionId",
                table: "Transaction",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "StudyTimes",
                table: "StudyTimes",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BaseTransactionId",
                table: "Transaction");

            migrationBuilder.AlterColumn<int>(
                name: "StudyTimes",
                table: "StudyTimes",
                type: "integer",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision");
        }
    }
}
