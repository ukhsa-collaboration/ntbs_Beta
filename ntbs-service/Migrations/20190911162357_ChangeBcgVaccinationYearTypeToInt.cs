using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class ChangeBcgVaccinationYearTypeToInt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "BCGVaccinationYear",
                table: "ClinicalDetails",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "BCGVaccinationYear",
                table: "ClinicalDetails",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
