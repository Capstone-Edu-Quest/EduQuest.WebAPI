using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduQuest_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class QAAttempt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LessonId",
                table: "QuizAttempt",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "Percentage",
                table: "QuizAttempt",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<DateTime>(
                name: "SubmitAt",
                table: "QuizAttempt",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TotalTime",
                table: "QuizAttempt",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AssignmentAttempts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    AssignmentId = table.Column<string>(type: "text", nullable: false),
                    LessonId = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    AttemptNo = table.Column<int>(type: "integer", nullable: false),
                    AnswerContent = table.Column<string>(type: "text", nullable: false),
                    ToTalTime = table.Column<double>(type: "double precision", nullable: false),
                    AnswerScore = table.Column<double>(type: "double precision", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssignmentAttempts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssignmentAttempts_Assignment_AssignmentId",
                        column: x => x.AssignmentId,
                        principalTable: "Assignment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssignmentAttempts_Lesson_LessonId",
                        column: x => x.LessonId,
                        principalTable: "Lesson",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssignmentAttempts_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserQuizAnswers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    QuestionId = table.Column<string>(type: "text", nullable: false),
                    AnswerId = table.Column<string>(type: "text", nullable: false),
                    IsCorrect = table.Column<bool>(type: "boolean", nullable: false),
                    QuizAttemptId = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserQuizAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserQuizAnswers_Answer_AnswerId",
                        column: x => x.AnswerId,
                        principalTable: "Answer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserQuizAnswers_Question_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Question",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserQuizAnswers_QuizAttempt_QuizAttemptId",
                        column: x => x.QuizAttemptId,
                        principalTable: "QuizAttempt",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AssignmentReviews",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    AssignmentAttemptId = table.Column<string>(type: "text", nullable: false),
                    ReviewerId = table.Column<string>(type: "text", nullable: false),
                    Score = table.Column<string>(type: "text", nullable: false),
                    Comment = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssignmentReviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssignmentReviews_AssignmentAttempts_AssignmentAttemptId",
                        column: x => x.AssignmentAttemptId,
                        principalTable: "AssignmentAttempts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentAttempts_AssignmentId",
                table: "AssignmentAttempts",
                column: "AssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentAttempts_DeletedAt",
                table: "AssignmentAttempts",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentAttempts_LessonId",
                table: "AssignmentAttempts",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentAttempts_UserId",
                table: "AssignmentAttempts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentReviews_AssignmentAttemptId",
                table: "AssignmentReviews",
                column: "AssignmentAttemptId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentReviews_DeletedAt",
                table: "AssignmentReviews",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_UserQuizAnswers_AnswerId",
                table: "UserQuizAnswers",
                column: "AnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_UserQuizAnswers_DeletedAt",
                table: "UserQuizAnswers",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_UserQuizAnswers_QuestionId",
                table: "UserQuizAnswers",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserQuizAnswers_QuizAttemptId",
                table: "UserQuizAnswers",
                column: "QuizAttemptId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssignmentReviews");

            migrationBuilder.DropTable(
                name: "UserQuizAnswers");

            migrationBuilder.DropTable(
                name: "AssignmentAttempts");

            migrationBuilder.DropColumn(
                name: "LessonId",
                table: "QuizAttempt");

            migrationBuilder.DropColumn(
                name: "Percentage",
                table: "QuizAttempt");

            migrationBuilder.DropColumn(
                name: "SubmitAt",
                table: "QuizAttempt");

            migrationBuilder.DropColumn(
                name: "TotalTime",
                table: "QuizAttempt");
        }
    }
}
