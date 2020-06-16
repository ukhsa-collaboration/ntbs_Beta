using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class AddAnchorLinkToFAQ : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<string>(
                name: "AnchorLink",
                table: "FrequentlyAskedQuestion",
                maxLength: 50,
                nullable: true);
            
            migrationBuilder.UpdateData(
                table: "FrequentlyAskedQuestion",
                keyColumn: "Question",
                keyValue: "Why do I not have permission to edit a record?",
                column: "AnchorLink",
                value: "no-permission-to-edit-a-record");            
            
            migrationBuilder.UpdateData(
                table: "FrequentlyAskedQuestion",
                keyColumn: "Question",
                keyValue: "Why can’t I view the full details of a record?",
                column: "AnchorLink",
                value: "no-permission-to-view-a-record");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnchorLink",
                table: "FrequentlyAskedQuestion");
        }
    }
}
