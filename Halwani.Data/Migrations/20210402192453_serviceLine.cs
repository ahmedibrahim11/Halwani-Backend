using Microsoft.EntityFrameworkCore.Migrations;

namespace Halwani.Data.Migrations
{
    public partial class serviceLine : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Team",
                table: "SLAs",
                newName: "ServiceLine");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ServiceLine",
                table: "SLAs",
                newName: "Team");
        }
    }
}
