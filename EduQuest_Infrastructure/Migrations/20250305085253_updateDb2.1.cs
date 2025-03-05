using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduQuest_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateDb21 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feedback_CourseLearner_CourseLearnerId",
                table: "Feedback");

            migrationBuilder.DropIndex(
                name: "IX_Feedback_CourseLearnerId",
                table: "Feedback");

            migrationBuilder.DropColumn(
                name: "CourseLearnerId",
                table: "Feedback");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CourseLearnerId",
                table: "Feedback",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Feedback_CourseLearnerId",
                table: "Feedback",
                column: "CourseLearnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Feedback_CourseLearner_CourseLearnerId",
                table: "Feedback",
                column: "CourseLearnerId",
                principalTable: "CourseLearner",
                principalColumn: "Id");
        }
    }
}
