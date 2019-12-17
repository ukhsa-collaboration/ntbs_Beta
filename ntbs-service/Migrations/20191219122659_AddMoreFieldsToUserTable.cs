using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class AddMoreFieldsToUserTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Alert_CaseManager_CaseManagerEmail",
                table: "Alert");

            migrationBuilder.DropForeignKey(
                name: "FK_Episode_CaseManager_CaseManagerEmail",
                table: "Episode");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "User",
                newName: "Username");

            migrationBuilder.RenameColumn(
                name: "CaseManagerEmail",
                table: "Episode",
                newName: "CaseManagerUsername");

            migrationBuilder.RenameIndex(
                name: "IX_Episode_CaseManagerEmail",
                table: "Episode",
                newName: "IX_Episode_CaseManagerUsername");

            migrationBuilder.RenameColumn(
                name: "CaseManagerEmail",
                table: "Alert",
                newName: "CaseManagerUsername");

            migrationBuilder.RenameIndex(
                name: "IX_Alert_CaseManagerEmail",
                table: "Alert",
                newName: "IX_Alert_CaseManagerUsername");

            migrationBuilder.AddColumn<string>(
                name: "AdGroup",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DisplayName",
                table: "User",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "User",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsCaseManager",
                table: "User",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Alert_User_CaseManagerUsername",
                table: "Alert",
                column: "CaseManagerUsername",
                principalTable: "User",
                principalColumn: "Username",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Episode_User_CaseManagerUsername",
                table: "Episode",
                column: "CaseManagerUsername",
                principalTable: "User",
                principalColumn: "Username",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Alert_User_CaseManagerUsername",
                table: "Alert");

            migrationBuilder.DropForeignKey(
                name: "FK_Episode_User_CaseManagerUsername",
                table: "Episode");

            migrationBuilder.DropColumn(
                name: "AdGroup",
                table: "User");

            migrationBuilder.DropColumn(
                name: "DisplayName",
                table: "User");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "User");

            migrationBuilder.DropColumn(
                name: "IsCaseManager",
                table: "User");

            migrationBuilder.RenameColumn(
                name: "Username",
                table: "User",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "CaseManagerUsername",
                table: "Episode",
                newName: "CaseManagerEmail");

            migrationBuilder.RenameIndex(
                name: "IX_Episode_CaseManagerUsername",
                table: "Episode",
                newName: "IX_Episode_CaseManagerEmail");

            migrationBuilder.RenameColumn(
                name: "CaseManagerUsername",
                table: "Alert",
                newName: "CaseManagerEmail");

            migrationBuilder.RenameIndex(
                name: "IX_Alert_CaseManagerUsername",
                table: "Alert",
                newName: "IX_Alert_CaseManagerEmail");

            migrationBuilder.AddForeignKey(
                name: "FK_Alert_CaseManager_CaseManagerEmail",
                table: "Alert",
                column: "CaseManagerEmail",
                principalTable: "User",
                principalColumn: "Email",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Episode_CaseManager_CaseManagerEmail",
                table: "Episode",
                column: "CaseManagerEmail",
                principalTable: "User",
                principalColumn: "Email",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
