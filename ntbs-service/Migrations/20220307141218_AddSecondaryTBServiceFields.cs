using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ntbs_service.Migrations
{
    public partial class AddSecondaryTBServiceFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReasonForTBServiceShare",
                table: "HospitalDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SecondaryTBServiceCode",
                table: "HospitalDetails",
                type: "nvarchar(16)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HospitalDetails_SecondaryTBServiceCode",
                table: "HospitalDetails",
                column: "SecondaryTBServiceCode");

            migrationBuilder.AddForeignKey(
                name: "FK_HospitalDetails_TbService_SecondaryTBServiceCode",
                table: "HospitalDetails",
                column: "SecondaryTBServiceCode",
                principalSchema: "ReferenceData",
                principalTable: "TbService",
                principalColumn: "Code");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HospitalDetails_TbService_SecondaryTBServiceCode",
                table: "HospitalDetails");

            migrationBuilder.DropIndex(
                name: "IX_HospitalDetails_SecondaryTBServiceCode",
                table: "HospitalDetails");

            migrationBuilder.DropColumn(
                name: "ReasonForTBServiceShare",
                table: "HospitalDetails");

            migrationBuilder.DropColumn(
                name: "SecondaryTBServiceCode",
                table: "HospitalDetails");
        }
    }
}
