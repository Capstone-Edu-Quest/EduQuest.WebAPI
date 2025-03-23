using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduQuest_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLesson : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LessonMaterial_Lesson_StagesId",
                table: "LessonMaterial");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LessonMaterial",
                table: "LessonMaterial");

            migrationBuilder.DropIndex(
                name: "IX_LessonMaterial_StagesId",
                table: "LessonMaterial");

            migrationBuilder.RenameColumn(
                name: "StagesId",
                table: "LessonMaterial",
                newName: "LessonsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LessonMaterial",
                table: "LessonMaterial",
                columns: new[] { "LessonsId", "MaterialsId" });

            migrationBuilder.CreateIndex(
                name: "IX_LessonMaterial_MaterialsId",
                table: "LessonMaterial",
                column: "MaterialsId");

            migrationBuilder.AddForeignKey(
                name: "FK_LessonMaterial_Lesson_LessonsId",
                table: "LessonMaterial",
                column: "LessonsId",
                principalTable: "Lesson",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LessonMaterial_Lesson_LessonsId",
                table: "LessonMaterial");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LessonMaterial",
                table: "LessonMaterial");

            migrationBuilder.DropIndex(
                name: "IX_LessonMaterial_MaterialsId",
                table: "LessonMaterial");

            migrationBuilder.RenameColumn(
                name: "LessonsId",
                table: "LessonMaterial",
                newName: "StagesId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LessonMaterial",
                table: "LessonMaterial",
                columns: new[] { "MaterialsId", "StagesId" });

            migrationBuilder.CreateIndex(
                name: "IX_LessonMaterial_StagesId",
                table: "LessonMaterial",
                column: "StagesId");

            migrationBuilder.AddForeignKey(
                name: "FK_LessonMaterial_Lesson_StagesId",
                table: "LessonMaterial",
                column: "StagesId",
                principalTable: "Lesson",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
