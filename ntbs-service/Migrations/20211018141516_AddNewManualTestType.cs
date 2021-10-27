using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class AddNewManualTestType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "ReferenceData",
                table: "ManualTestType",
                columns: new[] { "ManualTestTypeId", "Description" },
                values: new object[] { 7, "Chest CT" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "ReferenceData",
                table: "ManualTestType",
                keyColumn: "ManualTestTypeId",
                keyValue: 7);
        }
    }
}
