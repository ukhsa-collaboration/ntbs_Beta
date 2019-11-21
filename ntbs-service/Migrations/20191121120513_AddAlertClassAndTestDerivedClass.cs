using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class AddAlertClassAndTestDerivedClass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Alert",
                columns: table => new
                {
                    AlertId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NotificationId = table.Column<int>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    TbServiceCode = table.Column<string>(maxLength: 16, nullable: true),
                    CaseManagerEmail = table.Column<string>(maxLength: 64, nullable: true),
                    AlertStatus = table.Column<string>(maxLength: 30, nullable: false),
                    ClosureDate = table.Column<DateTime>(nullable: true),
                    ClosingUserId = table.Column<string>(maxLength: 64, nullable: true),
                    AlertType = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alert", x => x.AlertId);
                    table.ForeignKey(
                        name: "FK_Alert_CaseManager_CaseManagerEmail",
                        column: x => x.CaseManagerEmail,
                        principalTable: "CaseManager",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Alert_Notification_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "Notification",
                        principalColumn: "NotificationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Alert_TbService_TbServiceCode",
                        column: x => x.TbServiceCode,
                        principalTable: "TbService",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Alert_CaseManagerEmail",
                table: "Alert",
                column: "CaseManagerEmail");

            migrationBuilder.CreateIndex(
                name: "IX_Alert_TbServiceCode",
                table: "Alert",
                column: "TbServiceCode");

            migrationBuilder.CreateIndex(
                name: "IX_Alert_NotificationId_AlertType",
                table: "Alert",
                columns: new[] { "NotificationId", "AlertType" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Alert");
        }
    }
}
