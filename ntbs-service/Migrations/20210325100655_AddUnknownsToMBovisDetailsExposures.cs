using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class AddUnknownsToMBovisDetailsExposures : Migration
    {
        private static readonly List<string> ExposureTypesToMigrate = new List<string>
        {
            "ExposureToKnownCases",
            "UnpasteurisedMilkConsumption",
            "OccupationExposure",
            "AnimalExposure",
        };

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            ExposureTypesToMigrate.ForEach(exposureType => UpColumn(migrationBuilder, exposureType));
        }

        private void UpColumn(MigrationBuilder migrationBuilder, string exposureType)
        {
            var oldColumnName = $"Has{exposureType}";
            var newColumnName = $"{exposureType}Status";

            migrationBuilder.AddColumn<string>(
                name: newColumnName,
                table: "MBovisDetails",
                type: "nvarchar(30)",
                nullable: true);

            migrationBuilder.Sql(
                @" UPDATE MBovisDetails " +
                $" SET {newColumnName} = CASE" +
                $"     WHEN {oldColumnName} = 0 THEN 'No'" +
                $"     WHEN {oldColumnName} = 1 THEN 'Yes'" +
                $"     ELSE NULL" +
                " END");

            migrationBuilder.DropColumn(oldColumnName, "MBovisDetails");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            ExposureTypesToMigrate.ForEach(exposureType => DownColumn(migrationBuilder, exposureType));
        }

        private void DownColumn(MigrationBuilder migrationBuilder, string exposureType)
        {
            var oldColumnName = $"{exposureType}Status";
            var newColumnName = $"Has{exposureType}";

            migrationBuilder.AddColumn<string>(
                name: newColumnName,
                table: "MBovisDetails",
                type: "bit",
                nullable: true);

            migrationBuilder.Sql(
                @" UPDATE MBovisDetails" +
                $" SET {newColumnName} = CASE" +
                $"     WHEN {oldColumnName} = 'No' THEN 0" +
                $"     WHEN {oldColumnName} = 'Yes' THEN 1" +
                $"     WHEN {oldColumnName} = 'Unknown' THEN 0" +
                $"     ELSE NULL" +
                " END");

            migrationBuilder.DropColumn(oldColumnName, "MBovisDetails");
        }
    }
}
