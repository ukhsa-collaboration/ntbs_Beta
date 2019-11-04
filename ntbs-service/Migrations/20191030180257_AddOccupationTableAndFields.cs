using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class AddOccupationTableAndFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OccupationId",
                table: "Patients",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OccupationOther",
                table: "Patients",
                maxLength: 50,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Occupation",
                columns: table => new
                {
                    OccupationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Sector = table.Column<string>(maxLength: 40, nullable: true),
                    Role = table.Column<string>(maxLength: 40, nullable: true),
                    HasFreeTextField = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Occupation", x => x.OccupationId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Patients_OccupationId",
                table: "Patients",
                column: "OccupationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_Occupation_OccupationId",
                table: "Patients",
                column: "OccupationId",
                principalTable: "Occupation",
                principalColumn: "OccupationId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Patients_Occupation_OccupationId",
                table: "Patients");

            migrationBuilder.DropTable(
                name: "Occupation");

            migrationBuilder.DropIndex(
                name: "IX_Patients_OccupationId",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "OccupationId",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "OccupationOther",
                table: "Patients");
        }
    }
}
