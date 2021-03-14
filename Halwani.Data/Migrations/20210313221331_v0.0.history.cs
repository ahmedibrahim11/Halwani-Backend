using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Halwani.Data.Migrations
{
    public partial class v00history : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FromTeam",
                table: "TicketHistories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "TicketHistories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "NewStatus",
                table: "TicketHistories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OldStatus",
                table: "TicketHistories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "TicketId",
                table: "TicketHistories",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "ToTeam",
                table: "TicketHistories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TicketHistories_TicketId",
                table: "TicketHistories",
                column: "TicketId");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketHistories_Tickets_TicketId",
                table: "TicketHistories",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketHistories_Tickets_TicketId",
                table: "TicketHistories");

            migrationBuilder.DropIndex(
                name: "IX_TicketHistories_TicketId",
                table: "TicketHistories");

            migrationBuilder.DropColumn(
                name: "FromTeam",
                table: "TicketHistories");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "TicketHistories");

            migrationBuilder.DropColumn(
                name: "NewStatus",
                table: "TicketHistories");

            migrationBuilder.DropColumn(
                name: "OldStatus",
                table: "TicketHistories");

            migrationBuilder.DropColumn(
                name: "TicketId",
                table: "TicketHistories");

            migrationBuilder.DropColumn(
                name: "ToTeam",
                table: "TicketHistories");
        }
    }
}
