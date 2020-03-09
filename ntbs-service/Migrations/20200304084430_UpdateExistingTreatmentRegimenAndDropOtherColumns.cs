using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class UpdateExistingTreatmentRegimenAndDropOtherColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"  
                UPDATE ClinicalDetails
                SET TreatmentRegimen = 'MdrTreatment'
                WHERE IsMDRTreatment = 1");           
            
            migrationBuilder.Sql(@"  
                UPDATE ClinicalDetails
                SET TreatmentRegimen = 'StandardTherapy'
                WHERE IsShortCourseTreatment = 1");
            
            migrationBuilder.DropColumn(
                name: "IsMDRTreatment",
                table: "ClinicalDetails");

            migrationBuilder.DropColumn(
                name: "IsShortCourseTreatment",
                table: "ClinicalDetails");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsMDRTreatment",
                table: "ClinicalDetails",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsShortCourseTreatment",
                table: "ClinicalDetails",
                nullable: true);

            migrationBuilder.Sql(@"
                UPDATE ClinicalDetails
                SET IsShortCourseTreatment = 1
                WHERE TreatmentRegimen = 'StandardTherapy'");

            migrationBuilder.Sql(@"
                UPDATE ClinicalDetails
                SET IsMDRTreatment = 1
                WHERE TreatmentRegimen = 'MdrTreatment'");
        }
    }
}
