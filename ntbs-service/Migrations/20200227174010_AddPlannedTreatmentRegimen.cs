using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class AddPlannedTreatmentRegimen : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TreatmentRegimen",
                table: "ClinicalDetails",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TreatmentRegimenOtherDescription",
                table: "ClinicalDetails",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TreatmentRegimen",
                table: "ClinicalDetails");

            migrationBuilder.DropColumn(
                name: "TreatmentRegimenOtherDescription",
                table: "ClinicalDetails");
        }
    }
}
