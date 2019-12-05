using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class AddManualTestTypeAndSampleTypeTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ManualTestType",
                columns: table => new
                {
                    ManualTestTypeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(maxLength: 40, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ManualTestType", x => x.ManualTestTypeId);
                });

            migrationBuilder.CreateTable(
                name: "SampleType",
                columns: table => new
                {
                    SampleTypeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Category = table.Column<string>(maxLength: 40, nullable: true),
                    Description = table.Column<string>(maxLength: 40, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SampleType", x => x.SampleTypeId);
                });

            migrationBuilder.CreateTable(
                name: "ManualTestTypeSampleType",
                columns: table => new
                {
                    ManualTestTypeId = table.Column<int>(nullable: false),
                    SampleTypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ManualTestTypeSampleType", x => new { x.ManualTestTypeId, x.SampleTypeId });
                    table.ForeignKey(
                        name: "FK_ManualTestTypeSampleType_ManualTestType_ManualTestTypeId",
                        column: x => x.ManualTestTypeId,
                        principalTable: "ManualTestType",
                        principalColumn: "ManualTestTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ManualTestTypeSampleType_SampleType_SampleTypeId",
                        column: x => x.SampleTypeId,
                        principalTable: "SampleType",
                        principalColumn: "SampleTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ManualTestTypeSampleType_SampleTypeId",
                table: "ManualTestTypeSampleType",
                column: "SampleTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ManualTestTypeSampleType");

            migrationBuilder.DropTable(
                name: "ManualTestType");

            migrationBuilder.DropTable(
                name: "SampleType");
        }
    }
}
