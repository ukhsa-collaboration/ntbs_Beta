using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class AddAsylumSeekerAndImmigrationDetaineeStatuses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AsylumSeekerStatus",
                table: "SocialRiskFactors",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImmigrationDetaineeStatus",
                table: "SocialRiskFactors",
                maxLength: 30,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AsylumSeekerStatus",
                table: "SocialRiskFactors");

            migrationBuilder.DropColumn(
                name: "ImmigrationDetaineeStatus",
                table: "SocialRiskFactors");
        }
    }
}
