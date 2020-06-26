using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class ChangeTravelVisitorFromBoolToStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HasVisitor",
                table: "VisitorDetails",
                newName: "HasVisitorBool");
            migrationBuilder.AddColumn<string>(
                name: "HasVisitor",
                table: "VisitorDetails",
                maxLength: 30,
                nullable: true);
            migrationBuilder.Sql(
                @"UPDATE [VisitorDetails]
                    SET [HasVisitor] = CASE
                        WHEN [HasVisitorBool] = 1 THEN 'Yes'
                        WHEN [HasVisitorBool] = 0 THEN 'No'
                    END");
            migrationBuilder.DropColumn("HasVisitorBool", "VisitorDetails");
            
            migrationBuilder.RenameColumn(
                name: "HasTravel",
                table: "TravelDetails",
                newName: "HasTravelBool");
            migrationBuilder.AddColumn<string>(
                name: "HasTravel",
                table: "TravelDetails",
                maxLength: 30,
                nullable: true);
            migrationBuilder.Sql(
                @"UPDATE [TravelDetails]
                    SET [HasTravel] = CASE
                        WHEN [HasTravelBool] = 1 THEN 'Yes'
                        WHEN [HasTravelBool] = 0 THEN 'No'
                    END");
            migrationBuilder.DropColumn("HasTravelBool", "TravelDetails");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HasVisitor",
                table: "VisitorDetails",
                newName: "HasVisitorString");
            migrationBuilder.AddColumn<bool>(
                name: "HasVisitor",
                table: "VisitorDetails",
                nullable: true);
            migrationBuilder.Sql(
                @"UPDATE [VisitorDetails]
                    SET [HasVisitor] = CASE
                        WHEN [HasVisitorString] = 'Yes' THEN 1
                        WHEN [HasVisitorString] = 'No' THEN 0
                    END");
            migrationBuilder.DropColumn("HasVisitorString", "VisitorDetails");
            
            migrationBuilder.RenameColumn(
                name: "HasTravel",
                table: "TravelDetails",
                newName: "HasTravelString");
            migrationBuilder.AddColumn<bool>(
                name: "HasTravel",
                table: "TravelDetails",
                nullable: true);
            migrationBuilder.Sql(
                @"UPDATE [TravelDetails]
                    SET [HasTravel] = CASE
                        WHEN [HasTravelString] = 'Yes' THEN 1
                        WHEN [HasTravelString] = 'No' THEN 0
                    END");
            migrationBuilder.DropColumn("HasTravelString", "TravelDetails");
        }
    }
}
