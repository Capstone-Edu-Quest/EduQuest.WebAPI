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
            migrationBuilder.DropTable(
                name: "AccountPackagePackagePrivilege");

            migrationBuilder.AlterColumn<string>(
                name: "PackagePrivilegeId",
                table: "User",
                type: "nvarchar(450)",
                nullable: true,
                defaultValue: "",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AccountPackageId",
                table: "User",
                type: "nvarchar(450)",
                nullable: true,
                defaultValue: "",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_AccountPackageId",
                table: "User",
                column: "AccountPackageId");

            migrationBuilder.CreateIndex(
                name: "IX_User_PackagePrivilegeId",
                table: "User",
                column: "PackagePrivilegeId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_AccountPackage_AccountPackageId",
                table: "User",
                column: "AccountPackageId",
                principalTable: "AccountPackage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_User_PackagePrivilege_PackagePrivilegeId",
                table: "User",
                column: "PackagePrivilegeId",
                principalTable: "PackagePrivilege",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_AccountPackage_AccountPackageId",
                table: "User");

            migrationBuilder.DropForeignKey(
                name: "FK_User_PackagePrivilege_PackagePrivilegeId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_AccountPackageId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_PackagePrivilegeId",
                table: "User");

            migrationBuilder.AlterColumn<int>(
                name: "PackagePrivilegeId",
                table: "User",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "AccountPackageId",
                table: "User",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateTable(
                name: "AccountPackagePackagePrivilege",
                columns: table => new
                {
                    AccountPackagesId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PackagePrivilegesId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountPackagePackagePrivilege", x => new { x.AccountPackagesId, x.PackagePrivilegesId });
                    table.ForeignKey(
                        name: "FK_AccountPackagePackagePrivilege_AccountPackage_AccountPackagesId",
                        column: x => x.AccountPackagesId,
                        principalTable: "AccountPackage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccountPackagePackagePrivilege_PackagePrivilege_PackagePrivilegesId",
                        column: x => x.PackagePrivilegesId,
                        principalTable: "PackagePrivilege",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountPackagePackagePrivilege_PackagePrivilegesId",
                table: "AccountPackagePackagePrivilege",
                column: "PackagePrivilegesId");
        }
    }
}
