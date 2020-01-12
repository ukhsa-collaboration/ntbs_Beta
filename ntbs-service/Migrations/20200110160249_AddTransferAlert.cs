using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class AddTransferAlert : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<string>(
                name: "OtherReasonDescription",
                table: "Alert",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TransferReason",
                table: "Alert",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TransferRequestNote",
                table: "Alert",
                maxLength: 200,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OtherReasonDescription",
                table: "Alert");

            migrationBuilder.DropColumn(
                name: "TransferReason",
                table: "Alert");

            migrationBuilder.DropColumn(
                name: "TransferRequestNote",
                table: "Alert");
        }
    }
}
