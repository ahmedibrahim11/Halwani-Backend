using Microsoft.EntityFrameworkCore.Migrations;

namespace Halwani.Data.Migrations
{
    public partial class v00final : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AssignedUser",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResolveText",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssignedUser",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "ResolveText",
                table: "Tickets");
        }
    }
}
