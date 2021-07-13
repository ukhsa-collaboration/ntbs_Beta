using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class ReorderFAQs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData("FrequentlyAskedQuestion", "Question",
                "Why must I perform a search before creating a new notification?", "OrderIndex", 2);
            migrationBuilder.UpdateData("FrequentlyAskedQuestion", "Question",
                "Why can’t I view the full details of a record?", "OrderIndex", 3);
            migrationBuilder.UpdateData("FrequentlyAskedQuestion", "Question",
                "Why do I not have permission to edit a record?", "OrderIndex", 4);
            migrationBuilder.UpdateData("FrequentlyAskedQuestion", "Question",
                "What should I do if I want to notify a case of TB for a person who has had TB before?", "OrderIndex", 1);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData("FrequentlyAskedQuestion", "Question",
                "Why must I perform a search before creating a new notification?", "OrderIndex", 1);
            migrationBuilder.UpdateData("FrequentlyAskedQuestion", "Question",
                "Why can’t I view the full details of a record?", "OrderIndex", 2);
            migrationBuilder.UpdateData("FrequentlyAskedQuestion", "Question",
                "Why do I not have permission to edit a record?", "OrderIndex", 3);
            migrationBuilder.UpdateData("FrequentlyAskedQuestion", "Question",
                "What should I do if I want to notify a case of TB for a person who has had TB before?", "OrderIndex", 4);
        }
    }
}
