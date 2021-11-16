using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class AddNewMDRFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DiscussedAtMDRForum",
                table: "MDRDetails",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MDRExpectedTreatmentDurationInMonths",
                table: "ClinicalDetails",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiscussedAtMDRForum",
                table: "MDRDetails");

            migrationBuilder.DropColumn(
                name: "MDRExpectedTreatmentDurationInMonths",
                table: "ClinicalDetails");
        }
    }
}
