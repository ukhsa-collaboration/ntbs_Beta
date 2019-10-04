using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class MakeTreatmentFlagsNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsShortCourseTreatment",
                table: "ClinicalDetails",
                nullable: true,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<bool>(
                name: "IsMDRTreatment",
                table: "ClinicalDetails",
                nullable: true,
                oldClrType: typeof(bool));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsShortCourseTreatment",
                table: "ClinicalDetails",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsMDRTreatment",
                table: "ClinicalDetails",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);
        }
    }
}
