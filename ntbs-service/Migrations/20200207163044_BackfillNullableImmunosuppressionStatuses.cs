using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class BackfillNullableImmunosuppressionStatuses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"UPDATE ImmunosuppressionDetails
                    SET HasBioTherapy = NULL
                        , HasTransplantation = NULL
                        , HasOther = NULL
                        , OtherDescription = NULL
                    WHERE Status NOT LIKE 'Yes' OR Status IS NULL
                "
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"UPDATE ImmunosuppressionDetails
                    SET HasBioTherapy = 0
                        , HasTransplantation = 0
                        , HasOther = 0
                        , OtherDescription = 0
                    WHERE Status NOT LIKE 'Yes' OR Status IS NULL
                "
            );
        }
    }
}
