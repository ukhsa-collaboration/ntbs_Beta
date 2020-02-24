using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class AddMBovisMilkConsumptionEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasUnpasteurisedMilkConsumption",
                table: "MBovisDetails",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MBovisUnpasteurisedMilkConsumption",
                columns: table => new
                {
                    MBovisUnpasteurisedMilkConsumptionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NotificationId = table.Column<int>(nullable: false),
                    YearOfConsumption = table.Column<int>(nullable: false),
                    MilkProductType = table.Column<string>(maxLength: 30, nullable: false),
                    ConsumptionFrequency = table.Column<string>(maxLength: 30, nullable: false),
                    CountryId = table.Column<int>(nullable: false),
                    OtherDetails = table.Column<string>(maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MBovisUnpasteurisedMilkConsumption", x => x.MBovisUnpasteurisedMilkConsumptionId);
                    table.ForeignKey(
                        name: "FK_MBovisUnpasteurisedMilkConsumption_Country_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Country",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MBovisUnpasteurisedMilkConsumption_MBovisDetails_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "MBovisDetails",
                        principalColumn: "NotificationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MBovisUnpasteurisedMilkConsumption_CountryId",
                table: "MBovisUnpasteurisedMilkConsumption",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_MBovisUnpasteurisedMilkConsumption_NotificationId",
                table: "MBovisUnpasteurisedMilkConsumption",
                column: "NotificationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MBovisUnpasteurisedMilkConsumption");

            migrationBuilder.DropColumn(
                name: "HasUnpasteurisedMilkConsumption",
                table: "MBovisDetails");
        }
    }
}
