using Microsoft.EntityFrameworkCore.Migrations;

namespace Halwani.Data.Migrations
{
    public partial class TeamPermissionsUpdate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeamPermissions_Roles_RoleId1",
                table: "TeamPermissions");

            migrationBuilder.DropIndex(
                name: "IX_TeamPermissions_RoleId1",
                table: "TeamPermissions");

            migrationBuilder.DropColumn(
                name: "RoleId1",
                table: "TeamPermissions");

            migrationBuilder.AlterColumn<long>(
                name: "RoleId",
                table: "TeamPermissions",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_TeamPermissions_RoleId",
                table: "TeamPermissions",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_TeamPermissions_Roles_RoleId",
                table: "TeamPermissions",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeamPermissions_Roles_RoleId",
                table: "TeamPermissions");

            migrationBuilder.DropIndex(
                name: "IX_TeamPermissions_RoleId",
                table: "TeamPermissions");

            migrationBuilder.AlterColumn<int>(
                name: "RoleId",
                table: "TeamPermissions",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<long>(
                name: "RoleId1",
                table: "TeamPermissions",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TeamPermissions_RoleId1",
                table: "TeamPermissions",
                column: "RoleId1");

            migrationBuilder.AddForeignKey(
                name: "FK_TeamPermissions_Roles_RoleId1",
                table: "TeamPermissions",
                column: "RoleId1",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
