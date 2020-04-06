using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class moveReferenceDataToOwnSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "ReferenceData");

            migrationBuilder.RenameTable(
                name: "VenueType",
                newName: "VenueType",
                newSchema: "ReferenceData");

            migrationBuilder.RenameTable(
                name: "TreatmentOutcome",
                newName: "TreatmentOutcome",
                newSchema: "ReferenceData");

            migrationBuilder.RenameTable(
                name: "TbService",
                newName: "TbService",
                newSchema: "ReferenceData");

            migrationBuilder.RenameTable(
                name: "Site",
                newName: "Site",
                newSchema: "ReferenceData");

            migrationBuilder.RenameTable(
                name: "Sex",
                newName: "Sex",
                newSchema: "ReferenceData");

            migrationBuilder.RenameTable(
                name: "SampleType",
                newName: "SampleType",
                newSchema: "ReferenceData");

            migrationBuilder.RenameTable(
                name: "PostcodeLookup",
                newName: "PostcodeLookup",
                newSchema: "ReferenceData");

            migrationBuilder.RenameTable(
                name: "PHEC",
                newName: "PHEC",
                newSchema: "ReferenceData");

            migrationBuilder.RenameTable(
                name: "Occupation",
                newName: "Occupation",
                newSchema: "ReferenceData");

            migrationBuilder.RenameTable(
                name: "ManualTestTypeSampleType",
                newName: "ManualTestTypeSampleType",
                newSchema: "ReferenceData");

            migrationBuilder.RenameTable(
                name: "ManualTestType",
                newName: "ManualTestType",
                newSchema: "ReferenceData");

            migrationBuilder.RenameTable(
                name: "LocalAuthorityToPHEC",
                newName: "LocalAuthorityToPHEC",
                newSchema: "ReferenceData");

            migrationBuilder.RenameTable(
                name: "LocalAuthority",
                newName: "LocalAuthority",
                newSchema: "ReferenceData");

            migrationBuilder.RenameTable(
                name: "Hospital",
                newName: "Hospital",
                newSchema: "ReferenceData");

            migrationBuilder.RenameTable(
                name: "Ethnicity",
                newName: "Ethnicity",
                newSchema: "ReferenceData");

            migrationBuilder.RenameTable(
                name: "Country",
                newName: "Country",
                newSchema: "ReferenceData");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "VenueType",
                schema: "ReferenceData",
                newName: "VenueType");

            migrationBuilder.RenameTable(
                name: "TreatmentOutcome",
                schema: "ReferenceData",
                newName: "TreatmentOutcome");

            migrationBuilder.RenameTable(
                name: "TbService",
                schema: "ReferenceData",
                newName: "TbService");

            migrationBuilder.RenameTable(
                name: "Site",
                schema: "ReferenceData",
                newName: "Site");

            migrationBuilder.RenameTable(
                name: "Sex",
                schema: "ReferenceData",
                newName: "Sex");

            migrationBuilder.RenameTable(
                name: "SampleType",
                schema: "ReferenceData",
                newName: "SampleType");

            migrationBuilder.RenameTable(
                name: "PostcodeLookup",
                schema: "ReferenceData",
                newName: "PostcodeLookup");

            migrationBuilder.RenameTable(
                name: "PHEC",
                schema: "ReferenceData",
                newName: "PHEC");

            migrationBuilder.RenameTable(
                name: "Occupation",
                schema: "ReferenceData",
                newName: "Occupation");

            migrationBuilder.RenameTable(
                name: "ManualTestTypeSampleType",
                schema: "ReferenceData",
                newName: "ManualTestTypeSampleType");

            migrationBuilder.RenameTable(
                name: "ManualTestType",
                schema: "ReferenceData",
                newName: "ManualTestType");

            migrationBuilder.RenameTable(
                name: "LocalAuthorityToPHEC",
                schema: "ReferenceData",
                newName: "LocalAuthorityToPHEC");

            migrationBuilder.RenameTable(
                name: "LocalAuthority",
                schema: "ReferenceData",
                newName: "LocalAuthority");

            migrationBuilder.RenameTable(
                name: "Hospital",
                schema: "ReferenceData",
                newName: "Hospital");

            migrationBuilder.RenameTable(
                name: "Ethnicity",
                schema: "ReferenceData",
                newName: "Ethnicity");

            migrationBuilder.RenameTable(
                name: "Country",
                schema: "ReferenceData",
                newName: "Country");
        }
    }
}
