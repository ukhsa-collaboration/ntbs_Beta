using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class AddEpisodeTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notification_Hospital",
                table: "Notification");
            
            migrationBuilder.DropTable(
                name: "Hospital");

            migrationBuilder.CreateTable(
                name: "TBService",
                columns: table => new
                {
                    Code = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TBService", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "Hospital",
                columns: table => new
                {
                    HospitalId = table.Column<Guid>(nullable: false),
                    CountryCode = table.Column<string>(maxLength: 50, nullable: true),
                    Name = table.Column<string>(maxLength: 200, nullable: true),
                    TBServiceCode = table.Column<string>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hospital", x => x.HospitalId);
                    table.ForeignKey(
                        name: "FK_Hospital_TBService_TBServiceCode",
                        column: x => x.TBServiceCode,
                        principalTable: "TBService",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Episode",
                columns: table => new
                {
                    NotificationId = table.Column<int>(nullable: false),
                    Consultant = table.Column<string>(maxLength: 200, nullable: true),
                    CaseManager = table.Column<string>(maxLength: 200, nullable: true),
                    TBServiceCode = table.Column<string>(nullable: true),
                    HospitalId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Episode", x => x.NotificationId);
                    table.ForeignKey(
                        name: "FK_Episode_Hospital_HospitalId",
                        column: x => x.HospitalId,
                        principalTable: "Hospital",
                        principalColumn: "HospitalId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Episode_Notification_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "Notification",
                        principalColumn: "NotificationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Episode_TBService_TBServiceCode",
                        column: x => x.TBServiceCode,
                        principalTable: "TBService",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Hospital_TBServiceCode",
                table: "Hospital",
                column: "TBServiceCode");

            migrationBuilder.CreateIndex(
                name: "IX_Episode_HospitalId",
                table: "Episode",
                column: "HospitalId");

            migrationBuilder.CreateIndex(
                name: "IX_Episode_TBServiceCode",
                table: "Episode",
                column: "TBServiceCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Hospital_TBService_TBServiceCode",
                table: "Hospital");

            migrationBuilder.DropTable(
                name: "Episode");

            migrationBuilder.DropTable(
                name: "TBService");
            
            migrationBuilder.DropTable(
                name: "Hospital");

            migrationBuilder.CreateTable(
                name: "Hospital",
                columns: table => new
                {
                    HospitalId = table.Column<int>(nullable: false),
                    Label = table.Column<string>(maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hospital", x => x.HospitalId);
                });

            migrationBuilder.AlterColumn<int>(
                name: "HospitalId",
                table: "Notification",
                nullable: true,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_Hospital",
                table: "Notification",
                column: "HospitalId",
                principalTable: "Hospital",
                principalColumn: "HospitalId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
