using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class FixFaqSeededQuestion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
UPDATE FrequentlyAskedQuestion 
SET Answer = 'In NTBS, notifications which relate to the same person should be linked together.
 
In order to avoid creating unlinked notifications, you need to perform a search before creating a new notification. This will enable you to confirm:

* The case has not already been notified by another service
* The patient has not had a previous occurence of TB'
WHERE Question = 'Why must I perform a search before creating a new notification?'"
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
UPDATE FrequentlyAskedQuestion 
SET Answer = 'In NTBS, notifications which relate to the same person should be linked together.<br/> In order to avoid creating unlinked notifications, you need to perform a search before creating a new notification. This will enable you to check that the:<li> the case has not already been notified by another service</li><li> the patient has not had a previous occurrence of TB.</li>'
WHERE Question = 'Why must I perform a search before creating a new notification?'");
        }
    }
}
