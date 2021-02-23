using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class MoveTbServiceAndCaseManagerToTransferAlert : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Alert_TbService_TbServiceCode",
                table: "Alert");

            migrationBuilder.DropIndex(
                name: "IX_Alert_AlertStatus_AlertType_TbServiceCode",
                table: "Alert");

            migrationBuilder.AlterColumn<string>(
                name: "TbServiceCode",
                table: "Alert",
                maxLength: 16,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 16);

            migrationBuilder.CreateIndex(
                name: "IX_Alert_AlertStatus_AlertType",
                table: "Alert",
                columns: new[] { "AlertStatus", "AlertType" });

            migrationBuilder.AddForeignKey(
                name: "FK_Alert_TbService_TbServiceCode",
                table: "Alert",
                column: "TbServiceCode",
                principalSchema: "ReferenceData",
                principalTable: "TbService",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Alert_TbService_TbServiceCode",
                table: "Alert");

            migrationBuilder.DropIndex(
                name: "IX_Alert_AlertStatus_AlertType",
                table: "Alert");

            migrationBuilder.AlterColumn<string>(
                name: "TbServiceCode",
                table: "Alert",
                maxLength: 16,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 16,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Alert_AlertStatus_AlertType_TbServiceCode",
                table: "Alert",
                columns: new[] { "AlertStatus", "AlertType", "TbServiceCode" });

            migrationBuilder.AddForeignKey(
                name: "FK_Alert_TbService_TbServiceCode",
                table: "Alert",
                column: "TbServiceCode",
                principalSchema: "ReferenceData",
                principalTable: "TbService",
                principalColumn: "Code",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
