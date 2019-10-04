using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class MakePreviousTBHistoryBooleanNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "NotPreviouslyHadTB",
                table: "PatientTBHistories",
                nullable: true,
                oldClrType: typeof(bool));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "NotPreviouslyHadTB",
                table: "PatientTBHistories",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);
        }
    }
}
