using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class InvertNotPreviouslyHadTbBool : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NotPreviouslyHadTB",
                table: "PatientTBHistories",
                newName: "PreviouslyHadTB");
            migrationBuilder.Sql("UPDATE [dbo].[PatientTBHistories] SET [PreviouslyHadTB] = ~[PreviouslyHadTB]");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE [dbo].[PatientTBHistories] SET [PreviouslyHadTB] = ~[PreviouslyHadTB]");
            migrationBuilder.RenameColumn(
                name: "PreviouslyHadTB",
                table: "PatientTBHistories",
                newName: "NotPreviouslyHadTB");
        }
    }
}
