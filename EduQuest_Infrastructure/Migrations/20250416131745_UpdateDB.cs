using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduQuest_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			//migrationBuilder.DropTable(
			//	name: "FavoriteList");

			

            
           
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.CreateTable(
				name: "FavoriteList",
				columns: table => new
				{
					Id = table.Column<string>(type: "text", nullable: false),
					UserId = table.Column<string>(type: "text", nullable: false),
					CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
					UpdatedBy = table.Column<string>(type: "text", nullable: true),
					UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
					DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_FavoriteList", x => x.Id);
					table.ForeignKey(
						name: "FK_FavoriteList_User_UserId",
						column: x => x.UserId,
						principalTable: "User",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

		}
    }
}
