using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduQuest_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class QAAttempt3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Score",
                table: "AssignmentReviews");

            migrationBuilder.AddColumn<int>(
                name: "Grade",
                table: "AssignmentReviews",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentReviews_ReviewerId",
                table: "AssignmentReviews",
                column: "ReviewerId");

            migrationBuilder.AddForeignKey(
                name: "FK_AssignmentReviews_User_ReviewerId",
                table: "AssignmentReviews",
                column: "ReviewerId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssignmentReviews_User_ReviewerId",
                table: "AssignmentReviews");

            migrationBuilder.DropIndex(
                name: "IX_AssignmentReviews_ReviewerId",
                table: "AssignmentReviews");

            migrationBuilder.DropColumn(
                name: "Grade",
                table: "AssignmentReviews");

            migrationBuilder.AddColumn<string>(
                name: "Score",
                table: "AssignmentReviews",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
