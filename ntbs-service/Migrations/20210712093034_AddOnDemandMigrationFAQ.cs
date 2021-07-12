using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class AddOnDemandMigrationFAQ : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "FrequentlyAskedQuestion",
                columns: new [] { "Question", "Answer", "AnchorLink", "OrderIndex"},
                values: new object[,]
                {
                    {
                        "What should I do if I want to notify a case of TB for a person who has had TB before?",
                        @"You will be able to find the older case record when you perform a search, as this searches NTBS, ETS and LTBR.  Select the old case from the search results and then choose to import it into NTBS.  Choose 'New notification for this patient' from the 'manage notification' menu.  This will create a draft notification linked to the older case.

You can watch a training video on this at: <a href=""https://phecloud.sharepoint.com/sites/NTBSResources/SitePages/Training-video-search-and-import-legacy-notifications.aspx"" target=""_blank"">https://phecloud.sharepoint.com/sites/NTBSResources/SitePages/Training-video-search-and-import-legacy-notifications.aspx</a>",
                        "notify-case-for-previous-patient",
                        4
                    }
                }
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"DELETE FROM FrequentlyAskedQuestion WHERE Question = 'What should I do if I want to notify a case of TB for a person who has had TB before?'");
        }
    }
}
