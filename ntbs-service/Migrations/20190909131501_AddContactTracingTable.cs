using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class AddContactTracingTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContactTracing",
                columns: table => new
                {
                    NotificationId = table.Column<int>(nullable: false),
                    AdultsIdentified = table.Column<int>(nullable: false),
                    ChildrenIdentified = table.Column<int>(nullable: false),
                    AdultsScreened = table.Column<int>(nullable: false),
                    ChildrenScreened = table.Column<int>(nullable: false),
                    AdultsActiveTB = table.Column<int>(nullable: false),
                    ChildrenActiveTB = table.Column<int>(nullable: false),
                    AdultsLatentTB = table.Column<int>(nullable: false),
                    ChildrenLatentTB = table.Column<int>(nullable: false),
                    AdultsStartedTreatment = table.Column<int>(nullable: false),
                    ChildrenStartedTreatment = table.Column<int>(nullable: false),
                    AdultsFinishedTreatment = table.Column<int>(nullable: false),
                    ChildrenFinishedTreatment = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactTracing", x => x.NotificationId);
                    table.ForeignKey(
                        name: "FK_ContactTracing_Notification_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "Notification",
                        principalColumn: "NotificationId",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContactTracing");
        }
    }
}
