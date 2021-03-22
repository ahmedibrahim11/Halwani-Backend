using Microsoft.EntityFrameworkCore.Migrations;

namespace Halwani.Data.Migrations
{
    public partial class RequestTypesAndGroups : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TicketType",
                table: "Tickets",
                newName: "RequestTypeId");

            migrationBuilder.CreateTable(
                name: "Group",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Group", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RequestType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TicketType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RequestTypeGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestTypeId = table.Column<int>(type: "int", nullable: false),
                    GroupId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestTypeGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequestTypeGroups_Group_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Group",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RequestTypeGroups_RequestType_RequestTypeId",
                        column: x => x.RequestTypeId,
                        principalTable: "RequestType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_RequestTypeId",
                table: "Tickets",
                column: "RequestTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestTypeGroups_GroupId",
                table: "RequestTypeGroups",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestTypeGroups_RequestTypeId",
                table: "RequestTypeGroups",
                column: "RequestTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_RequestType_RequestTypeId",
                table: "Tickets",
                column: "RequestTypeId",
                principalTable: "RequestType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_RequestType_RequestTypeId",
                table: "Tickets");

            migrationBuilder.DropTable(
                name: "RequestTypeGroups");

            migrationBuilder.DropTable(
                name: "Group");

            migrationBuilder.DropTable(
                name: "RequestType");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_RequestTypeId",
                table: "Tickets");

            migrationBuilder.RenameColumn(
                name: "RequestTypeId",
                table: "Tickets",
                newName: "TicketType");
        }
    }
}
