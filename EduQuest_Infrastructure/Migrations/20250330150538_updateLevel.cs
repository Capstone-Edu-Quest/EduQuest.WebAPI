using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduQuest_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateLevel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LevelRewards");

            migrationBuilder.AddColumn<string>(
                name: "RewardTypes",
                table: "Levels",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RewardValues",
                table: "Levels",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RewardTypes",
                table: "Levels");

            migrationBuilder.DropColumn(
                name: "RewardValues",
                table: "Levels");

            migrationBuilder.CreateTable(
                name: "LevelRewards",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    LevelId = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RewardType = table.Column<int>(type: "integer", nullable: true),
                    RewardValue = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LevelRewards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LevelRewards_Levels_LevelId",
                        column: x => x.LevelId,
                        principalTable: "Levels",
                        principalColumn: "Id");
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
    }
}
