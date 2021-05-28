using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class AddLegacyImportMigrationRunTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LegacyImportMigrationRun",
                columns: table => new
                {
                    LegacyImportMigrationRunId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AppRelease = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LegacyIdList = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RangeStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RangeEndDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LegacyImportMigrationRun", x => x.LegacyImportMigrationRunId);
                });

            migrationBuilder.CreateTable(
                name: "LegacyImportNotificationLogMessage",
                columns: table => new
                {
                    LegacyImportNotificationLogMessageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LegacyImportMigrationRunId = table.Column<int>(type: "int", nullable: false),
                    OldNotificationId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LogMessageLevel = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LegacyImportNotificationLogMessage", x => x.LegacyImportNotificationLogMessageId);
                    table.ForeignKey(
                        name: "FK_LegacyImportNotificationLogMessage_LegacyImportMigrationRun_LegacyImportMigrationRunId",
                        column: x => x.LegacyImportMigrationRunId,
                        principalTable: "LegacyImportMigrationRun",
                        principalColumn: "LegacyImportMigrationRunId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LegacyImportNotificationOutcome",
                columns: table => new
                {
                    LegacyImportNotificationOutcomeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LegacyImportMigrationRunId = table.Column<int>(type: "int", nullable: false),
                    OldNotificationId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NtbsId = table.Column<int>(type: "int", nullable: true),
                    SuccessfullyMigrated = table.Column<bool>(type: "bit", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LegacyImportNotificationOutcome", x => x.LegacyImportNotificationOutcomeId);
                    table.ForeignKey(
                        name: "FK_LegacyImportNotificationOutcome_LegacyImportMigrationRun_LegacyImportMigrationRunId",
                        column: x => x.LegacyImportMigrationRunId,
                        principalTable: "LegacyImportMigrationRun",
                        principalColumn: "LegacyImportMigrationRunId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LegacyImportNotificationLogMessage_LegacyImportMigrationRunId",
                table: "LegacyImportNotificationLogMessage",
                column: "LegacyImportMigrationRunId");

            migrationBuilder.CreateIndex(
                name: "IX_LegacyImportNotificationOutcome_LegacyImportMigrationRunId",
                table: "LegacyImportNotificationOutcome",
                column: "LegacyImportMigrationRunId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LegacyImportNotificationLogMessage");

            migrationBuilder.DropTable(
                name: "LegacyImportNotificationOutcome");

            migrationBuilder.DropTable(
                name: "LegacyImportMigrationRun");
        }
    }
}
