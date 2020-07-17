using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class IncreaseCommentColumnSizeInSocialContexts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Details",
                table: "SocialContextVenue",
                newName: "DetailsOld");
            migrationBuilder.RenameColumn(
                name: "Details",
                table: "SocialContextAddress",
                newName: "DetailsOld");
            
            migrationBuilder.AddColumn<string>(
                name: "Details",
                table: "SocialContextVenue",
                maxLength: 250,
                nullable: true);
            migrationBuilder.AddColumn<string>(
                name: "Details",
                table: "SocialContextAddress",
                maxLength: 250,
                nullable: true);
                
            migrationBuilder.Sql(@"
                UPDATE SocialContextVenue
                SET Details = DetailsOld
            ");
            migrationBuilder.Sql(@"
                UPDATE SocialContextAddress
                SET Details = DetailsOld
            ");
            
            migrationBuilder.DropColumn("DetailsOld", "SocialContextVenue");
            migrationBuilder.DropColumn("DetailsOld", "SocialContextAddress");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Details",
                table: "SocialContextVenue",
                newName: "DetailsOld");
            migrationBuilder.RenameColumn(
                name: "Details",
                table: "SocialContextAddress",
                newName: "DetailsOld");
            
            migrationBuilder.AddColumn<string>(
                name: "Details",
                table: "SocialContextVenue",
                maxLength: 100,
                nullable: true);
            migrationBuilder.AddColumn<string>(
                name: "Details",
                table: "SocialContextAddress",
                maxLength: 100,
                nullable: true);
                
            migrationBuilder.Sql(@"
                UPDATE SocialContextVenue
                SET Details = DetailsOld
            ");
            migrationBuilder.Sql(@"
                UPDATE SocialContextAddress
                SET Details = DetailsOld
            ");
            
            migrationBuilder.DropColumn("DetailsOld", "SocialContextVenue");
            migrationBuilder.DropColumn("DetailsOld", "SocialContextAddress");
        }
    }
}
