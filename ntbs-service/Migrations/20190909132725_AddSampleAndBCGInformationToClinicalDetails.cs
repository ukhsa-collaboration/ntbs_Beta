using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class AddSampleAndBCGInformationToClinicalDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BCGVaccinationState",
                table: "ClinicalDetails",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BCGVaccinationYear",
                table: "ClinicalDetails",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "NoSampleTaken",
                table: "ClinicalDetails",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BCGVaccinationState",
                table: "ClinicalDetails");

            migrationBuilder.DropColumn(
                name: "BCGVaccinationYear",
                table: "ClinicalDetails");

            migrationBuilder.DropColumn(
                name: "NoSampleTaken",
                table: "ClinicalDetails");
        }
    }
}
