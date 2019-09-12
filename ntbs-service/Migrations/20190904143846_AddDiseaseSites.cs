using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class AddDiseaseSites : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Site",
                columns: table => new
                {
                    SiteId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Site", x => x.SiteId);
                });

            migrationBuilder.CreateTable(
                name: "NotificationSite",
                columns: table => new
                {
                    NotificationId = table.Column<int>(nullable: false),
                    SiteId = table.Column<int>(nullable: false),
                    SiteDescription = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationSite", x => new { x.NotificationId, x.SiteId });
                    table.ForeignKey(
                        name: "FK_NotificationSite_Notification_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "Notification",
                        principalColumn: "NotificationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NotificationSite_Site_SiteId",
                        column: x => x.SiteId,
                        principalTable: "Site",
                        principalColumn: "SiteId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Site",
                columns: new[] { "SiteId", "Description" },
                values: new object[,]
                {
                    { 1, "Pulmonary" },
                    { 15, "Pericardial" },
                    { 14, "Pleural" },
                    { 13, "Miliary" },
                    { 12, "Laryngeal" },
                    { 11, "Extra-thoracic" },
                    { 10, "Intra-thoracic" },
                    { 16, "Soft tissue/Skin" },
                    { 9, "Genitourinary" },
                    { 7, "Cryptic disseminated" },
                    { 6, "Ocular" },
                    { 5, "other" },
                    { 4, "meningitis" },
                    { 3, "Bone/joint: other" },
                    { 2, "Bone/joint: spine" },
                    { 8, "Gastrointestinal/peritoneal" },
                    { 17, "Other extra-pulmonary" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_NotificationSite_SiteId",
                table: "NotificationSite",
                column: "SiteId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NotificationSite");

            migrationBuilder.DropTable(
                name: "Site");
        }
    }
}
