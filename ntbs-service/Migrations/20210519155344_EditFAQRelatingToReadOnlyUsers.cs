using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class EditFAQRelatingToReadOnlyUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
UPDATE [dbo].[FrequentlyAskedQuestion]
SET Answer = 'You won’t have permission to edit a record if:

1. You are viewing a notification which is linked to a notification which you can edit. (The patient has been notified with TB on more than one occasion and these notifications are linked in the system.)
2. You are a PHE user and the case belongs to your region by residence (the person lives within the region) but not treatment (the person is being treated in a different region).
3. The notification has been transferred out of your service.

You also cannot edit a notification if it is closed. A notification will be closed if it had a final treatment outcome recorded on it more than 12 months ago.

If you have been configured for read-only access to the system you will not be able to edit any notification.'
WHERE Question = 'Why do I not have permission to edit a record?'");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
UPDATE [dbo].[FrequentlyAskedQuestion]
SET Answer = 'You won’t have permission to edit a record if:

1. You are viewing a notification which is linked to a notification which you can edit. (The patient has been notified with TB on more than one occasion and these notifications are linked in the system.)
2. You are a PHE user and the case belongs to your region by residence (the person lives within the region) but not treatment (the person is being treated in a different region).
3. The notification has been transferred out of your service.

You also cannot edit a notification if it is closed. A notification will be closed if it had a final treatment outcome recorded on it more than 12 months ago.'
WHERE Question = 'Why do I not have permission to edit a record?'");
        }
    }
}
