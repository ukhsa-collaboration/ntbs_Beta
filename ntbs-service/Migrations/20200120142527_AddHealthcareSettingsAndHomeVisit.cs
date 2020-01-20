using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class AddHealthcareSettingsAndHomeVisit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FirstHomeVisitDate",
                table: "ClinicalDetails",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HealthcareDescription",
                table: "ClinicalDetails",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HealthcareSetting",
                table: "ClinicalDetails",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HomeVisitCarriedOut",
                table: "ClinicalDetails",
                maxLength: 30,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstHomeVisitDate",
                table: "ClinicalDetails");

            migrationBuilder.DropColumn(
                name: "HealthcareDescription",
                table: "ClinicalDetails");

            migrationBuilder.DropColumn(
                name: "HealthcareSetting",
                table: "ClinicalDetails");

            migrationBuilder.DropColumn(
                name: "HomeVisitCarriedOut",
                table: "ClinicalDetails");
        }
    }
}
