using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class CleanMigrationHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"IF EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20210802135016_ChangeDidNotStartTreatmentToStartedTreatment')
                    BEGIN
                    WITH Migrations AS
                    (
                        SELECT
                        ROW_NUMBER() OVER(ORDER BY MigrationId ASC) AS Row, MigrationId
                        FROM [__EFMigrationsHistory]
                        WHERE MigrationId <> '20200326094027_AddDataProtectionKeys'
                        AND MigrationId <> '20200311133500_AddSessionStateTable'
                        AND MigrationId <> '20190820134541_InitialMigration'
                    )
                    DELETE
                    FROM Migrations
                    WHERE Row <= (SELECT Row FROM Migrations WHERE [MigrationId] = N'20210802135016_ChangeDidNotStartTreatmentToStartedTreatment');
                    END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
