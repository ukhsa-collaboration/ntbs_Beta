using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ntbs_service.Migrations
{
    public partial class AddTransferDateField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "TransferDate",
                table: "Alert",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransferDate",
                table: "Alert");
        }
    }
}
