using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class RemoveReferenceData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // All reference data that was seeded from CSV files has now been removed in favour of seeding from the
            // geography database.
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}