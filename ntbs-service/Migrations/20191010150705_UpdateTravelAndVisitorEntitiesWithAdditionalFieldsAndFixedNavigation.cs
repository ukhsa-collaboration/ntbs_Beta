using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class UpdateTravelAndVisitorEntitiesWithAdditionalFieldsAndFixedNavigation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TravelDetails_Country_Country1CountryId",
                table: "TravelDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_TravelDetails_Country_Country2CountryId",
                table: "TravelDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_TravelDetails_Country_Country3CountryId",
                table: "TravelDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_VisitorDetails_Country_Country1CountryId",
                table: "VisitorDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_VisitorDetails_Country_Country2CountryId",
                table: "VisitorDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_VisitorDetails_Country_Country3CountryId",
                table: "VisitorDetails");

            migrationBuilder.DropIndex(
                name: "IX_VisitorDetails_Country1CountryId",
                table: "VisitorDetails");

            migrationBuilder.DropIndex(
                name: "IX_VisitorDetails_Country2CountryId",
                table: "VisitorDetails");

            migrationBuilder.DropIndex(
                name: "IX_TravelDetails_Country1CountryId",
                table: "TravelDetails");

            migrationBuilder.DropIndex(
                name: "IX_TravelDetails_Country2CountryId",
                table: "TravelDetails");

            migrationBuilder.DropColumn(
                name: "Country1CountryId",
                table: "VisitorDetails");

            migrationBuilder.DropColumn(
                name: "Country2CountryId",
                table: "VisitorDetails");

            migrationBuilder.DropColumn(
                name: "Country1CountryId",
                table: "TravelDetails");

            migrationBuilder.DropColumn(
                name: "Country2CountryId",
                table: "TravelDetails");

            migrationBuilder.RenameColumn(
                name: "CountryId3",
                table: "VisitorDetails",
                newName: "TotalNumberOfCountries");

            migrationBuilder.RenameColumn(
                name: "CountryId2",
                table: "VisitorDetails",
                newName: "Country3Id");

            migrationBuilder.RenameColumn(
                name: "CountryId1",
                table: "VisitorDetails",
                newName: "Country2Id");

            migrationBuilder.RenameColumn(
                name: "Country3CountryId",
                table: "VisitorDetails",
                newName: "Country1Id");

            migrationBuilder.RenameIndex(
                name: "IX_VisitorDetails_Country3CountryId",
                table: "VisitorDetails",
                newName: "IX_VisitorDetails_Country1Id");

            migrationBuilder.RenameColumn(
                name: "CountryId3",
                table: "TravelDetails",
                newName: "TotalNumberOfCountries");

            migrationBuilder.RenameColumn(
                name: "CountryId2",
                table: "TravelDetails",
                newName: "Country3Id");

            migrationBuilder.RenameColumn(
                name: "CountryId1",
                table: "TravelDetails",
                newName: "Country2Id");

            migrationBuilder.RenameColumn(
                name: "Country3CountryId",
                table: "TravelDetails",
                newName: "Country1Id");

            migrationBuilder.RenameIndex(
                name: "IX_TravelDetails_Country3CountryId",
                table: "TravelDetails",
                newName: "IX_TravelDetails_Country1Id");

            migrationBuilder.CreateIndex(
                name: "IX_VisitorDetails_Country2Id",
                table: "VisitorDetails",
                column: "Country2Id");

            migrationBuilder.CreateIndex(
                name: "IX_VisitorDetails_Country3Id",
                table: "VisitorDetails",
                column: "Country3Id");

            migrationBuilder.CreateIndex(
                name: "IX_TravelDetails_Country2Id",
                table: "TravelDetails",
                column: "Country2Id");

            migrationBuilder.CreateIndex(
                name: "IX_TravelDetails_Country3Id",
                table: "TravelDetails",
                column: "Country3Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TravelDetails_Country_Country1Id",
                table: "TravelDetails",
                column: "Country1Id",
                principalTable: "Country",
                principalColumn: "CountryId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TravelDetails_Country_Country2Id",
                table: "TravelDetails",
                column: "Country2Id",
                principalTable: "Country",
                principalColumn: "CountryId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TravelDetails_Country_Country3Id",
                table: "TravelDetails",
                column: "Country3Id",
                principalTable: "Country",
                principalColumn: "CountryId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VisitorDetails_Country_Country1Id",
                table: "VisitorDetails",
                column: "Country1Id",
                principalTable: "Country",
                principalColumn: "CountryId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VisitorDetails_Country_Country2Id",
                table: "VisitorDetails",
                column: "Country2Id",
                principalTable: "Country",
                principalColumn: "CountryId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VisitorDetails_Country_Country3Id",
                table: "VisitorDetails",
                column: "Country3Id",
                principalTable: "Country",
                principalColumn: "CountryId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TravelDetails_Country_Country1Id",
                table: "TravelDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_TravelDetails_Country_Country2Id",
                table: "TravelDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_TravelDetails_Country_Country3Id",
                table: "TravelDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_VisitorDetails_Country_Country1Id",
                table: "VisitorDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_VisitorDetails_Country_Country2Id",
                table: "VisitorDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_VisitorDetails_Country_Country3Id",
                table: "VisitorDetails");

            migrationBuilder.DropIndex(
                name: "IX_VisitorDetails_Country2Id",
                table: "VisitorDetails");

            migrationBuilder.DropIndex(
                name: "IX_VisitorDetails_Country3Id",
                table: "VisitorDetails");

            migrationBuilder.DropIndex(
                name: "IX_TravelDetails_Country2Id",
                table: "TravelDetails");

            migrationBuilder.DropIndex(
                name: "IX_TravelDetails_Country3Id",
                table: "TravelDetails");

            migrationBuilder.RenameColumn(
                name: "TotalNumberOfCountries",
                table: "VisitorDetails",
                newName: "CountryId3");

            migrationBuilder.RenameColumn(
                name: "Country3Id",
                table: "VisitorDetails",
                newName: "CountryId2");

            migrationBuilder.RenameColumn(
                name: "Country2Id",
                table: "VisitorDetails",
                newName: "CountryId1");

            migrationBuilder.RenameColumn(
                name: "Country1Id",
                table: "VisitorDetails",
                newName: "Country3CountryId");

            migrationBuilder.RenameIndex(
                name: "IX_VisitorDetails_Country1Id",
                table: "VisitorDetails",
                newName: "IX_VisitorDetails_Country3CountryId");

            migrationBuilder.RenameColumn(
                name: "TotalNumberOfCountries",
                table: "TravelDetails",
                newName: "CountryId3");

            migrationBuilder.RenameColumn(
                name: "Country3Id",
                table: "TravelDetails",
                newName: "CountryId2");

            migrationBuilder.RenameColumn(
                name: "Country2Id",
                table: "TravelDetails",
                newName: "CountryId1");

            migrationBuilder.RenameColumn(
                name: "Country1Id",
                table: "TravelDetails",
                newName: "Country3CountryId");

            migrationBuilder.RenameIndex(
                name: "IX_TravelDetails_Country1Id",
                table: "TravelDetails",
                newName: "IX_TravelDetails_Country3CountryId");

            migrationBuilder.AddColumn<int>(
                name: "Country1CountryId",
                table: "VisitorDetails",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Country2CountryId",
                table: "VisitorDetails",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Country1CountryId",
                table: "TravelDetails",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Country2CountryId",
                table: "TravelDetails",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_VisitorDetails_Country1CountryId",
                table: "VisitorDetails",
                column: "Country1CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_VisitorDetails_Country2CountryId",
                table: "VisitorDetails",
                column: "Country2CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_TravelDetails_Country1CountryId",
                table: "TravelDetails",
                column: "Country1CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_TravelDetails_Country2CountryId",
                table: "TravelDetails",
                column: "Country2CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_TravelDetails_Country_Country1CountryId",
                table: "TravelDetails",
                column: "Country1CountryId",
                principalTable: "Country",
                principalColumn: "CountryId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TravelDetails_Country_Country2CountryId",
                table: "TravelDetails",
                column: "Country2CountryId",
                principalTable: "Country",
                principalColumn: "CountryId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TravelDetails_Country_Country3CountryId",
                table: "TravelDetails",
                column: "Country3CountryId",
                principalTable: "Country",
                principalColumn: "CountryId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VisitorDetails_Country_Country1CountryId",
                table: "VisitorDetails",
                column: "Country1CountryId",
                principalTable: "Country",
                principalColumn: "CountryId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VisitorDetails_Country_Country2CountryId",
                table: "VisitorDetails",
                column: "Country2CountryId",
                principalTable: "Country",
                principalColumn: "CountryId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VisitorDetails_Country_Country3CountryId",
                table: "VisitorDetails",
                column: "Country3CountryId",
                principalTable: "Country",
                principalColumn: "CountryId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
