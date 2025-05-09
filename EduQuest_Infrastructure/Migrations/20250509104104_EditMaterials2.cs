using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduQuest_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EditMaterials2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Material_Assignment_AssignmentId",
                table: "Material");

            migrationBuilder.DropForeignKey(
                name: "FK_Material_Quiz_QuizId",
                table: "Material");

            migrationBuilder.DropIndex(
                name: "IX_Material_AssignmentId",
                table: "Material");

            migrationBuilder.DropIndex(
                name: "IX_Material_QuizId",
                table: "Material");

            migrationBuilder.DropColumn(
                name: "AssignmentId",
                table: "Material");

            migrationBuilder.DropColumn(
                name: "QuizId",
                table: "Material");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AssignmentId",
                table: "Material",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QuizId",
                table: "Material",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Material_AssignmentId",
                table: "Material",
                column: "AssignmentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Material_QuizId",
                table: "Material",
                column: "QuizId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Material_Assignment_AssignmentId",
                table: "Material",
                column: "AssignmentId",
                principalTable: "Assignment",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Material_Quiz_QuizId",
                table: "Material",
                column: "QuizId",
                principalTable: "Quiz",
                principalColumn: "Id");
        }
    }
}
