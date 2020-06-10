using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class AddContactDetailsToUsersTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "User",
                maxLength: 500,
                nullable: true);
            
            migrationBuilder.AddColumn<string>(
                name: "EmailPrimary",
                table: "User",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmailSecondary",
                table: "User",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JobTitle",
                table: "User",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumberPrimary",
                table: "User",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumberSecondary",
                table: "User",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Notes",
                table: "User");

            migrationBuilder.DropColumn(
                name: "EmailPrimary",
                table: "User");

            migrationBuilder.DropColumn(
                name: "EmailSecondary",
                table: "User");

            migrationBuilder.DropColumn(
                name: "JobTitle",
                table: "User");

            migrationBuilder.DropColumn(
                name: "PhoneNumberPrimary",
                table: "User");

            migrationBuilder.DropColumn(
                name: "PhoneNumberSecondary",
                table: "User");
        }
    }
}
