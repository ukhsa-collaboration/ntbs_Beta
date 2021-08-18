using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class ChangeDidNotStartTreatmentToStartedTreatment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DidNotStartTreatment",
                table: "ClinicalDetails",
                newName: "StartedTreatment");

            migrationBuilder.Sql(
                @"UPDATE [ClinicalDetails] SET StartedTreatment = ~StartedTreatment");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StartedTreatment",
                table: "ClinicalDetails",
                newName: "DidNotStartTreatment");

            migrationBuilder.Sql(
                @"UPDATE [ClinicalDetails] SET DidNotStartTreatment = ~DidNotStartTreatment");
        }
    }
}
