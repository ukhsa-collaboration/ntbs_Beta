using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class AddDrugResistanceProfileTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DrugResistanceProfile",
                columns: table => new
                {
                    NotificationId = table.Column<int>(nullable: false),
                    Species = table.Column<string>(nullable: true),
                    DrugResistanceProfileString = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrugResistanceProfile", x => x.NotificationId);
                    table.ForeignKey(
                        name: "FK_DrugResistanceProfile_Notification_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "Notification",
                        principalColumn: "NotificationId",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DrugResistanceProfile");
        }
    }
}
