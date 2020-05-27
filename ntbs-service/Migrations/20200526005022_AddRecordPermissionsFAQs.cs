using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class AddRecordPermissionsFAQs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "FrequentlyAskedQuestion",
                columns: new [] { "Question", "Answer"},
                values: new object[,]
                {
                    {
                        "Why can’t I view the full details of a record?",
                        @"You can’t view the full details of a record which:

* does not belong to your TB service (if you work in a TB service)
* is not being treated in your region (if you work for a PHE team)"
                    },
                    {
                        "Why do I not have permission to edit a record?",
                        @"You won’t have permission to edit a record if:

1. You are viewing a notification which is linked to a notification which you can edit. (The patient has been notified with TB on more than one occasion and these notifications are linked in the system.)
2. You are a PHE user and the case belongs to your region by residence (the person lives within the region) but not treatment (the person is being treated in a different region).
3. The notification has been transferred out of your service but you have retained access for a period of time - 48 months.

You also cannot edit a notification if it is closed. A notification will be closed if it had a final treatment outcome recorded on it more than 12 months ago."
                    }
                }

                );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"DELETE FROM FrequentlyAskedQuestion WHERE Question = 'Why can’t I view the full details of a record?'");
            migrationBuilder.Sql(
                @"DELETE FROM FrequentlyAskedQuestion WHERE Question = 'Why do I not have permission to edit a record?'");
        }
    }
}
