using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class MakeRiskFactorPeriodsNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "MoreThanFiveYearsAgo",
                table: "RiskFactorImprisonment",
                nullable: true,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<bool>(
                name: "IsCurrent",
                table: "RiskFactorImprisonment",
                nullable: true,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<bool>(
                name: "InPastFiveYears",
                table: "RiskFactorImprisonment",
                nullable: true,
                oldClrType: typeof(bool));
            
            migrationBuilder.Sql(
                @"UPDATE RiskFactorImprisonment
                    SET InPastFiveYears = NULL
                        , IsCurrent = NULL
                        , MoreThanFiveYearsAgo = NULL
                    WHERE Status <> 'Yes' OR Status IS NULL
                "
            );
            

            migrationBuilder.AlterColumn<bool>(
                name: "MoreThanFiveYearsAgo",
                table: "RiskFactorHomelessness",
                nullable: true,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<bool>(
                name: "IsCurrent",
                table: "RiskFactorHomelessness",
                nullable: true,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<bool>(
                name: "InPastFiveYears",
                table: "RiskFactorHomelessness",
                nullable: true,
                oldClrType: typeof(bool));
            
            migrationBuilder.Sql(
                @"UPDATE RiskFactorHomelessness
                    SET InPastFiveYears = NULL
                        , IsCurrent = NULL
                        , MoreThanFiveYearsAgo = NULL
                    WHERE Status <> 'Yes' OR Status IS NULL
                "
            );
            

            migrationBuilder.AlterColumn<bool>(
                name: "MoreThanFiveYearsAgo",
                table: "RiskFactorDrugs",
                nullable: true,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<bool>(
                name: "IsCurrent",
                table: "RiskFactorDrugs",
                nullable: true,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<bool>(
                name: "InPastFiveYears",
                table: "RiskFactorDrugs",
                nullable: true,
                oldClrType: typeof(bool));
            
            migrationBuilder.Sql(
                @"UPDATE RiskFactorDrugs
                    SET InPastFiveYears = NULL
                        , IsCurrent = NULL
                        , MoreThanFiveYearsAgo = NULL
                    WHERE Status <> 'Yes' OR Status IS NULL
                "
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"UPDATE RiskFactorImprisonment
                    SET InPastFiveYears = 0
                        , IsCurrent = 0
                        , MoreThanFiveYearsAgo = 0
                    WHERE Status <> 'Yes' OR Status IS NULL
                "
            );
            
            migrationBuilder.AlterColumn<bool>(
                name: "MoreThanFiveYearsAgo",
                table: "RiskFactorImprisonment",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsCurrent",
                table: "RiskFactorImprisonment",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "InPastFiveYears",
                table: "RiskFactorImprisonment",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);
            
            migrationBuilder.Sql(
                @"UPDATE RiskFactorHomelessness
                    SET InPastFiveYears = 0
                        , IsCurrent = 0
                        , MoreThanFiveYearsAgo = 0
                    WHERE Status <> 'Yes' OR Status IS NULL
                "
            );

            migrationBuilder.AlterColumn<bool>(
                name: "MoreThanFiveYearsAgo",
                table: "RiskFactorHomelessness",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsCurrent",
                table: "RiskFactorHomelessness",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "InPastFiveYears",
                table: "RiskFactorHomelessness",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);
            
            migrationBuilder.Sql(
                @"UPDATE RiskFactorDrugs
                    SET InPastFiveYears = 0
                        , IsCurrent = 0
                        , MoreThanFiveYearsAgo = 0
                    WHERE Status <> 'Yes' OR Status IS NULL
                "
            );

            migrationBuilder.AlterColumn<bool>(
                name: "MoreThanFiveYearsAgo",
                table: "RiskFactorDrugs",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsCurrent",
                table: "RiskFactorDrugs",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "InPastFiveYears",
                table: "RiskFactorDrugs",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);
        }
    }
}
