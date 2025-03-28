using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduQuest_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class newTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpPerLevel",
                table: "Levels");

            migrationBuilder.DropColumn(
                name: "LevelRequirement",
                table: "Levels");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Levels");

            migrationBuilder.DropColumn(
                name: "UpLevelReward",
                table: "Levels");

            migrationBuilder.AddColumn<int>(
                name: "Exp",
                table: "Levels",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LevelNumber",
                table: "Levels",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "LevelRewards",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    LevelId = table.Column<string>(type: "text", nullable: false),
                    RewardType = table.Column<int>(type: "integer", nullable: false),
                    RewardValue = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LevelRewards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LevelRewards_Levels_LevelId",
                        column: x => x.LevelId,
                        principalTable: "Levels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LevelRewards_DeletedAt",
                table: "LevelRewards",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_LevelRewards_LevelId",
                table: "LevelRewards",
                column: "LevelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LevelRewards");

            migrationBuilder.DropColumn(
                name: "Exp",
                table: "Levels");

            migrationBuilder.DropColumn(
                name: "LevelNumber",
                table: "Levels");

            migrationBuilder.AddColumn<string>(
                name: "ExpPerLevel",
                table: "Levels",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LevelRequirement",
                table: "Levels",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Levels",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UpLevelReward",
                table: "Levels",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
