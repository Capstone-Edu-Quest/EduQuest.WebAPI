using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduQuest_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EditMaterials : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LessonMaterial_Material_MaterialId",
                table: "LessonMaterial");

            migrationBuilder.AlterColumn<string>(
                name: "MaterialId",
                table: "LessonMaterial",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "AssignmentId",
                table: "LessonMaterial",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QuizId",
                table: "LessonMaterial",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LessonMaterial_AssignmentId",
                table: "LessonMaterial",
                column: "AssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_LessonMaterial_QuizId",
                table: "LessonMaterial",
                column: "QuizId");

            migrationBuilder.AddForeignKey(
                name: "FK_LessonMaterial_Assignment_AssignmentId",
                table: "LessonMaterial",
                column: "AssignmentId",
                principalTable: "Assignment",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LessonMaterial_Material_MaterialId",
                table: "LessonMaterial",
                column: "MaterialId",
                principalTable: "Material",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LessonMaterial_Quiz_QuizId",
                table: "LessonMaterial",
                column: "QuizId",
                principalTable: "Quiz",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LessonMaterial_Assignment_AssignmentId",
                table: "LessonMaterial");

            migrationBuilder.DropForeignKey(
                name: "FK_LessonMaterial_Material_MaterialId",
                table: "LessonMaterial");

            migrationBuilder.DropForeignKey(
                name: "FK_LessonMaterial_Quiz_QuizId",
                table: "LessonMaterial");

            migrationBuilder.DropIndex(
                name: "IX_LessonMaterial_AssignmentId",
                table: "LessonMaterial");

            migrationBuilder.DropIndex(
                name: "IX_LessonMaterial_QuizId",
                table: "LessonMaterial");

            migrationBuilder.DropColumn(
                name: "AssignmentId",
                table: "LessonMaterial");

            migrationBuilder.DropColumn(
                name: "QuizId",
                table: "LessonMaterial");

            migrationBuilder.AlterColumn<string>(
                name: "MaterialId",
                table: "LessonMaterial",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_LessonMaterial_Material_MaterialId",
                table: "LessonMaterial",
                column: "MaterialId",
                principalTable: "Material",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
