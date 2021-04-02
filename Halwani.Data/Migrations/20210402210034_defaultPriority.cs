using Microsoft.EntityFrameworkCore.Migrations;

namespace Halwani.Data.Migrations
{
    public partial class defaultPriority : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Priority",
                table: "RequestType",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Severity",
                table: "RequestType",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Priority",
                table: "RequestType");

            migrationBuilder.DropColumn(
                name: "Severity",
                table: "RequestType");
        }
    }
}
