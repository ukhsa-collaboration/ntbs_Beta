using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class AddEpisodeFKToCaseManager : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CaseManager",
                table: "Episode");

            migrationBuilder.AddColumn<string>(
                name: "CaseManagerEmail",
                table: "Episode",
                maxLength: 64,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Episode_CaseManagerEmail",
                table: "Episode",
                column: "CaseManagerEmail");

            migrationBuilder.AddForeignKey(
                name: "FK_Episode_CaseManager_CaseManagerEmail",
                table: "Episode",
                column: "CaseManagerEmail",
                principalTable: "CaseManager",
                principalColumn: "Email",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Episode_CaseManager_CaseManagerEmail",
                table: "Episode");

            migrationBuilder.DropIndex(
                name: "IX_Episode_CaseManagerEmail",
                table: "Episode");

            migrationBuilder.DropColumn(
                name: "CaseManagerEmail",
                table: "Episode");

            migrationBuilder.AddColumn<string>(
                name: "CaseManager",
                table: "Episode",
                maxLength: 200,
                nullable: true);
        }
    }
}
