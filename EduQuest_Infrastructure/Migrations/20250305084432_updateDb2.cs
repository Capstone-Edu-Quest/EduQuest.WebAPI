using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduQuest_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateDb2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feedback_CourseLearner_LearnerId",
                table: "Feedback");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Feedback",
                table: "Feedback");

            migrationBuilder.RenameColumn(
                name: "LearnerId",
                table: "Feedback",
                newName: "CourseLearnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Feedback_LearnerId",
                table: "Feedback",
                newName: "IX_Feedback_CourseLearnerId");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Feedback",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Feedback",
                table: "Feedback",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Report",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ClosedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Reporter = table.Column<string>(type: "text", nullable: false),
                    Violator = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: true),
                    Reason = table.Column<string>(type: "text", nullable: false),
                    FeedbackId = table.Column<string>(type: "text", nullable: true),
                    CourseId = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Report", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Report_Course_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Course",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Report_Feedback_FeedbackId",
                        column: x => x.FeedbackId,
                        principalTable: "Feedback",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Report_User_Reporter",
                        column: x => x.Reporter,
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Report_User_Violator",
                        column: x => x.Violator,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Feedback_UserId",
                table: "Feedback",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Report_CourseId",
                table: "Report",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Report_FeedbackId",
                table: "Report",
                column: "FeedbackId");

            migrationBuilder.CreateIndex(
                name: "IX_Report_Reporter",
                table: "Report",
                column: "Reporter");

            migrationBuilder.CreateIndex(
                name: "IX_Report_Violator",
                table: "Report",
                column: "Violator");

            migrationBuilder.AddForeignKey(
                name: "FK_Feedback_CourseLearner_CourseLearnerId",
                table: "Feedback",
                column: "CourseLearnerId",
                principalTable: "CourseLearner",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feedback_CourseLearner_CourseLearnerId",
                table: "Feedback");

            migrationBuilder.DropTable(
                name: "Report");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Feedback",
                table: "Feedback");

            migrationBuilder.DropIndex(
                name: "IX_Feedback_UserId",
                table: "Feedback");

            migrationBuilder.RenameColumn(
                name: "CourseLearnerId",
                table: "Feedback",
                newName: "LearnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Feedback_CourseLearnerId",
                table: "Feedback",
                newName: "IX_Feedback_LearnerId");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Feedback",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Feedback",
                table: "Feedback",
                columns: new[] { "UserId", "CourseId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Feedback_CourseLearner_LearnerId",
                table: "Feedback",
                column: "LearnerId",
                principalTable: "CourseLearner",
                principalColumn: "Id");
        }
    }
}
