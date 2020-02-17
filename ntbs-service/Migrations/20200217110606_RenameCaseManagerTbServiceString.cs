using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class RenameCaseManagerTbServiceString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CaseManagerTbServiceString",
                table: "Alert",
                newName: "DecliningUserAndTbServiceString");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DecliningUserAndTbServiceString",
                table: "Alert",
                newName: "CaseManagerTbServiceString");
        }
    }
}
