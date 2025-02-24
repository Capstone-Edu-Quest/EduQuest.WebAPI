using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EduQuest_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateLearner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItem_Cart_CartId1",
                table: "CartItem");

            migrationBuilder.DropForeignKey(
                name: "FK_CartItem_Course_CourseId",
                table: "CartItem");

            migrationBuilder.DropForeignKey(
                name: "FK_StudyTimes_UserStatistic_userId",
                table: "StudyTimes");

            migrationBuilder.DropTable(
                name: "LearnerStatistic");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CartItem",
                table: "CartItem");

            migrationBuilder.DropIndex(
                name: "IX_CartItem_CartId1",
                table: "CartItem");

            migrationBuilder.DropColumn(
                name: "CartItemId",
                table: "CartItem");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "CartItem");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Cart");

            migrationBuilder.RenameColumn(
                name: "userId",
                table: "StudyTimes",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "studyTime",
                table: "StudyTimes",
                newName: "StudyTimes");

            migrationBuilder.RenameIndex(
                name: "IX_StudyTimes_userId",
                table: "StudyTimes",
                newName: "IX_StudyTimes_UserId");

            migrationBuilder.RenameColumn(
                name: "CartId1",
                table: "CartItem",
                newName: "UpdatedBy");

            migrationBuilder.AddColumn<double>(
                name: "TotalRevenue",
                table: "UserStatistic",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Gold",
                table: "Learner",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProgressPercentage",
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

            migrationBuilder.AddColumn<double>(
                name: "TotalRefund",
                table: "CourseStatistic",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "TotalRevenue",
                table: "CourseStatistic",
                type: "double precision",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CourseId",
                table: "CartItem",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CartId",
                table: "CartItem",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "CartItem",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "CartItem",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "CartItem",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "CartItem",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CartItem",
                table: "CartItem",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_CartItem_CartId",
                table: "CartItem",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItem_DeletedAt",
                table: "CartItem",
                column: "DeletedAt");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItem_Cart_CartId",
                table: "CartItem",
                column: "CartId",
                principalTable: "Cart",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CartItem_Course_CourseId",
                table: "CartItem",
                column: "CourseId",
                principalTable: "Course",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudyTimes_UserStatistic_UserId",
                table: "StudyTimes",
                column: "UserId",
                principalTable: "UserStatistic",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItem_Cart_CartId",
                table: "CartItem");

            migrationBuilder.DropForeignKey(
                name: "FK_CartItem_Course_CourseId",
                table: "CartItem");

            migrationBuilder.DropForeignKey(
                name: "FK_StudyTimes_UserStatistic_UserId",
                table: "StudyTimes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CartItem",
                table: "CartItem");

            migrationBuilder.DropIndex(
                name: "IX_CartItem_CartId",
                table: "CartItem");

            migrationBuilder.DropIndex(
                name: "IX_CartItem_DeletedAt",
                table: "CartItem");

            migrationBuilder.DropColumn(
                name: "TotalRevenue",
                table: "UserStatistic");

            migrationBuilder.DropColumn(
                name: "Gold",
                table: "Learner");

            migrationBuilder.DropColumn(
                name: "ProgressPercentage",
                table: "Learner");

            migrationBuilder.DropColumn(
                name: "Xp",
                table: "Learner");

            migrationBuilder.DropColumn(
                name: "TotalRefund",
                table: "CourseStatistic");

            migrationBuilder.DropColumn(
                name: "TotalRevenue",
                table: "CourseStatistic");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "CartItem");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "CartItem");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "CartItem");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "CartItem");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "StudyTimes",
                newName: "userId");

            migrationBuilder.RenameColumn(
                name: "StudyTimes",
                table: "StudyTimes",
                newName: "studyTime");

            migrationBuilder.RenameIndex(
                name: "IX_StudyTimes_UserId",
                table: "StudyTimes",
                newName: "IX_StudyTimes_userId");

            migrationBuilder.RenameColumn(
                name: "UpdatedBy",
                table: "CartItem",
                newName: "CartId1");

            migrationBuilder.AlterColumn<string>(
                name: "CourseId",
                table: "CartItem",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "CartId",
                table: "CartItem",
                type: "integer",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<int>(
                name: "CartItemId",
                table: "CartItem",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "CartItem",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Cart",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CartItem",
                table: "CartItem",
                column: "CartItemId");

            migrationBuilder.CreateTable(
                name: "LearnerStatistic",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    CourseId = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Gold = table.Column<int>(type: "integer", nullable: false),
                    ProgressPercentage = table.Column<int>(type: "integer", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    Xp = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LearnerStatistic", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LearnerStatistic_Course_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Course",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LearnerStatistic_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartItem_CartId1",
                table: "CartItem",
                column: "CartId1");

            migrationBuilder.CreateIndex(
                name: "IX_LearnerStatistic_CourseId",
                table: "LearnerStatistic",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_LearnerStatistic_DeletedAt",
                table: "LearnerStatistic",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_LearnerStatistic_UserId",
                table: "LearnerStatistic",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItem_Cart_CartId1",
                table: "CartItem",
                column: "CartId1",
                principalTable: "Cart",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItem_Course_CourseId",
                table: "CartItem",
                column: "CourseId",
                principalTable: "Course",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StudyTimes_UserStatistic_userId",
                table: "StudyTimes",
                column: "userId",
                principalTable: "UserStatistic",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
