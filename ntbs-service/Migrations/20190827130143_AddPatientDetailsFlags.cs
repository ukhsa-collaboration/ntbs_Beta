using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class AddPatientDetailsFlags : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsNhsNumberUnknown",
                table: "Patient",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPostcodeUnknown",
                table: "Patient",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsNhsNumberUnknown",
                table: "Patient");

            migrationBuilder.DropColumn(
                name: "IsPostcodeUnknown",
                table: "Patient");
        }
    }
}
