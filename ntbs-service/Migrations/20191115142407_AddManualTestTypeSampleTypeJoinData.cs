using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class AddManualTestTypeSampleTypeJoinData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ManualTestTypeSampleType",
                columns: new[] { "ManualTestTypeId", "SampleTypeId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 5, 16 },
                    { 5, 15 },
                    { 5, 14 },
                    { 5, 11 },
                    { 5, 10 },
                    { 5, 9 },
                    { 5, 7 },
                    { 5, 6 },
                    { 5, 5 },
                    { 5, 4 },
                    { 5, 3 },
                    { 5, 2 },
                    { 5, 1 },
                    { 3, 23 },
                    { 3, 22 },
                    { 3, 20 },
                    { 3, 17 },
                    { 5, 18 },
                    { 3, 15 },
                    { 5, 19 },
                    { 5, 22 },
                    { 6, 21 },
                    { 6, 19 },
                    { 6, 18 },
                    { 6, 16 },
                    { 6, 15 },
                    { 6, 14 },
                    { 6, 11 },
                    { 6, 10 },
                    { 6, 9 },
                    { 6, 7 },
                    { 6, 6 },
                    { 6, 5 },
                    { 6, 4 },
                    { 6, 3 },
                    { 6, 2 },
                    { 6, 1 },
                    { 5, 23 },
                    { 5, 21 },
                    { 6, 22 },
                    { 3, 13 },
                    { 3, 8 },
                    { 1, 23 },
                    { 1, 22 },
                    { 1, 21 },
                    { 1, 19 },
                    { 1, 18 },
                    { 1, 16 },
                    { 1, 15 },
                    { 1, 14 },
                    { 1, 11 },
                    { 1, 10 },
                    { 1, 9 },
                    { 1, 7 },
                    { 1, 6 },
                    { 1, 5 },
                    { 1, 4 },
                    { 1, 3 },
                    { 1, 2 },
                    { 2, 1 },
                    { 3, 12 },
                    { 2, 2 },
                    { 2, 4 },
                    { 3, 7 },
                    { 3, 3 },
                    { 3, 2 },
                    { 2, 23 },
                    { 2, 22 },
                    { 2, 21 },
                    { 2, 19 },
                    { 2, 18 },
                    { 2, 16 },
                    { 2, 15 },
                    { 2, 14 },
                    { 2, 11 },
                    { 2, 10 },
                    { 2, 9 },
                    { 2, 7 },
                    { 2, 6 },
                    { 2, 5 },
                    { 2, 3 },
                    { 6, 23 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ManualTestTypeSampleType",
                keyColumns: new[] { "ManualTestTypeId", "SampleTypeId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "ManualTestTypeSampleType",
                keyColumns: new[] { "ManualTestTypeId", "SampleTypeId" },
                keyValues: new object[] { 1, 2 });

            migrationBuilder.DeleteData(
                table: "ManualTestTypeSampleType",
                keyColumns: new[] { "ManualTestTypeId", "SampleTypeId" },
                keyValues: new object[] { 1, 3 });

            migrationBuilder.DeleteData(
                table: "ManualTestTypeSampleType",
                keyColumns: new[] { "ManualTestTypeId", "SampleTypeId" },
                keyValues: new object[] { 1, 4 });

            migrationBuilder.DeleteData(
                table: "ManualTestTypeSampleType",
                keyColumns: new[] { "ManualTestTypeId", "SampleTypeId" },
                keyValues: new object[] { 1, 5 });

            migrationBuilder.DeleteData(
                table: "ManualTestTypeSampleType",
                keyColumns: new[] { "ManualTestTypeId", "SampleTypeId" },
                keyValues: new object[] { 1, 6 });

            migrationBuilder.DeleteData(
                table: "ManualTestTypeSampleType",
                keyColumns: new[] { "ManualTestTypeId", "SampleTypeId" },
                keyValues: new object[] { 1, 7 });

            migrationBuilder.DeleteData(
                table: "ManualTestTypeSampleType",
                keyColumns: new[] { "ManualTestTypeId", "SampleTypeId" },
                keyValues: new object[] { 1, 9 });

            migrationBuilder.DeleteData(
                table: "ManualTestTypeSampleType",
                keyColumns: new[] { "ManualTestTypeId", "SampleTypeId" },
                keyValues: new object[] { 1, 10 });

            migrationBuilder.DeleteData(
                table: "ManualTestTypeSampleType",
                keyColumns: new[] { "ManualTestTypeId", "SampleTypeId" },
                keyValues: new object[] { 1, 11 });

            migrationBuilder.DeleteData(
                table: "ManualTestTypeSampleType",
                keyColumns: new[] { "ManualTestTypeId", "SampleTypeId" },
                keyValues: new object[] { 1, 14 });

            migrationBuilder.DeleteData(
                table: "ManualTestTypeSampleType",
                keyColumns: new[] { "ManualTestTypeId", "SampleTypeId" },
                keyValues: new object[] { 1, 15 });

            migrationBuilder.DeleteData(
                table: "ManualTestTypeSampleType",
                keyColumns: new[] { "ManualTestTypeId", "SampleTypeId" },
                keyValues: new object[] { 1, 16 });

            migrationBuilder.DeleteData(
                table: "ManualTestTypeSampleType",
                keyColumns: new[] { "ManualTestTypeId", "SampleTypeId" },
                keyValues: new object[] { 1, 18 });

            migrationBuilder.DeleteData(
                table: "ManualTestTypeSampleType",
                keyColumns: new[] { "ManualTestTypeId", "SampleTypeId" },
                keyValues: new object[] { 1, 19 });

            migrationBuilder.DeleteData(
                table: "ManualTestTypeSampleType",
                keyColumns: new[] { "ManualTestTypeId", "SampleTypeId" },
                keyValues: new object[] { 1, 21 });

            migrationBuilder.DeleteData(
                table: "ManualTestTypeSampleType",
                keyColumns: new[] { "ManualTestTypeId", "SampleTypeId" },
                keyValues: new object[] { 1, 22 });

            migrationBuilder.DeleteData(
                table: "ManualTestTypeSampleType",
                keyColumns: new[] { "ManualTestTypeId", "SampleTypeId" },
                keyValues: new object[] { 1, 23 });

            migrationBuilder.DeleteData(
                table: "ManualTestTypeSampleType",
                keyColumns: new[] { "ManualTestTypeId", "SampleTypeId" },
                keyValues: new object[] { 2, 1 });

            migrationBuilder.DeleteData(
                table: "ManualTestTypeSampleType",
                keyColumns: new[] { "ManualTestTypeId", "SampleTypeId" },
                keyValues: new object[] { 2, 2 });

            migrationBuilder.DeleteData(
                table: "ManualTestTypeSampleType",
                keyColumns: new[] { "ManualTestTypeId", "SampleTypeId" },
                keyValues: new object[] { 2, 3 });

            migrationBuilder.DeleteData(
                table: "ManualTestTypeSampleType",
                keyColumns: new[] { "ManualTestTypeId", "SampleTypeId" },
                keyValues: new object[] { 2, 4 });

            migrationBuilder.DeleteData(
                table: "ManualTestTypeSampleType",
                keyColumns: new[] { "ManualTestTypeId", "SampleTypeId" },
                keyValues: new object[] { 2, 5 });

            migrationBuilder.DeleteData(
                table: "ManualTestTypeSampleType",
                keyColumns: new[] { "ManualTestTypeId", "SampleTypeId" },
                keyValues: new object[] { 2, 6 });

            migrationBuilder.DeleteData(
                table: "ManualTestTypeSampleType",
                keyColumns: new[] { "ManualTestTypeId", "SampleTypeId" },
                keyValues: new object[] { 2, 7 });

            migrationBuilder.DeleteData(
                table: "ManualTestTypeSampleType",
                keyColumns: new[] { "ManualTestTypeId", "SampleTypeId" },
                keyValues: new object[] { 2, 9 });

            migrationBuilder.DeleteData(
                table: "ManualTestTypeSampleType",
                keyColumns: new[] { "ManualTestTypeId", "SampleTypeId" },
                keyValues: new object[] { 2, 10 });

            migrationBuilder.DeleteData(
                table: "ManualTestTypeSampleType",
                keyColumns: new[] { "ManualTestTypeId", "SampleTypeId" },
                keyValues: new object[] { 2, 11 });

            migrationBuilder.DeleteData(
                table: "ManualTestTypeSampleType",
                keyColumns: new[] { "ManualTestTypeId", "SampleTypeId" },
                keyValues: new object[] { 2, 14 });

            migrationBuilder.DeleteData(
                table: "ManualTestTypeSampleType",
                keyColumns: new[] { "ManualTestTypeId", "SampleTypeId" },
                keyValues: new object[] { 2, 15 });

            migrationBuilder.DeleteData(
                table: "ManualTestTypeSampleType",
                keyColumns: new[] { "ManualTestTypeId", "SampleTypeId" },
                keyValues: new object[] { 2, 16 });

            migrationBuilder.DeleteData(
                table: "ManualTestTypeSampleType",
                keyColumns: new[] { "ManualTestTypeId", "SampleTypeId" },
                keyValues: new object[] { 2, 18 });

            migrationBuilder.DeleteData(
                table: "ManualTestTypeSampleType",
                keyColumns: new[] { "ManualTestTypeId", "SampleTypeId" },
                keyValues: new object[] { 2, 19 });

            migrationBuilder.DeleteData(
                table: "ManualTestTypeSampleType",
                keyColumns: new[] { "ManualTestTypeId", "SampleTypeId" },
                keyValues: new object[] { 2, 21 });

            migrationBuilder.DeleteData(
                table: "ManualTestTypeSampleType",
                keyColumns: new[] { "ManualTestTypeId", "SampleTypeId" },
                keyValues: new object[] { 2, 22 });

            migrationBuilder.DeleteData(
                table: "ManualTestTypeSampleType",
                keyColumns: new[] { "ManualTestTypeId", "SampleTypeId" },
                keyValues: new object[] { 2, 23 });

            migrationBuilder.DeleteData(
                table: "ManualTestTypeSampleType",
                keyColumns: new[] { "ManualTestTypeId", "SampleTypeId" },
                keyValues: new object[] { 3, 2 });

            migrationBuilder.DeleteData(
                table: "ManualTestTypeSampleType",
                keyColumns: new[] { "ManualTestTypeId", "SampleTypeId" },
                keyValues: new object[] { 3, 3 });

            migrationBuilder.DeleteData(
                table: "ManualTestTypeSampleType",
                keyColumns: new[] { "ManualTestTypeId", "SampleTypeId" },
                keyValues: new object[] { 3, 7 });

            migrationBuilder.DeleteData(
                table: "ManualTestTypeSampleType",
                keyColumns: new[] { "ManualTestTypeId", "SampleTypeId" },
                keyValues: new object[] { 3, 8 });

            migrationBuilder.DeleteData(
                table: "ManualTestTypeSampleType",
                keyColumns: new[] { "ManualTestTypeId", "SampleTypeId" },
                keyValues: new object[] { 3, 12 });

            migrationBuilder.DeleteData(
                table: "ManualTestTypeSampleType",
                keyColumns: new[] { "ManualTestTypeId", "SampleTypeId" },
                keyValues: new object[] { 3, 13 });

            migrationBuilder.DeleteData(
                table: "ManualTestTypeSampleType",
                keyColumns: new[] { "ManualTestTypeId", "SampleTypeId" },
                keyValues: new object[] { 3, 15 });

            migrationBuilder.DeleteData(
                table: "ManualTestTypeSampleType",
                keyColumns: new[] { "ManualTestTypeId", "SampleTypeId" },
                keyValues: new object[] { 3, 17 });

            migrationBuilder.DeleteData(
                table: "ManualTestTypeSampleType",
                keyColumns: new[] { "ManualTestTypeId", "SampleTypeId" },
                keyValues: new object[] { 3, 20 });

            migrationBuilder.DeleteData(
                table: "ManualTestTypeSampleType",
                keyColumns: new[] { "ManualTestTypeId", "SampleTypeId" },
                keyValues: new object[] { 3, 22 });

            migrationBuilder.DeleteData(
                table: "ManualTestTypeSampleType",
                keyColumns: new[] { "ManualTestTypeId", "SampleTypeId" },
                keyValues: new object[] { 3, 23 });

            migrationBuilder.DeleteData(
                table: "ManualTestTypeSampleType",
                keyColumns: new[] { "ManualTestTypeId", "SampleTypeId" },
                keyValues: new object[] { 5, 1 });

            migrationBuilder.DeleteData(
                table: "ManualTestTypeSampleType",
                keyColumns: new[] { "ManualTestTypeId", "SampleTypeId" },
                keyValues: new object[] { 5, 2 });

            migrationBuilder.DeleteData(
                table: "ManualTestTypeSampleType",
                keyColumns: new[] { "ManualTestTypeId", "SampleTypeId" },
                keyValues: new object[] { 5, 3 });

            migrationBuilder.DeleteData(
                table: "ManualTestTypeSampleType",
                keyColumns: new[] { "ManualTestTypeId", "SampleTypeId" },
                keyValues: new object[] { 5, 4 });

            migrationBuilder.DeleteData(
                table: "ManualTestTypeSampleType",
                keyColumns: new[] { "ManualTestTypeId", "SampleTypeId" },
                keyValues: new object[] { 5, 5 });

            migrationBuilder.DeleteData(
                table: "ManualTestTypeSampleType",
                keyColumns: new[] { "ManualTestTypeId", "SampleTypeId" },
                keyValues: new object[] { 5, 6 });

            migrationBuilder.DeleteData(
                table: "ManualTestTypeSampleType",
                keyColumns: new[] { "ManualTestTypeId", "SampleTypeId" },
                keyValues: new object[] { 5, 7 });

            migrationBuilder.DeleteData(
                table: "ManualTestTypeSampleType",
                keyColumns: new[] { "ManualTestTypeId", "SampleTypeId" },
                keyValues: new object[] { 5, 9 });

            migrationBuilder.DeleteData(
                table: "ManualTestTypeSampleType",
                keyColumns: new[] { "ManualTestTypeId", "SampleTypeId" },
                keyValues: new object[] { 5, 10 });

            migrationBuilder.DeleteData(
                table: "ManualTestTypeSampleType",
                keyColumns: new[] { "ManualTestTypeId", "SampleTypeId" },
                keyValues: new object[] { 5, 11 });

            migrationBuilder.DeleteData(
                table: "ManualTestTypeSampleType",
                keyColumns: new[] { "ManualTestTypeId", "SampleTypeId" },
                keyValues: new object[] { 5, 14 });

            migrationBuilder.DeleteData(
                table: "ManualTestTypeSampleType",
                keyColumns: new[] { "ManualTestTypeId", "SampleTypeId" },
                keyValues: new object[] { 5, 15 });

            migrationBuilder.DeleteData(
                table: "ManualTestTypeSampleType",
                keyColumns: new[] { "ManualTestTypeId", "SampleTypeId" },
                keyValues: new object[] { 5, 16 });

            migrationBuilder.DeleteData(
                table: "ManualTestTypeSampleType",
                keyColumns: new[] { "ManualTestTypeId", "SampleTypeId" },
                keyValues: new object[] { 5, 18 });

            migrationBuilder.DeleteData(
                table: "ManualTestTypeSampleType",
                keyColumns: new[] { "ManualTestTypeId", "SampleTypeId" },
                keyValues: new object[] { 5, 19 });

            migrationBuilder.DeleteData(
                table: "ManualTestTypeSampleType",
                keyColumns: new[] { "ManualTestTypeId", "SampleTypeId" },
                keyValues: new object[] { 5, 21 });

            migrationBuilder.DeleteData(
                table: "ManualTestTypeSampleType",
                keyColumns: new[] { "ManualTestTypeId", "SampleTypeId" },
                keyValues: new object[] { 5, 22 });

            migrationBuilder.DeleteData(
                table: "ManualTestTypeSampleType",
                keyColumns: new[] { "ManualTestTypeId", "SampleTypeId" },
                keyValues: new object[] { 5, 23 });

            migrationBuilder.DeleteData(
                table: "ManualTestTypeSampleType",
                keyColumns: new[] { "ManualTestTypeId", "SampleTypeId" },
                keyValues: new object[] { 6, 1 });

            migrationBuilder.DeleteData(
                table: "ManualTestTypeSampleType",
                keyColumns: new[] { "ManualTestTypeId", "SampleTypeId" },
                keyValues: new object[] { 6, 2 });

            migrationBuilder.DeleteData(
                table: "ManualTestTypeSampleType",
                keyColumns: new[] { "ManualTestTypeId", "SampleTypeId" },
                keyValues: new object[] { 6, 3 });

            migrationBuilder.DeleteData(
                table: "ManualTestTypeSampleType",
                keyColumns: new[] { "ManualTestTypeId", "SampleTypeId" },
                keyValues: new object[] { 6, 4 });

            migrationBuilder.DeleteData(
                table: "ManualTestTypeSampleType",
                keyColumns: new[] { "ManualTestTypeId", "SampleTypeId" },
                keyValues: new object[] { 6, 5 });

            migrationBuilder.DeleteData(
                table: "ManualTestTypeSampleType",
                keyColumns: new[] { "ManualTestTypeId", "SampleTypeId" },
                keyValues: new object[] { 6, 6 });

            migrationBuilder.DeleteData(
                table: "ManualTestTypeSampleType",
                keyColumns: new[] { "ManualTestTypeId", "SampleTypeId" },
                keyValues: new object[] { 6, 7 });

            migrationBuilder.DeleteData(
                table: "ManualTestTypeSampleType",
                keyColumns: new[] { "ManualTestTypeId", "SampleTypeId" },
                keyValues: new object[] { 6, 9 });

            migrationBuilder.DeleteData(
                table: "ManualTestTypeSampleType",
                keyColumns: new[] { "ManualTestTypeId", "SampleTypeId" },
                keyValues: new object[] { 6, 10 });

            migrationBuilder.DeleteData(
                table: "ManualTestTypeSampleType",
                keyColumns: new[] { "ManualTestTypeId", "SampleTypeId" },
                keyValues: new object[] { 6, 11 });

            migrationBuilder.DeleteData(
                table: "ManualTestTypeSampleType",
                keyColumns: new[] { "ManualTestTypeId", "SampleTypeId" },
                keyValues: new object[] { 6, 14 });

            migrationBuilder.DeleteData(
                table: "ManualTestTypeSampleType",
                keyColumns: new[] { "ManualTestTypeId", "SampleTypeId" },
                keyValues: new object[] { 6, 15 });

            migrationBuilder.DeleteData(
                table: "ManualTestTypeSampleType",
                keyColumns: new[] { "ManualTestTypeId", "SampleTypeId" },
                keyValues: new object[] { 6, 16 });

            migrationBuilder.DeleteData(
                table: "ManualTestTypeSampleType",
                keyColumns: new[] { "ManualTestTypeId", "SampleTypeId" },
                keyValues: new object[] { 6, 18 });

            migrationBuilder.DeleteData(
                table: "ManualTestTypeSampleType",
                keyColumns: new[] { "ManualTestTypeId", "SampleTypeId" },
                keyValues: new object[] { 6, 19 });

            migrationBuilder.DeleteData(
                table: "ManualTestTypeSampleType",
                keyColumns: new[] { "ManualTestTypeId", "SampleTypeId" },
                keyValues: new object[] { 6, 21 });

            migrationBuilder.DeleteData(
                table: "ManualTestTypeSampleType",
                keyColumns: new[] { "ManualTestTypeId", "SampleTypeId" },
                keyValues: new object[] { 6, 22 });

            migrationBuilder.DeleteData(
                table: "ManualTestTypeSampleType",
                keyColumns: new[] { "ManualTestTypeId", "SampleTypeId" },
                keyValues: new object[] { 6, 23 });
        }
    }
}
