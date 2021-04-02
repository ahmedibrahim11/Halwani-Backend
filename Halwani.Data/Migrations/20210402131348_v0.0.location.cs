using Microsoft.EntityFrameworkCore.Migrations;

namespace Halwani.Data.Migrations
{
    public partial class v00location : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ServiceName",
                table: "Tickets",
                newName: "TeamName");

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Location",
                table: "Tickets");

            migrationBuilder.RenameColumn(
                name: "TeamName",
                table: "Tickets",
                newName: "ServiceName");
        }
    }
}
