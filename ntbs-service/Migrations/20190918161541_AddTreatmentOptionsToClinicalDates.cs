using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class AddTreatmentOptionsToClinicalDates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsMDRTreatment",
                table: "ClinicalDetails",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsShortCourseTreatment",
                table: "ClinicalDetails",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "MDRTreatmentStartDate",
                table: "ClinicalDetails",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsMDRTreatment",
                table: "ClinicalDetails");

            migrationBuilder.DropColumn(
                name: "IsShortCourseTreatment",
                table: "ClinicalDetails");

            migrationBuilder.DropColumn(
                name: "MDRTreatmentStartDate",
                table: "ClinicalDetails");
        }
    }
}
