using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class BackfillHealthcareSettingValueChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                UPDATE ClinicalDetails 
                SET HealthcareSetting = 'FindAndTreat' 
                WHERE HealthcareSetting = 'FindAndTrace';");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                UPDATE ClinicalDetails 
                SET HealthcareSetting = 'FindAndTrace' 
                WHERE HealthcareSetting = 'FindAndTreat';");
        }
    }
}
