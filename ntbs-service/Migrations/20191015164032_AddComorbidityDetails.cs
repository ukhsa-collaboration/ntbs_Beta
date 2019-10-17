using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class AddComorbidityDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ComorbidityDetails",
                columns: table => new
                {
                    NotificationId = table.Column<int>(nullable: false),
                    DiabetesStatus = table.Column<string>(maxLength: 30, nullable: true),
                    HepatitisBStatus = table.Column<string>(maxLength: 30, nullable: true),
                    HepatitisCStatus = table.Column<string>(maxLength: 30, nullable: true),
                    LiverDiseaseStatus = table.Column<string>(maxLength: 30, nullable: true),
                    RenalDiseaseStatus = table.Column<string>(maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComorbidityDetails", x => x.NotificationId);
                    table.ForeignKey(
                        name: "FK_ComorbidityDetails_Notification_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "Notification",
                        principalColumn: "NotificationId",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ComorbidityDetails");
        }
    }
}
