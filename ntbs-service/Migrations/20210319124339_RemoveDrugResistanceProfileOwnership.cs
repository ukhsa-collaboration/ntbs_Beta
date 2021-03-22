using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    // This migration is empty because making the DrugResistanceProfile unowned does not affect the database
    // model. However it is included here because the snapshot has changed, and so a corresponding migration
    // is sensible.
    public partial class RemoveDrugResistanceProfileOwnership : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Do nothing
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Do nothing
        }
    }
}
