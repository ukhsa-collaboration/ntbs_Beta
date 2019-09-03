using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class MakePatientDetailsOwnedEntityType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notification_Patient",
                table: "Notification");

            migrationBuilder.DropTable(
                name: "Patient");

            migrationBuilder.DropIndex(
                name: "IX_Notification_PatientId",
                table: "Notification");

            migrationBuilder.DropColumn(
                name: "PatientId",
                table: "Notification");

            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    NotificationId = table.Column<int>(nullable: false),
                    FamilyName = table.Column<string>(maxLength: 35, nullable: true),
                    GivenName = table.Column<string>(maxLength: 35, nullable: true),
                    NhsNumber = table.Column<string>(maxLength: 10, nullable: true),
                    Dob = table.Column<DateTime>(nullable: true),
                    UkBorn = table.Column<bool>(nullable: true),
                    Postcode = table.Column<string>(nullable: true),
                    CountryId = table.Column<int>(nullable: true),
                    EthnicityId = table.Column<int>(nullable: true),
                    SexId = table.Column<int>(nullable: true),
                    NhsNumberNotKnown = table.Column<bool>(nullable: false),
                    NoFixedAbode = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.NotificationId);
                    table.ForeignKey(
                        name: "FK_Patients_Country_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Country",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Patients_Ethnicity_EthnicityId",
                        column: x => x.EthnicityId,
                        principalTable: "Ethnicity",
                        principalColumn: "EthnicityId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Patients_Notification_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "Notification",
                        principalColumn: "NotificationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Patients_Sex_SexId",
                        column: x => x.SexId,
                        principalTable: "Sex",
                        principalColumn: "SexId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Patients_CountryId",
                table: "Patients",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_EthnicityId",
                table: "Patients",
                column: "EthnicityId");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_SexId",
                table: "Patients",
                column: "SexId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Patients");

            migrationBuilder.AddColumn<int>(
                name: "PatientId",
                table: "Notification",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Patient",
                columns: table => new
                {
                    PatientId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CountryId = table.Column<int>(nullable: true),
                    Dob = table.Column<DateTime>(type: "date", nullable: true),
                    EthnicityId = table.Column<int>(nullable: true),
                    FamilyName = table.Column<string>(maxLength: 35, nullable: true),
                    GivenName = table.Column<string>(maxLength: 35, nullable: true),
                    NhsNumber = table.Column<string>(maxLength: 10, nullable: true),
                    NhsNumberNotKnown = table.Column<bool>(nullable: false),
                    NoFixedAbode = table.Column<bool>(nullable: false),
                    Postcode = table.Column<string>(maxLength: 50, nullable: true),
                    SexId = table.Column<int>(nullable: true),
                    UkBorn = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patient", x => x.PatientId);
                    table.ForeignKey(
                        name: "FK_Patient_Country",
                        column: x => x.CountryId,
                        principalTable: "Country",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Patient_Ethnicity",
                        column: x => x.EthnicityId,
                        principalTable: "Ethnicity",
                        principalColumn: "EthnicityId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Patient_Sex",
                        column: x => x.SexId,
                        principalTable: "Sex",
                        principalColumn: "SexId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Notification_PatientId",
                table: "Notification",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Patient_CountryId",
                table: "Patient",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Patient_EthnicityId",
                table: "Patient",
                column: "EthnicityId");

            migrationBuilder.CreateIndex(
                name: "IX_Patient_SexId",
                table: "Patient",
                column: "SexId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_Patient",
                table: "Notification",
                column: "PatientId",
                principalTable: "Patient",
                principalColumn: "PatientId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
