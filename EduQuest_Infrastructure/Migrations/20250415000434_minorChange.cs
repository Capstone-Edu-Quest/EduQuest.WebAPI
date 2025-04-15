using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduQuest_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class minorChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CertificateUser");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Certificate",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Certificate_UserId",
                table: "Certificate",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Certificate_User_UserId",
                table: "Certificate",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Certificate_User_UserId",
                table: "Certificate");

            migrationBuilder.DropIndex(
                name: "IX_Certificate_UserId",
                table: "Certificate");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Certificate");

            migrationBuilder.CreateTable(
                name: "CertificateUser",
                columns: table => new
                {
                    CertificatesId = table.Column<string>(type: "text", nullable: false),
                    UsersId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CertificateUser", x => new { x.CertificatesId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_CertificateUser_Certificate_CertificatesId",
                        column: x => x.CertificatesId,
                        principalTable: "Certificate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CertificateUser_User_UsersId",
                        column: x => x.UsersId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CertificateUser_UsersId",
                table: "CertificateUser",
                column: "UsersId");
        }
    }
}
