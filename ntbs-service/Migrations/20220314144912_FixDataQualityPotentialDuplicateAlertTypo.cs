using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ntbs_service.Migrations
{
    public partial class FixDataQualityPotentialDuplicateAlertTypo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "UPDATE Alert SET AlertType='DataQualityPotentialDuplicate' WHERE AlertType='DataQualityPotientialDuplicate'");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "UPDATE Alert SET AlertType='DataQualityPotientialDuplicate' WHERE AlertType='DataQualityPotentialDuplicate'");
        }
    }
}
