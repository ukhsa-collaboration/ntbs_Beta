using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class MergeRiskFactorsIntoSameModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "RiskFactorImprisonment",
                nullable: false,
                defaultValue: "Imprisonment");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "RiskFactorHomelessness",
                nullable: false,
                defaultValue: "Homelessness");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "RiskFactorDrugs",
                nullable: false,
                defaultValue: "Drugs");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "RiskFactorImprisonment");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "RiskFactorHomelessness");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "RiskFactorDrugs");
        }
    }
}
