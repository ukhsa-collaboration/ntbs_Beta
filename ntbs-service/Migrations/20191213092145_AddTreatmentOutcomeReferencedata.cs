using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class AddTreatmentOutcomeReferencedata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "TreatmentOutcome",
                columns: new[] { "TreatmentOutcomeId", "TreatmentOutcomeSubType", "TreatmentOutcomeType" },
                values: new object[,]
                {
                    { 1, "StandardTherapy", "Completed" },
                    { 2, "MdrRegimen", "Completed" },
                    { 3, "Other", "Completed" },
                    { 4, "StandardTherapy", "Cured" },
                    { 5, "MdrRegimen", "Cured" },
                    { 6, "Other", "Cured" },
                    { 7, "TbCausedDeath", "Died" },
                    { 8, "TbContributedToDeath", "Died" },
                    { 9, "TbIncidentalToDeath", "Died" },
                    { 10, "Unknown", "Died" },
                    { 11, "PatientLeftUk", "Lost" },
                    { 12, "PatientNotLeftUk", "Lost" },
                    { 13, "Other", "Lost" },
                    { 14, null, "TreatmentStopped" },
                    { 15, null, "NotEvaluated" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TreatmentOutcome",
                keyColumn: "TreatmentOutcomeId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "TreatmentOutcome",
                keyColumn: "TreatmentOutcomeId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "TreatmentOutcome",
                keyColumn: "TreatmentOutcomeId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "TreatmentOutcome",
                keyColumn: "TreatmentOutcomeId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "TreatmentOutcome",
                keyColumn: "TreatmentOutcomeId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "TreatmentOutcome",
                keyColumn: "TreatmentOutcomeId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "TreatmentOutcome",
                keyColumn: "TreatmentOutcomeId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "TreatmentOutcome",
                keyColumn: "TreatmentOutcomeId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "TreatmentOutcome",
                keyColumn: "TreatmentOutcomeId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "TreatmentOutcome",
                keyColumn: "TreatmentOutcomeId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "TreatmentOutcome",
                keyColumn: "TreatmentOutcomeId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "TreatmentOutcome",
                keyColumn: "TreatmentOutcomeId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "TreatmentOutcome",
                keyColumn: "TreatmentOutcomeId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "TreatmentOutcome",
                keyColumn: "TreatmentOutcomeId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "TreatmentOutcome",
                keyColumn: "TreatmentOutcomeId",
                keyValue: 15);
        }
    }
}
