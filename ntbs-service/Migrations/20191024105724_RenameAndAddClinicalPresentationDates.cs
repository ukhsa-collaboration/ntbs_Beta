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
                newName: "PresentationToAnyHealthServiceDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "PresentationToTBServiceDate",
                table: "ClinicalDetails",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PresentationToTBServiceDate",
                table: "ClinicalDetails");

            migrationBuilder.RenameColumn(
                name: "PresentationToAnyHealthServiceDate",
                table: "ClinicalDetails",
                newName: "PresentationDate");
        }
    }
}
