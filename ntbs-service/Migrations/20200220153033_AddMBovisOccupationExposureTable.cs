using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class AddMBovisOccupationExposureTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasOccupationExposure",
                table: "MBovisDetails",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MBovisOccupationExposures",
                columns: table => new
                {
                    MBovisOccupationExposureId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NotificationId = table.Column<int>(nullable: false),
                    YearOfExposure = table.Column<int>(nullable: false),
                    OccupationSetting = table.Column<string>(maxLength: 30, nullable: false),
                    OccupationDuration = table.Column<int>(nullable: false),
                    CountryId = table.Column<int>(nullable: false),
                    OtherDetails = table.Column<string>(maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MBovisOccupationExposures", x => x.MBovisOccupationExposureId);
                    table.ForeignKey(
                        name: "FK_MBovisOccupationExposures_Country_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Country",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MBovisOccupationExposures_MBovisDetails_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "MBovisDetails",
                        principalColumn: "NotificationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MBovisOccupationExposures_CountryId",
                table: "MBovisOccupationExposures",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_MBovisOccupationExposures_NotificationId",
                table: "MBovisOccupationExposures",
                column: "NotificationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MBovisOccupationExposures");

            migrationBuilder.DropColumn(
                name: "HasOccupationExposure",
                table: "MBovisDetails");
        }
    }
}
