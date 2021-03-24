using Microsoft.EntityFrameworkCore.Migrations;

namespace Halwani.Data.Migrations
{
    public partial class v00team : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
