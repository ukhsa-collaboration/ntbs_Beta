using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class RenameCaseManagerEmailToUsername : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CaseManagerTbService_CaseManager_CaseManagerEmail",
                table: "CaseManagerTbService");

            migrationBuilder.RenameColumn(
                name: "CaseManagerEmail",
                table: "CaseManagerTbService",
                newName: "CaseManagerUsername");

            migrationBuilder.AddForeignKey(
                name: "FK_CaseManagerTbService_User_CaseManagerUsername",
                table: "CaseManagerTbService",
                column: "CaseManagerUsername",
                principalTable: "User",
                principalColumn: "Username",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CaseManagerTbService_User_CaseManagerUsername",
                table: "CaseManagerTbService");

            migrationBuilder.RenameColumn(
                name: "CaseManagerUsername",
                table: "CaseManagerTbService",
                newName: "CaseManagerEmail");

            migrationBuilder.AddForeignKey(
                name: "FK_CaseManagerTbService_CaseManager_CaseManagerEmail",
                table: "CaseManagerTbService",
                column: "CaseManagerEmail",
                principalTable: "User",
                principalColumn: "Username",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
