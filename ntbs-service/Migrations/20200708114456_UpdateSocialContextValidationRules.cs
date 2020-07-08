using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class UpdateSocialContextValidationRules : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SocialContextVenue_VenueType_VenueTypeId",
                table: "SocialContextVenue");

            migrationBuilder.AlterColumn<int>(
                name: "VenueTypeId",
                table: "SocialContextVenue",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "SocialContextVenue",
                maxLength: 40,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 40);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateFrom",
                table: "SocialContextVenue",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateTo",
                table: "SocialContextVenue",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldNullable: false);
            
            migrationBuilder.AlterColumn<DateTime>(
                name: "DateTo",
                table: "SocialContextAddress",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldNullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "SocialContextVenue",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 150);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateFrom",
                table: "SocialContextAddress",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "SocialContextAddress",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 150);

            migrationBuilder.AlterColumn<string>(
                name: "Postcode",
                table: "SocialContextAddress",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddForeignKey(
                name: "FK_SocialContextVenue_VenueType_VenueTypeId",
                table: "SocialContextVenue",
                column: "VenueTypeId",
                principalSchema: "ReferenceData",
                principalTable: "VenueType",
                principalColumn: "VenueTypeId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SocialContextVenue_VenueType_VenueTypeId",
                table: "SocialContextVenue");

            migrationBuilder.AlterColumn<int>(
                name: "VenueTypeId",
                table: "SocialContextVenue",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "SocialContextVenue",
                maxLength: 40,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 40,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateFrom",
                table: "SocialContextVenue",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateTo",
                table: "SocialContextVenue",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);
            
            migrationBuilder.AlterColumn<DateTime>(
                name: "DateTo",
                table: "SocialContextAddress",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);
            
            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "SocialContextVenue",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateFrom",
                table: "SocialContextAddress",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "SocialContextAddress",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Postcode",
                table: "SocialContextAddress",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SocialContextVenue_VenueType_VenueTypeId",
                table: "SocialContextVenue",
                column: "VenueTypeId",
                principalSchema: "ReferenceData",
                principalTable: "VenueType",
                principalColumn: "VenueTypeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
