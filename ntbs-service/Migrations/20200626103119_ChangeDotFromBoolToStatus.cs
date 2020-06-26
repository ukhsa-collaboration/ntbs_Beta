using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class ChangeDotFromBoolToStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.RenameColumn(
                name: "IsDotOffered",
                table: "ClinicalDetails",
                newName: "IsDotOfferedBool");
            migrationBuilder.AddColumn<string>(
                name: "IsDotOffered",
                table: "ClinicalDetails",
                maxLength: 30,
                nullable: true);
            migrationBuilder.Sql(
                @"UPDATE [ClinicalDetails]
                    SET [IsDotOffered] = CASE
                        WHEN [IsDotOfferedBool] = 1 THEN 'Yes'
                        WHEN [IsDotOfferedBool] = 0 THEN 'No'
                    END");
            migrationBuilder.DropColumn("IsDotOfferedBool", "ClinicalDetails");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsDotOffered",
                table: "ClinicalDetails",
                newName: "IsDotOfferedString");
            migrationBuilder.AddColumn<string>(
                name: "IsDotOffered",
                table: "ClinicalDetails",
                maxLength: 30,
                nullable: true);
            migrationBuilder.Sql(
                @"UPDATE [ClinicalDetails]
                    SET [IsDotOffered] = CASE
                        WHEN [IsDotOfferedString] = 'Yes' THEN 1
                        WHEN [IsDotOfferedString] = 'No' THEN 0
                    END");
            migrationBuilder.DropColumn("IsDotOfferedString", "ClinicalDetails");
        }
    }
}
