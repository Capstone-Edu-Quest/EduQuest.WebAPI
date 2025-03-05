using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduQuest_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateDb1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feedback_Learner_LearnerId",
                table: "Feedback");

            migrationBuilder.DropForeignKey(
                name: "FK_Learner_Course_CourseId",
                table: "Learner");

            migrationBuilder.DropForeignKey(
                name: "FK_Learner_User_UserId",
                table: "Learner");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Learner",
                table: "Learner");

            migrationBuilder.DropColumn(
                name: "Gold",
                table: "Learner");

            migrationBuilder.DropColumn(
                name: "Xp",
                table: "Learner");

            migrationBuilder.RenameTable(
                name: "Learner",
                newName: "CourseLearner");

            migrationBuilder.RenameIndex(
                name: "IX_Learner_UserId",
                table: "CourseLearner",
                newName: "IX_CourseLearner_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Learner_DeletedAt",
                table: "CourseLearner",
                newName: "IX_CourseLearner_DeletedAt");

            migrationBuilder.RenameIndex(
                name: "IX_Learner_CourseId",
                table: "CourseLearner",
                newName: "IX_CourseLearner_CourseId");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastActive",
                table: "UserStatistic",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_CourseLearner",
                table: "CourseLearner",
                column: "Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Feedback_CourseLearner_LearnerId",
                table: "Feedback",
                column: "LearnerId",
                principalTable: "CourseLearner",
                principalColumn: "Id");
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

            migrationBuilder.DropForeignKey(
                name: "FK_Feedback_CourseLearner_LearnerId",
                table: "Feedback");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CourseLearner",
                table: "CourseLearner");

            migrationBuilder.DropColumn(
                name: "LastActive",
                table: "UserStatistic");

            migrationBuilder.RenameTable(
                name: "CourseLearner",
                newName: "Learner");

            migrationBuilder.RenameIndex(
                name: "IX_CourseLearner_UserId",
                table: "Learner",
                newName: "IX_Learner_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_CourseLearner_DeletedAt",
                table: "Learner",
                newName: "IX_Learner_DeletedAt");

            migrationBuilder.RenameIndex(
                name: "IX_CourseLearner_CourseId",
                table: "Learner",
                newName: "IX_Learner_CourseId");

            migrationBuilder.AddColumn<int>(
                name: "Gold",
                table: "Learner",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Xp",
                table: "Learner",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Learner",
                table: "Learner",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Feedback_Learner_LearnerId",
                table: "Feedback",
                column: "LearnerId",
                principalTable: "Learner",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Learner_Course_CourseId",
                table: "Learner",
                column: "CourseId",
                principalTable: "Course",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Learner_User_UserId",
                table: "Learner",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
