using Microsoft.EntityFrameworkCore.Migrations;

namespace Halwani.Data.Migrations
{
    public partial class teamNameInRequestType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestType_Teams_TeamId",
                table: "RequestType");

            migrationBuilder.DropIndex(
                name: "IX_RequestType_TeamId",
                table: "RequestType");

            migrationBuilder.DropColumn(
                name: "TeamId",
                table: "RequestType");

            migrationBuilder.AddColumn<long>(
                name: "DefaultTeamId",
                table: "RequestType",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TeamName",
                table: "RequestType",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RequestType_DefaultTeamId",
                table: "RequestType",
                column: "DefaultTeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestType_Teams_DefaultTeamId",
                table: "RequestType",
                column: "DefaultTeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestType_Teams_DefaultTeamId",
                table: "RequestType");

            migrationBuilder.DropIndex(
                name: "IX_RequestType_DefaultTeamId",
                table: "RequestType");

            migrationBuilder.DropColumn(
                name: "DefaultTeamId",
                table: "RequestType");

            migrationBuilder.DropColumn(
                name: "TeamName",
                table: "RequestType");

            migrationBuilder.AddColumn<long>(
                name: "TeamId",
                table: "RequestType",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_RequestType_TeamId",
                table: "RequestType",
                column: "TeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestType_Teams_TeamId",
                table: "RequestType",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
