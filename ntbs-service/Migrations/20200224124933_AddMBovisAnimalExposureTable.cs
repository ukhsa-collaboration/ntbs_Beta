using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class AddMBovisAnimalExposureTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasAnimalExposure",
                table: "MBovisDetails",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MBovisAnimalExposure",
                columns: table => new
                {
                    MBovisAnimalExposureId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NotificationId = table.Column<int>(nullable: false),
                    YearOfExposure = table.Column<int>(nullable: false),
                    AnimalType = table.Column<string>(maxLength: 30, nullable: false),
                    Animal = table.Column<string>(maxLength: 35, nullable: false),
                    AnimalTbStatus = table.Column<string>(maxLength: 30, nullable: false),
                    ExposureDuration = table.Column<int>(nullable: false),
                    CountryId = table.Column<int>(nullable: false),
                    OtherDetails = table.Column<string>(maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MBovisAnimalExposure", x => x.MBovisAnimalExposureId);
                    table.ForeignKey(
                        name: "FK_MBovisAnimalExposure_Country_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Country",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MBovisAnimalExposure_MBovisDetails_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "MBovisDetails",
                        principalColumn: "NotificationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MBovisAnimalExposure_CountryId",
                table: "MBovisAnimalExposure",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_MBovisAnimalExposure_NotificationId",
                table: "MBovisAnimalExposure",
                column: "NotificationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MBovisAnimalExposure");

            migrationBuilder.DropColumn(
                name: "HasAnimalExposure",
                table: "MBovisDetails");
        }
    }
}
