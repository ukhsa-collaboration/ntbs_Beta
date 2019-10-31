using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class RenameTbService : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Episode_TBService_TBServiceCode",
                table: "Episode");

            migrationBuilder.DropForeignKey(
                name: "FK_Hospital_TBService_TBServiceCode",
                table: "Hospital");

            migrationBuilder.DropForeignKey(
                name: "FK_TBService_PHEC_PHECCode",
                table: "TBService");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TBService",
                table: "TBService");

            migrationBuilder.RenameTable(
                name: "TBService",
                newName: "TbService");

            migrationBuilder.RenameIndex(
                name: "IX_TBService_ServiceAdGroup",
                table: "TbService",
                newName: "IX_TbService_ServiceAdGroup");

            migrationBuilder.RenameIndex(
                name: "IX_TBService_PHECCode",
                table: "TbService",
                newName: "IX_TbService_PHECCode");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TbService",
                table: "TbService",
                column: "Code");

            migrationBuilder.AddForeignKey(
                name: "FK_Episode_TbService_TBServiceCode",
                table: "Episode",
                column: "TBServiceCode",
                principalTable: "TbService",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Hospital_TbService_TBServiceCode",
                table: "Hospital",
                column: "TBServiceCode",
                principalTable: "TbService",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TbService_PHEC_PHECCode",
                table: "TbService",
                column: "PHECCode",
                principalTable: "PHEC",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Episode_TbService_TBServiceCode",
                table: "Episode");

            migrationBuilder.DropForeignKey(
                name: "FK_Hospital_TbService_TBServiceCode",
                table: "Hospital");

            migrationBuilder.DropForeignKey(
                name: "FK_TbService_PHEC_PHECCode",
                table: "TbService");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TbService",
                table: "TbService");

            migrationBuilder.RenameTable(
                name: "TbService",
                newName: "TBService");

            migrationBuilder.RenameIndex(
                name: "IX_TbService_ServiceAdGroup",
                table: "TBService",
                newName: "IX_TBService_ServiceAdGroup");

            migrationBuilder.RenameIndex(
                name: "IX_TbService_PHECCode",
                table: "TBService",
                newName: "IX_TBService_PHECCode");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TBService",
                table: "TBService",
                column: "Code");

            migrationBuilder.AddForeignKey(
                name: "FK_Episode_TBService_TBServiceCode",
                table: "Episode",
                column: "TBServiceCode",
                principalTable: "TBService",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Hospital_TBService_TBServiceCode",
                table: "Hospital",
                column: "TBServiceCode",
                principalTable: "TBService",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TBService_PHEC_PHECCode",
                table: "TBService",
                column: "PHECCode",
                principalTable: "PHEC",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
