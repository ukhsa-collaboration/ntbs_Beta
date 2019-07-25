using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Hospital",
                columns: table => new
                {
                    HospitalId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Label = table.Column<string>(maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hospital", x => x.HospitalId);
                });

            migrationBuilder.CreateTable(
                name: "Region",
                columns: table => new
                {
                    RegionId = table.Column<byte>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Label = table.Column<string>(maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Region", x => x.RegionId);
                });

            migrationBuilder.CreateTable(
                name: "ResistentDrug",
                columns: table => new
                {
                    ResistentDrugId = table.Column<byte>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 50, nullable: true),
                    Label = table.Column<string>(maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResistentDrug", x => x.ResistentDrugId);
                });

            migrationBuilder.CreateTable(
                name: "Sex",
                columns: table => new
                {
                    SexId = table.Column<byte>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Label = table.Column<string>(maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sex", x => x.SexId);
                });

            migrationBuilder.CreateTable(
                name: "Patient",
                columns: table => new
                {
                    PatientId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Surname = table.Column<string>(maxLength: 200, nullable: true),
                    Forename = table.Column<string>(maxLength: 200, nullable: true),
                    NhsNumber = table.Column<string>(maxLength: 50, nullable: true),
                    SexId = table.Column<byte>(nullable: false),
                    Dob = table.Column<DateTime>(type: "date", nullable: false),
                    UkBorn = table.Column<byte>(nullable: true),
                    RegionId = table.Column<byte>(nullable: false),
                    EtsId = table.Column<Guid>(nullable: true),
                    LtbrId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patient", x => x.PatientId);
                    table.ForeignKey(
                        name: "FK_Patient_Region",
                        column: x => x.RegionId,
                        principalTable: "Region",
                        principalColumn: "RegionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Patient_Sex",
                        column: x => x.SexId,
                        principalTable: "Sex",
                        principalColumn: "SexId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Notification",
                columns: table => new
                {
                    NotificationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PatientId = table.Column<int>(nullable: false),
                    HospitalId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification", x => x.NotificationId);
                    table.ForeignKey(
                        name: "FK_Notification_Hospital",
                        column: x => x.HospitalId,
                        principalTable: "Hospital",
                        principalColumn: "HospitalId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Notification_Patient",
                        column: x => x.PatientId,
                        principalTable: "Patient",
                        principalColumn: "PatientId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DrugResistence",
                columns: table => new
                {
                    DrugResistenceId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NotificationId = table.Column<int>(nullable: false),
                    ResistentDrugId = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrugResistence", x => x.DrugResistenceId);
                    table.ForeignKey(
                        name: "FK_DrugResistence_Notification",
                        column: x => x.NotificationId,
                        principalTable: "Notification",
                        principalColumn: "NotificationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ResistentDrug_Notification",
                        column: x => x.ResistentDrugId,
                        principalTable: "ResistentDrug",
                        principalColumn: "ResistentDrugId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Episode",
                columns: table => new
                {
                    EpisodeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NotificationId = table.Column<int>(nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Episode", x => x.EpisodeId);
                    table.ForeignKey(
                        name: "FK_Episode_Notification",
                        column: x => x.NotificationId,
                        principalTable: "Notification",
                        principalColumn: "NotificationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DrugResistence_NotificationId",
                table: "DrugResistence",
                column: "NotificationId");

            migrationBuilder.CreateIndex(
                name: "IX_DrugResistence_ResistentDrugId",
                table: "DrugResistence",
                column: "ResistentDrugId");

            migrationBuilder.CreateIndex(
                name: "IX_Episode_NotificationId",
                table: "Episode",
                column: "NotificationId");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_HospitalId",
                table: "Notification",
                column: "HospitalId");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_PatientId",
                table: "Notification",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Patient_RegionId",
                table: "Patient",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_Patient_SexId",
                table: "Patient",
                column: "SexId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DrugResistence");

            migrationBuilder.DropTable(
                name: "Episode");

            migrationBuilder.DropTable(
                name: "ResistentDrug");

            migrationBuilder.DropTable(
                name: "Notification");

            migrationBuilder.DropTable(
                name: "Hospital");

            migrationBuilder.DropTable(
                name: "Patient");

            migrationBuilder.DropTable(
                name: "Region");

            migrationBuilder.DropTable(
                name: "Sex");
        }
    }
}
