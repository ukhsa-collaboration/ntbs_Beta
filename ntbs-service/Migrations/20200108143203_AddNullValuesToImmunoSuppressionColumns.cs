using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class AddNullValuesToImmunoSuppressionColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "HasBioTherapy",
                table: "ImmunosuppressionDetails",
                nullable: true
            );
            migrationBuilder.AlterColumn<bool>(
                name: "HasTransplantation",
                table: "ImmunosuppressionDetails",
                nullable: true
            );
            migrationBuilder.AlterColumn<bool>(
                name: "HasOther",
                table: "ImmunosuppressionDetails",
                nullable: true
            );
            migrationBuilder.Sql(
                @"UPDATE ImmunosuppressionDetails
                    SET HasBioTherapy = NULL
                        , HasTransplantation = NULL
                        , HasOther = NULL
                        , OtherDescription = NULL
                    WHERE Status NOT LIKE 'Yes'
                "
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"UPDATE ImmunosuppressionDetails
                  SET HasBioTherapy = 0
                  WHERE HasBioTherapy IS NULL"
            );
            migrationBuilder.AlterColumn<bool>(
                name: "HasBioTherapy",
                table: "ImmunosuppressionDetails",
                nullable: false
            );
            migrationBuilder.Sql(
                @"UPDATE ImmunosuppressionDetails
                  SET HasTransplantation = 0
                  WHERE HasTransplantation IS NULL"
            );
            migrationBuilder.AlterColumn<bool>(
                name: "HasTransplantation",
                table: "ImmunosuppressionDetails",
                nullable: false
            );
            migrationBuilder.Sql(
                @"UPDATE ImmunosuppressionDetails
                  SET HasOther = 0
                  WHERE HasOther IS NULL"
            );
            migrationBuilder.AlterColumn<bool>(
                name: "HasOther",
                table: "ImmunosuppressionDetails",
                nullable: false
            );
        }
    }
}
