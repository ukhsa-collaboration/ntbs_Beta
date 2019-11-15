using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class AddManualTestTypeAndSampleTypeReferenceData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ManualTestType",
                columns: new[] { "ManualTestTypeId", "Description" },
                values: new object[,]
                {
                    { 1, "Smear" },
                    { 2, "Culture" },
                    { 3, "Histology" },
                    { 4, "Chest x-ray" },
                    { 5, "PCR" },
                    { 6, "Line probe assay" }
                });

            migrationBuilder.InsertData(
                table: "SampleType",
                columns: new[] { "SampleTypeId", "Category", "Description" },
                values: new object[,]
                {
                    { 21, "Non-Respiratory", "Urine" },
                    { 20, "Non-Respiratory", "Skin" },
                    { 19, "Non-Respiratory", "Pus" },
                    { 18, "Non-Respiratory", "Pleural fluid or biopsy" },
                    { 17, "Non-Respiratory", "Pleural" },
                    { 16, "Non-Respiratory", "Peritoneal fluid" },
                    { 15, "Non-Respiratory", "Lymph node" },
                    { 14, "Non-Respiratory", "Gynaecological" },
                    { 13, "Non-Respiratory", "Genitourinary" },
                    { 12, "Non-Respiratory", "Gastrointestinal" },
                    { 9, "Non-Respiratory", "CSF" },
                    { 10, "Non-Respiratory", "Faeces" },
                    { 22, "Non-Respiratory", "Other tissues" },
                    { 8, "Non-Respiratory", "CNS" },
                    { 7, "Non-Respiratory", "Bone and joint" },
                    { 6, "Non-Respiratory", "Blood" },
                    { 5, "Respiratory", "Sputum (spontaneous)" },
                    { 4, "Respiratory", "Sputum (induced)" },
                    { 3, "Respiratory", "Lung bronchial tree tissue" },
                    { 2, "Respiratory", "Bronchoscopy sample" },
                    { 1, "Respiratory", "Bronchial washings" },
                    { 11, "Non-Respiratory", "Gastric washings" },
                    { 23, "Non-Respiratory", "Not known" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ManualTestType",
                keyColumn: "ManualTestTypeId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ManualTestType",
                keyColumn: "ManualTestTypeId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ManualTestType",
                keyColumn: "ManualTestTypeId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ManualTestType",
                keyColumn: "ManualTestTypeId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ManualTestType",
                keyColumn: "ManualTestTypeId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "ManualTestType",
                keyColumn: "ManualTestTypeId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "SampleType",
                keyColumn: "SampleTypeId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "SampleType",
                keyColumn: "SampleTypeId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "SampleType",
                keyColumn: "SampleTypeId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "SampleType",
                keyColumn: "SampleTypeId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "SampleType",
                keyColumn: "SampleTypeId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "SampleType",
                keyColumn: "SampleTypeId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "SampleType",
                keyColumn: "SampleTypeId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "SampleType",
                keyColumn: "SampleTypeId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "SampleType",
                keyColumn: "SampleTypeId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "SampleType",
                keyColumn: "SampleTypeId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "SampleType",
                keyColumn: "SampleTypeId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "SampleType",
                keyColumn: "SampleTypeId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "SampleType",
                keyColumn: "SampleTypeId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "SampleType",
                keyColumn: "SampleTypeId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "SampleType",
                keyColumn: "SampleTypeId",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "SampleType",
                keyColumn: "SampleTypeId",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "SampleType",
                keyColumn: "SampleTypeId",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "SampleType",
                keyColumn: "SampleTypeId",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "SampleType",
                keyColumn: "SampleTypeId",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "SampleType",
                keyColumn: "SampleTypeId",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "SampleType",
                keyColumn: "SampleTypeId",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "SampleType",
                keyColumn: "SampleTypeId",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "SampleType",
                keyColumn: "SampleTypeId",
                keyValue: 23);
        }
    }
}
