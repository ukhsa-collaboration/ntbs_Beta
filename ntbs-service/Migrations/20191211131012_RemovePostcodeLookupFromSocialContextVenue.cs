using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class RemovePostcodeLookupFromSocialContextVenue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SocialContextVenue_PostcodeLookup_PostcodeToLookup",
                table: "SocialContextVenue");

            migrationBuilder.DropIndex(
                name: "IX_SocialContextVenue_PostcodeToLookup",
                table: "SocialContextVenue");

            migrationBuilder.DropColumn(
                name: "PostcodeToLookup",
                table: "SocialContextVenue");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PostcodeToLookup",
                table: "SocialContextVenue",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SocialContextVenue_PostcodeToLookup",
                table: "SocialContextVenue",
                column: "PostcodeToLookup",
                unique: true,
                filter: "[PostcodeToLookup] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_SocialContextVenue_PostcodeLookup_PostcodeToLookup",
                table: "SocialContextVenue",
                column: "PostcodeToLookup",
                principalTable: "PostcodeLookup",
                principalColumn: "Postcode",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
