using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduQuest_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tag_Assignment_AssignmentId",
                table: "Tag");

            migrationBuilder.DropForeignKey(
                name: "FK_Tag_Material_MaterialId",
                table: "Tag");

            migrationBuilder.DropForeignKey(
                name: "FK_Tag_Quiz_QuizId",
                table: "Tag");

            migrationBuilder.DropIndex(
                name: "IX_Tag_AssignmentId",
                table: "Tag");

            migrationBuilder.DropIndex(
                name: "IX_Tag_MaterialId",
                table: "Tag");

            migrationBuilder.DropIndex(
                name: "IX_Tag_QuizId",
                table: "Tag");

            migrationBuilder.DropColumn(
                name: "AssignmentId",
                table: "Tag");

            migrationBuilder.DropColumn(
                name: "MaterialId",
                table: "Tag");

            migrationBuilder.DropColumn(
                name: "QuizId",
                table: "Tag");

            migrationBuilder.CreateTable(
                name: "AssignmentTag",
                columns: table => new
                {
                    AssignmentsId = table.Column<string>(type: "text", nullable: false),
                    TagsId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssignmentTag", x => new { x.AssignmentsId, x.TagsId });
                    table.ForeignKey(
                        name: "FK_AssignmentTag_Assignment_AssignmentsId",
                        column: x => x.AssignmentsId,
                        principalTable: "Assignment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssignmentTag_Tag_TagsId",
                        column: x => x.TagsId,
                        principalTable: "Tag",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MaterialTag",
                columns: table => new
                {
                    MaterialsId = table.Column<string>(type: "text", nullable: false),
                    TagsId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialTag", x => new { x.MaterialsId, x.TagsId });
                    table.ForeignKey(
                        name: "FK_MaterialTag_Material_MaterialsId",
                        column: x => x.MaterialsId,
                        principalTable: "Material",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MaterialTag_Tag_TagsId",
                        column: x => x.TagsId,
                        principalTable: "Tag",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuizTag",
                columns: table => new
                {
                    QuizzesId = table.Column<string>(type: "text", nullable: false),
                    TagsId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizTag", x => new { x.QuizzesId, x.TagsId });
                    table.ForeignKey(
                        name: "FK_QuizTag_Quiz_QuizzesId",
                        column: x => x.QuizzesId,
                        principalTable: "Quiz",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuizTag_Tag_TagsId",
                        column: x => x.TagsId,
                        principalTable: "Tag",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentTag_TagsId",
                table: "AssignmentTag",
                column: "TagsId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialTag_TagsId",
                table: "MaterialTag",
                column: "TagsId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizTag_TagsId",
                table: "QuizTag",
                column: "TagsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssignmentTag");

            migrationBuilder.DropTable(
                name: "MaterialTag");

            migrationBuilder.DropTable(
                name: "QuizTag");

            migrationBuilder.AddColumn<string>(
                name: "AssignmentId",
                table: "Tag",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MaterialId",
                table: "Tag",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QuizId",
                table: "Tag",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tag_AssignmentId",
                table: "Tag",
                column: "AssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Tag_MaterialId",
                table: "Tag",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_Tag_QuizId",
                table: "Tag",
                column: "QuizId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tag_Assignment_AssignmentId",
                table: "Tag",
                column: "AssignmentId",
                principalTable: "Assignment",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tag_Material_MaterialId",
                table: "Tag",
                column: "MaterialId",
                principalTable: "Material",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tag_Quiz_QuizId",
                table: "Tag",
                column: "QuizId",
                principalTable: "Quiz",
                principalColumn: "Id");
        }
    }
}
