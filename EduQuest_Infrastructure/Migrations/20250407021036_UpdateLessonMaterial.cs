using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduQuest_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLessonMaterial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.DropForeignKey(name: "FK_LessonMaterial_Lesson_LessonsId", table: "LessonMaterial");
			migrationBuilder.DropForeignKey(name: "FK_LessonMaterial_Material_MaterialsId", table: "LessonMaterial");
			migrationBuilder.DropPrimaryKey(name: "PK_LessonMaterial", table: "LessonMaterial");

			// Đổi tên các cột để chuẩn hóa tên
			migrationBuilder.RenameColumn(name: "MaterialsId", table: "LessonMaterial", newName: "MaterialId");
			migrationBuilder.RenameColumn(name: "LessonsId", table: "LessonMaterial", newName: "LessonId");

			// Đổi tên chỉ mục
			migrationBuilder.RenameIndex(name: "IX_LessonMaterial_MaterialsId", table: "LessonMaterial", newName: "IX_LessonMaterial_MaterialId");

			// Thêm cột mới và khóa chính
			migrationBuilder.AddColumn<string>(name: "Id", table: "LessonMaterial", type: "text", nullable: false, defaultValue: "");
			migrationBuilder.AddPrimaryKey(name: "PK_LessonMaterial", table: "LessonMaterial", column: "Id");

			// Tạo chỉ mục duy nhất trên cặp LessonId và MaterialId
			migrationBuilder.CreateIndex(name: "IX_LessonMaterial_LessonId_MaterialId", table: "LessonMaterial", columns: new[] { "LessonId", "MaterialId" }, unique: true);

			// Thêm các khóa ngoại
			migrationBuilder.AddForeignKey(name: "FK_LessonMaterial_Lesson_LessonId", table: "LessonMaterial", column: "LessonId", principalTable: "Lesson", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
			migrationBuilder.AddForeignKey(name: "FK_LessonMaterial_Material_MaterialId", table: "LessonMaterial", column: "MaterialId", principalTable: "Material", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
		}

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.DropForeignKey(name: "FK_LessonMaterial_Lesson_LessonId", table: "LessonMaterial");
			migrationBuilder.DropForeignKey(name: "FK_LessonMaterial_Material_MaterialId", table: "LessonMaterial");
			migrationBuilder.DropPrimaryKey(name: "PK_LessonMaterial", table: "LessonMaterial");
			migrationBuilder.DropIndex(name: "IX_LessonMaterial_LessonId_MaterialId", table: "LessonMaterial");

			// Xóa các cột đã thêm
			migrationBuilder.DropColumn(name: "Id", table: "LessonMaterial");

			// Đổi tên lại các cột
			migrationBuilder.RenameColumn(name: "MaterialId", table: "LessonMaterial", newName: "MaterialsId");
			migrationBuilder.RenameColumn(name: "LessonId", table: "LessonMaterial", newName: "LessonsId");

			// Thêm lại khóa chính cũ (trên cặp LessonId và MaterialId)
			migrationBuilder.AddPrimaryKey(name: "PK_LessonMaterial", table: "LessonMaterial", columns: new[] { "LessonsId", "MaterialsId" });

			// Thêm lại các khóa ngoại cũ
			migrationBuilder.AddForeignKey(name: "FK_LessonMaterial_Lesson_LessonsId", table: "LessonMaterial", column: "LessonsId", principalTable: "Lesson", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
			migrationBuilder.AddForeignKey(name: "FK_LessonMaterial_Material_MaterialsId", table: "LessonMaterial", column: "MaterialsId", principalTable: "Material", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
		}
    }
}
