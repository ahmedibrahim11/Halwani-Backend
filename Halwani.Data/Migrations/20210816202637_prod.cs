using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Halwani.Data.Migrations
{
    public partial class prod : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Teams_TeamsId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "TeamPermissions");

            migrationBuilder.DropIndex(
                name: "IX_Users_TeamsId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TeamsId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ProductCategoryName",
                table: "SLAs");

            migrationBuilder.RenameColumn(
                name: "WorkingDays",
                table: "SLAs",
                newName: "RequestType");

            migrationBuilder.RenameColumn(
                name: "ServiceLine",
                table: "SLAs",
                newName: "OpenStatus");

            migrationBuilder.RenameColumn(
                name: "SLAName",
                table: "SLAs",
                newName: "CloseStatus");

            migrationBuilder.AddColumn<bool>(
                name: "SetTicketHigh",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "EsclationReason",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedDate",
                table: "SLmMeasurements",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<decimal>(
                name: "Goal",
                table: "ProductCategories",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsVisible",
                table: "ProductCategories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "UserTeams",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    TeamId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTeams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserTeams_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserTeams_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserTeams_TeamId",
                table: "UserTeams",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTeams_UserId",
                table: "UserTeams",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserTeams");

            migrationBuilder.DropColumn(
                name: "SetTicketHigh",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "EsclationReason",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "Goal",
                table: "ProductCategories");

            migrationBuilder.DropColumn(
                name: "IsVisible",
                table: "ProductCategories");

            migrationBuilder.RenameColumn(
                name: "RequestType",
                table: "SLAs",
                newName: "WorkingDays");

            migrationBuilder.RenameColumn(
                name: "OpenStatus",
                table: "SLAs",
                newName: "ServiceLine");

            migrationBuilder.RenameColumn(
                name: "CloseStatus",
                table: "SLAs",
                newName: "SLAName");

            migrationBuilder.AddColumn<long>(
                name: "TeamsId",
                table: "Users",
                type: "bigint",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedDate",
                table: "SLmMeasurements",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductCategoryName",
                table: "SLAs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TeamPermissions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AllowedTeams = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsAllTeams = table.Column<bool>(type: "bit", nullable: false),
                    RoleId = table.Column<long>(type: "bigint", nullable: false),
                    TeamId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamPermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeamPermissions_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeamPermissions_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_TeamsId",
                table: "Users",
                column: "TeamsId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamPermissions_RoleId",
                table: "TeamPermissions",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamPermissions_TeamId",
                table: "TeamPermissions",
                column: "TeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Teams_TeamsId",
                table: "Users",
                column: "TeamsId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
