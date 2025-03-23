using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduQuest_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCourseLearner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_CourseLearner_CourseLearnerId",
                table: "User");

            migrationBuilder.DropTable(
                name: "CourseCourseLearner");

            migrationBuilder.DropIndex(
                name: "IX_User_CourseLearnerId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "CourseLearnerId",
                table: "User");

            migrationBuilder.CreateIndex(
                name: "IX_CourseLearner_CourseId",
                table: "CourseLearner",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseLearner_UserId",
                table: "CourseLearner",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseLearner_Course_CourseId",
                table: "CourseLearner",
                column: "CourseId",
                principalTable: "Course",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseLearner_User_UserId",
                table: "CourseLearner",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseLearner_Course_CourseId",
                table: "CourseLearner");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseLearner_User_UserId",
                table: "CourseLearner");

            migrationBuilder.DropIndex(
                name: "IX_CourseLearner_CourseId",
                table: "CourseLearner");

            migrationBuilder.DropIndex(
                name: "IX_CourseLearner_UserId",
                table: "CourseLearner");

            migrationBuilder.AddColumn<string>(
                name: "CourseLearnerId",
                table: "User",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CourseCourseLearner",
                columns: table => new
                {
                    CourseLearnersId = table.Column<string>(type: "text", nullable: false),
                    CoursesId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseCourseLearner", x => new { x.CourseLearnersId, x.CoursesId });
                    table.ForeignKey(
                        name: "FK_CourseCourseLearner_CourseLearner_CourseLearnersId",
                        column: x => x.CourseLearnersId,
                        principalTable: "CourseLearner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseCourseLearner_Course_CoursesId",
                        column: x => x.CoursesId,
                        principalTable: "Course",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_User_CourseLearnerId",
                table: "User",
                column: "CourseLearnerId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseCourseLearner_CoursesId",
                table: "CourseCourseLearner",
                column: "CoursesId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_CourseLearner_CourseLearnerId",
                table: "User",
                column: "CourseLearnerId",
                principalTable: "CourseLearner",
                principalColumn: "Id");
        }
    }
}
