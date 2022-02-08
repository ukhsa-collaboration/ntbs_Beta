using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ntbs_service.Migrations
{
    public partial class AddTBServiceReferralDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "TBServiceReferralDate",
                table: "ClinicalDetails",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TBServiceReferralDate",
                table: "ClinicalDetails");
        }
    }
}
