using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class AddReleaseVersion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReleaseVersion",
                columns: table => new
                {
                    Version = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReleaseVersion", x => x.Version);
                });
            migrationBuilder.InsertData(
                table: "ReleaseVersion",
                columns: new[] { "Version", "Date" },
                values: new object[] { "pre-release", DateTime.UtcNow });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReleaseVersion");
        }
    }
}
