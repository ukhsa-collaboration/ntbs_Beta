using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class AddImmunosuppressionAndReduceEnumFieldLength : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SmokingStatus",
                table: "SocialRiskFactors",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MentalHealthStatus",
                table: "SocialRiskFactors",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AlcoholMisuseStatus",
                table: "SocialRiskFactors",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "RiskFactorImprisonment",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "RiskFactorImprisonment",
                maxLength: 30,
                nullable: false,
                defaultValue: "Imprisonment");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "RiskFactorHomelessness",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "RiskFactorHomelessness",
                maxLength: 30,
                nullable: false,
                defaultValue: "Homelessness");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "RiskFactorDrugs",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "RiskFactorDrugs",
                maxLength: 30,
                nullable: false,
                defaultValue: "Drugs");

            migrationBuilder.AlterColumn<bool>(
                name: "NotPreviouslyHadTB",
                table: "PatientTBHistories",
                nullable: true,
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<string>(
                name: "NotificationStatus",
                table: "Notification",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "BCGVaccinationState",
                table: "ClinicalDetails",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.CreateTable(
                name: "ImmunosuppressionDetails",
                columns: table => new
                {
                    NotificationId = table.Column<int>(nullable: false),
                    Status = table.Column<string>(maxLength: 30, nullable: true),
                    HasBioTherapy = table.Column<bool>(nullable: false),
                    HasTransplantation = table.Column<bool>(nullable: false),
                    HasOther = table.Column<bool>(nullable: false),
                    OtherDescription = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImmunosuppressionDetails", x => x.NotificationId);
                    table.ForeignKey(
                        name: "FK_ImmunosuppressionDetails_Notification_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "Notification",
                        principalColumn: "NotificationId",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ImmunosuppressionDetails");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "RiskFactorImprisonment",
                oldClrType: typeof(string),
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "RiskFactorHomelessness",
                oldClrType: typeof(string),
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "RiskFactorDrugs",
                oldClrType: typeof(string),
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SmokingStatus",
                table: "SocialRiskFactors",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MentalHealthStatus",
                table: "SocialRiskFactors",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AlcoholMisuseStatus",
                table: "SocialRiskFactors",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "RiskFactorImprisonment",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "RiskFactorHomelessness",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "RiskFactorDrugs",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "NotPreviouslyHadTB",
                table: "PatientTBHistories",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NotificationStatus",
                table: "Notification",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "BCGVaccinationState",
                table: "ClinicalDetails",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 30,
                oldNullable: true);
        }
    }
}
