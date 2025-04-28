using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduQuest_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Course_Advertise_AdvertiseId",
                table: "Course");

            migrationBuilder.DropForeignKey(
                name: "FK_Course_Setting_SettingId",
                table: "Course");

            migrationBuilder.DropTable(
                name: "Advertise");

            migrationBuilder.DropTable(
                name: "CourseItem");

            migrationBuilder.DropTable(
                name: "LearningHistory");

            migrationBuilder.DropTable(
                name: "SearchHistory");

            migrationBuilder.DropTable(
                name: "Setting");

            migrationBuilder.DropTable(
                name: "Item");

            migrationBuilder.DropIndex(
                name: "IX_Course_AdvertiseId",
                table: "Course");

            migrationBuilder.DropIndex(
                name: "IX_Course_SettingId",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "HeldAmount",
                table: "UserMeta");

            migrationBuilder.DropColumn(
                name: "PaidAmount",
                table: "UserMeta");

            migrationBuilder.DropColumn(
                name: "TotalRevenue",
                table: "UserMeta");

            migrationBuilder.DropColumn(
                name: "AdvertiseId",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "SettingId",
                table: "Course");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "HeldAmount",
                table: "UserMeta",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PaidAmount",
                table: "UserMeta",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "TotalRevenue",
                table: "UserMeta",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdvertiseId",
                table: "Course",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SettingId",
                table: "Course",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Advertise",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Clicks = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: false),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    RedirectUrl = table.Column<string>(type: "text", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<byte>(type: "smallint", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Advertise", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Item",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<int>(type: "integer", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Item", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LearningHistory",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    CourseId = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastAccessed = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LearningHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LearningHistory_Course_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Course",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LearningHistory_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SearchHistory",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Keyword = table.Column<string>(type: "text", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SearchHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SearchHistory_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Setting",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DifficultyLevel = table.Column<string>(type: "text", nullable: false),
                    MaxStudent = table.Column<int>(type: "integer", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Setting", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CourseItem",
                columns: table => new
                {
                    CoursesId = table.Column<string>(type: "text", nullable: false),
                    ItemsId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseItem", x => new { x.CoursesId, x.ItemsId });
                    table.ForeignKey(
                        name: "FK_CourseItem_Course_CoursesId",
                        column: x => x.CoursesId,
                        principalTable: "Course",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseItem_Item_ItemsId",
                        column: x => x.ItemsId,
                        principalTable: "Item",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Course_AdvertiseId",
                table: "Course",
                column: "AdvertiseId");

            migrationBuilder.CreateIndex(
                name: "IX_Course_SettingId",
                table: "Course",
                column: "SettingId");

            migrationBuilder.CreateIndex(
                name: "IX_Advertise_DeletedAt",
                table: "Advertise",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_CourseItem_ItemsId",
                table: "CourseItem",
                column: "ItemsId");

            migrationBuilder.CreateIndex(
                name: "IX_Item_DeletedAt",
                table: "Item",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_LearningHistory_CourseId",
                table: "LearningHistory",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_LearningHistory_DeletedAt",
                table: "LearningHistory",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_LearningHistory_UserId",
                table: "LearningHistory",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SearchHistory_DeletedAt",
                table: "SearchHistory",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_SearchHistory_UserId",
                table: "SearchHistory",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Setting_DeletedAt",
                table: "Setting",
                column: "DeletedAt");

            migrationBuilder.AddForeignKey(
                name: "FK_Course_Advertise_AdvertiseId",
                table: "Course",
                column: "AdvertiseId",
                principalTable: "Advertise",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Course_Setting_SettingId",
                table: "Course",
                column: "SettingId",
                principalTable: "Setting",
                principalColumn: "Id");
        }
    }
}
