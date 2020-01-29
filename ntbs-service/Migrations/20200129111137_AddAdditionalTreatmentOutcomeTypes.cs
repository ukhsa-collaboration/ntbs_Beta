using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class AddAdditionalTreatmentOutcomeTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "TreatmentOutcome",
                keyColumn: "TreatmentOutcomeId",
                keyValue: 15,
                column: "TreatmentOutcomeSubType",
                value: "TransferredAbroad");

            migrationBuilder.InsertData(
                table: "TreatmentOutcome",
                columns: new[] { "TreatmentOutcomeId", "TreatmentOutcomeSubType", "TreatmentOutcomeType" },
                values: new object[,]
                {
                    { 16, "StillOnTreatment", "NotEvaluated" },
                    { 17, "Other", "NotEvaluated" },
                    { 18, "CulturePositive", "Failed" },
                    { 19, "AdditionalResistance", "Failed" },
                    { 20, "AdverseReaction", "Failed" },
                    { 21, "Other", "Failed" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TreatmentOutcome",
                keyColumn: "TreatmentOutcomeId",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "TreatmentOutcome",
                keyColumn: "TreatmentOutcomeId",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "TreatmentOutcome",
                keyColumn: "TreatmentOutcomeId",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "TreatmentOutcome",
                keyColumn: "TreatmentOutcomeId",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "TreatmentOutcome",
                keyColumn: "TreatmentOutcomeId",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "TreatmentOutcome",
                keyColumn: "TreatmentOutcomeId",
                keyValue: 21);

            migrationBuilder.UpdateData(
                table: "TreatmentOutcome",
                keyColumn: "TreatmentOutcomeId",
                keyValue: 15,
                column: "TreatmentOutcomeSubType",
                value: null);
        }
    }
}
