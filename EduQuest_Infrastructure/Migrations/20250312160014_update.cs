using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduQuest_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudyTimes_UserStatistic_UserMetaId",
                table: "StudyTimes");

            migrationBuilder.DropForeignKey(
                name: "FK_UserStatistic_User_UserId",
                table: "UserStatistic");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserStatistic",
                table: "UserStatistic");

            migrationBuilder.RenameTable(
                name: "UserStatistic",
                newName: "UserMeta");

            migrationBuilder.RenameIndex(
                name: "IX_UserStatistic_UserId",
                table: "UserMeta",
                newName: "IX_UserMeta_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserStatistic_DeletedAt",
                table: "UserMeta",
                newName: "IX_UserMeta_DeletedAt");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserMeta",
                table: "UserMeta",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StudyTimes_UserMeta_UserMetaId",
                table: "StudyTimes",
                column: "UserMetaId",
                principalTable: "UserMeta",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserMeta_User_UserId",
                table: "UserMeta",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudyTimes_UserMeta_UserMetaId",
                table: "StudyTimes");

            migrationBuilder.DropForeignKey(
                name: "FK_UserMeta_User_UserId",
                table: "UserMeta");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserMeta",
                table: "UserMeta");

            migrationBuilder.RenameTable(
                name: "UserMeta",
                newName: "UserStatistic");

            migrationBuilder.RenameIndex(
                name: "IX_UserMeta_UserId",
                table: "UserStatistic",
                newName: "IX_UserStatistic_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserMeta_DeletedAt",
                table: "UserStatistic",
                newName: "IX_UserStatistic_DeletedAt");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserStatistic",
                table: "UserStatistic",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StudyTimes_UserStatistic_UserMetaId",
                table: "StudyTimes",
                column: "UserMetaId",
                principalTable: "UserStatistic",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserStatistic_User_UserId",
                table: "UserStatistic",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
