using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class RenamePatientDetailsFlags : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsPostcodeUnknown",
                table: "Patient",
                newName: "NoFixedAbode");

            migrationBuilder.RenameColumn(
                name: "IsNhsNumberUnknown",
                table: "Patient",
                newName: "NhsNumberNotKnown");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NoFixedAbode",
                table: "Patient",
                newName: "IsPostcodeUnknown");

            migrationBuilder.RenameColumn(
                name: "NhsNumberNotKnown",
                table: "Patient",
                newName: "IsNhsNumberUnknown");
        }
    }
}
