using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class AddInitialFAQandAnswers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "FrequentlyAskedQuestion",
                columns: new[] { "Question", "Answer" },
                values: new[] {"Why must I perform a search before creating a new notification?", "In NTBS, notifications which relate to the same person should be linked together.<br/> In order to avoid creating unlinked notifications, you need to perform a search before creating a new notification. This will enable you to check that the:<li> the case has not already been notified by another service</li><li> the patient has not had a previous occurrence of TB.</li>"}
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                "FrequentlyAskedQuestion",
                "Question",
                "Why must I perform a search before creating a new notification?");
        }
    }
}
