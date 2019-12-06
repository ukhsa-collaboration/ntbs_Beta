using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class MakeManualTestResultSampleTypeNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ManualTestResult_SampleType_SampleTypeId",
                table: "ManualTestResult");

            migrationBuilder.AlterColumn<int>(
                name: "SampleTypeId",
                table: "ManualTestResult",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_ManualTestResult_SampleType_SampleTypeId",
                table: "ManualTestResult",
                column: "SampleTypeId",
                principalTable: "SampleType",
                principalColumn: "SampleTypeId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ManualTestResult_SampleType_SampleTypeId",
                table: "ManualTestResult");

            migrationBuilder.AlterColumn<int>(
                name: "SampleTypeId",
                table: "ManualTestResult",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ManualTestResult_SampleType_SampleTypeId",
                table: "ManualTestResult",
                column: "SampleTypeId",
                principalTable: "SampleType",
                principalColumn: "SampleTypeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
