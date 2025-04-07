using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduQuest_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddLessionMaterial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.CreateTable(
			name: "LessonMaterial",
			columns: table => new
			{
				// Cột Id là khóa chính được kế thừa từ BaseEntity
				Id = table.Column<string>(nullable: false),
				LessonId = table.Column<string>(nullable: false),
				MaterialId = table.Column<string>(nullable: false),
				Index = table.Column<int>(nullable: false),
				CreatedAt = table.Column<DateTime>(nullable: true),
				UpdatedBy = table.Column<string>(nullable: true),
				UpdatedAt = table.Column<DateTime>(nullable: true),
				DeletedAt = table.Column<DateTime>(nullable: true),
			},
			constraints: table =>
			{
				// Đặt khóa chính cho bảng là Id
				table.PrimaryKey("PK_LessonMaterial", x => x.Id);

				// Nếu cần, thêm các khóa ngoại
				// table.ForeignKey("FK_LessonMaterial_Lesson", x => x.LessonId, "Lesson", "Id");
				// table.ForeignKey("FK_LessonMaterial_Material", x => x.MaterialId, "Material", "Id");
			});
		}

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.DropTable(
			name: "LessonMaterial");
		}
    }
}
