using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class AddCaseManagerAndTbServiceToTreatmentEvents : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CaseManagerUsername",
                table: "TreatmentEvent",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TbServiceCode",
                table: "TreatmentEvent",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TreatmentEvent_CaseManagerUsername",
                table: "TreatmentEvent",
                column: "CaseManagerUsername");

            migrationBuilder.CreateIndex(
                name: "IX_TreatmentEvent_TbServiceCode",
                table: "TreatmentEvent",
                column: "TbServiceCode");

            migrationBuilder.AddForeignKey(
                name: "FK_TreatmentEvent_User_CaseManagerUsername",
                table: "TreatmentEvent",
                column: "CaseManagerUsername",
                principalTable: "User",
                principalColumn: "Username",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TreatmentEvent_TbService_TbServiceCode",
                table: "TreatmentEvent",
                column: "TbServiceCode",
                principalTable: "TbService",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TreatmentEvent_User_CaseManagerUsername",
                table: "TreatmentEvent");

            migrationBuilder.DropForeignKey(
                name: "FK_TreatmentEvent_TbService_TbServiceCode",
                table: "TreatmentEvent");

            migrationBuilder.DropIndex(
                name: "IX_TreatmentEvent_CaseManagerUsername",
                table: "TreatmentEvent");

            migrationBuilder.DropIndex(
                name: "IX_TreatmentEvent_TbServiceCode",
                table: "TreatmentEvent");

            migrationBuilder.DropColumn(
                name: "CaseManagerUsername",
                table: "TreatmentEvent");

            migrationBuilder.DropColumn(
                name: "TbServiceCode",
                table: "TreatmentEvent");
        }
    }
}
