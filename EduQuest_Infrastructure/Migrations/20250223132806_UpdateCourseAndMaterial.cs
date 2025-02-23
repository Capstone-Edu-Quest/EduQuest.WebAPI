using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EduQuest_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCourseAndMaterial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cart_Course_CourseId",
                table: "Cart");

            migrationBuilder.DropForeignKey(
                name: "FK_LearningMaterial_Stage_StageId",
                table: "LearningMaterial");

            migrationBuilder.DropForeignKey(
                name: "FK_Quiz_Stage_StageId",
                table: "Quiz");

            migrationBuilder.DropIndex(
                name: "IX_Quiz_StageId",
                table: "Quiz");

            migrationBuilder.DropIndex(
                name: "IX_LearningMaterial_StageId",
                table: "LearningMaterial");

            migrationBuilder.DropColumn(
                name: "QuizData",
                table: "Quiz");

            migrationBuilder.DropColumn(
                name: "StageId",
                table: "Quiz");

            migrationBuilder.DropColumn(
                name: "PaidDate",
                table: "Payment");

            migrationBuilder.AddColumn<string>(
                name: "PaymentIntentId",
                table: "Transaction",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Transaction",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AssignmentId",
                table: "Stage",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PassingPercentage",
                table: "Quiz",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "TimeLimit",
                table: "Quiz",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "AssignmentId",
                table: "LearningMaterial",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "LearningMaterial",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QuizId",
                table: "LearningMaterial",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Thumbnail",
                table: "LearningMaterial",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CourseId",
                table: "Cart",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Cart",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Assignment",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    TimeLimit = table.Column<int>(type: "integer", nullable: true),
                    Question = table.Column<string>(type: "text", nullable: true),
                    AnswerLanguage = table.Column<string>(type: "text", nullable: true),
                    ExpectedAnswer = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assignment", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CartItem",
                columns: table => new
                {
                    CartItemId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CartId = table.Column<int>(type: "integer", nullable: true),
                    ProductId = table.Column<int>(type: "integer", nullable: true),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    CartId1 = table.Column<string>(type: "text", nullable: true),
                    CourseId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItem", x => x.CartItemId);
                    table.ForeignKey(
                        name: "FK_CartItem_Cart_CartId1",
                        column: x => x.CartId1,
                        principalTable: "Cart",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CartItem_Course_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Course",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LearningMaterialStage",
                columns: table => new
                {
                    LearningMaterialsId = table.Column<string>(type: "text", nullable: false),
                    StagesId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LearningMaterialStage", x => new { x.LearningMaterialsId, x.StagesId });
                    table.ForeignKey(
                        name: "FK_LearningMaterialStage_LearningMaterial_LearningMaterialsId",
                        column: x => x.LearningMaterialsId,
                        principalTable: "LearningMaterial",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LearningMaterialStage_Stage_StagesId",
                        column: x => x.StagesId,
                        principalTable: "Stage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Stage_AssignmentId",
                table: "Stage",
                column: "AssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_LearningMaterial_AssignmentId",
                table: "LearningMaterial",
                column: "AssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_LearningMaterial_QuizId",
                table: "LearningMaterial",
                column: "QuizId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignment_DeletedAt",
                table: "Assignment",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_CartItem_CartId1",
                table: "CartItem",
                column: "CartId1");

            migrationBuilder.CreateIndex(
                name: "IX_CartItem_CourseId",
                table: "CartItem",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_LearningMaterialStage_StagesId",
                table: "LearningMaterialStage",
                column: "StagesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cart_Course_CourseId",
                table: "Cart",
                column: "CourseId",
                principalTable: "Course",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LearningMaterial_Assignment_AssignmentId",
                table: "LearningMaterial",
                column: "AssignmentId",
                principalTable: "Assignment",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LearningMaterial_Quiz_QuizId",
                table: "LearningMaterial",
                column: "QuizId",
                principalTable: "Quiz",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Stage_Assignment_AssignmentId",
                table: "Stage",
                column: "AssignmentId",
                principalTable: "Assignment",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cart_Course_CourseId",
                table: "Cart");

            migrationBuilder.DropForeignKey(
                name: "FK_LearningMaterial_Assignment_AssignmentId",
                table: "LearningMaterial");

            migrationBuilder.DropForeignKey(
                name: "FK_LearningMaterial_Quiz_QuizId",
                table: "LearningMaterial");

            migrationBuilder.DropForeignKey(
                name: "FK_Stage_Assignment_AssignmentId",
                table: "Stage");

            migrationBuilder.DropTable(
                name: "Assignment");

            migrationBuilder.DropTable(
                name: "CartItem");

            migrationBuilder.DropTable(
                name: "LearningMaterialStage");

            migrationBuilder.DropIndex(
                name: "IX_Stage_AssignmentId",
                table: "Stage");

            migrationBuilder.DropIndex(
                name: "IX_LearningMaterial_AssignmentId",
                table: "LearningMaterial");

            migrationBuilder.DropIndex(
                name: "IX_LearningMaterial_QuizId",
                table: "LearningMaterial");

            migrationBuilder.DropColumn(
                name: "PaymentIntentId",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "AssignmentId",
                table: "Stage");

            migrationBuilder.DropColumn(
                name: "PassingPercentage",
                table: "Quiz");

            migrationBuilder.DropColumn(
                name: "TimeLimit",
                table: "Quiz");

            migrationBuilder.DropColumn(
                name: "AssignmentId",
                table: "LearningMaterial");

            migrationBuilder.DropColumn(
                name: "Content",
                table: "LearningMaterial");

            migrationBuilder.DropColumn(
                name: "QuizId",
                table: "LearningMaterial");

            migrationBuilder.DropColumn(
                name: "Thumbnail",
                table: "LearningMaterial");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Cart");

            migrationBuilder.AddColumn<string>(
                name: "QuizData",
                table: "Quiz",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StageId",
                table: "Quiz",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "PaidDate",
                table: "Payment",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "CourseId",
                table: "Cart",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Quiz_StageId",
                table: "Quiz",
                column: "StageId");

            migrationBuilder.CreateIndex(
                name: "IX_LearningMaterial_StageId",
                table: "LearningMaterial",
                column: "StageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cart_Course_CourseId",
                table: "Cart",
                column: "CourseId",
                principalTable: "Course",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LearningMaterial_Stage_StageId",
                table: "LearningMaterial",
                column: "StageId",
                principalTable: "Stage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Quiz_Stage_StageId",
                table: "Quiz",
                column: "StageId",
                principalTable: "Stage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
