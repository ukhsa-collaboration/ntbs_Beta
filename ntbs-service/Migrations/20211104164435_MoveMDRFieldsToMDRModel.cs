using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class MoveMDRFieldsToMDRModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExpectedTreatmentDurationInMonths",
                table: "MDRDetails",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "MDRTreatmentStartDate",
                table: "MDRDetails",
                type: "datetime2",
                nullable: true);

            migrationBuilder.DropColumn(
                name: "MDRExpectedTreatmentDurationInMonths",
                table: "ClinicalDetails");

            migrationBuilder.DropColumn(
                name: "MDRTreatmentStartDate",
                table: "ClinicalDetails");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpectedTreatmentDurationInMonths",
                table: "MDRDetails");

            migrationBuilder.DropColumn(
                name: "MDRTreatmentStartDate",
                table: "MDRDetails");

            migrationBuilder.AddColumn<string>(
                name: "MDRExpectedTreatmentDurationInMonths",
                table: "ClinicalDetails",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "MDRTreatmentStartDate",
                table: "ClinicalDetails",
                type: "datetime2",
                nullable: true);
        }
    }
}
