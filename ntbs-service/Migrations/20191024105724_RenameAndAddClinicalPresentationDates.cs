using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class RenameAndAddClinicalPresentationDates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PresentationDate",
                table: "ClinicalDetails",
                newName: "FirstPresentationDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "TBServicePresentationDate",
                table: "ClinicalDetails",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TBServicePresentationDate",
                table: "ClinicalDetails");

            migrationBuilder.RenameColumn(
                name: "FirstPresentationDate",
                table: "ClinicalDetails",
                newName: "PresentationDate");
        }
    }
}
