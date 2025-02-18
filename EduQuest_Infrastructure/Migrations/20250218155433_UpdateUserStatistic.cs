using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduQuest_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserStatistic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudyTimes_User_userId",
                table: "StudyTimes");

            migrationBuilder.RenameColumn(
                name: "TotalActiveDay",
                table: "UserStatistic",
                newName: "TotalStudyTime");

            migrationBuilder.RenameColumn(
                name: "StudyTime",
                table: "UserStatistic",
                newName: "TotalCompletedCourses");

            migrationBuilder.RenameColumn(
                name: "MaxStudyStreakDay",
                table: "UserStatistic",
                newName: "LongestStreak");

            migrationBuilder.RenameColumn(
                name: "CompletedCourses",
                table: "UserStatistic",
                newName: "CurrentStreak");

            migrationBuilder.AddForeignKey(
                name: "FK_StudyTimes_UserStatistic_userId",
                table: "StudyTimes",
                column: "userId",
                principalTable: "UserStatistic",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudyTimes_UserStatistic_userId",
                table: "StudyTimes");

            migrationBuilder.RenameColumn(
                name: "TotalStudyTime",
                table: "UserStatistic",
                newName: "TotalActiveDay");

            migrationBuilder.RenameColumn(
                name: "TotalCompletedCourses",
                table: "UserStatistic",
                newName: "StudyTime");

            migrationBuilder.RenameColumn(
                name: "LongestStreak",
                table: "UserStatistic",
                newName: "MaxStudyStreakDay");

            migrationBuilder.RenameColumn(
                name: "CurrentStreak",
                table: "UserStatistic",
                newName: "CompletedCourses");

            migrationBuilder.AddForeignKey(
                name: "FK_StudyTimes_User_userId",
                table: "StudyTimes",
                column: "userId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
