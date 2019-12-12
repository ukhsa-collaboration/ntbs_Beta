using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class AddSocialContextVenues : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VenueType",
                columns: table => new
                {
                    VenueTypeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Category = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VenueType", x => x.VenueTypeId);
                });

            migrationBuilder.CreateTable(
                name: "SocialContextVenue",
                columns: table => new
                {
                    SocialContextVenueId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NotificationId = table.Column<int>(nullable: false),
                    VenueTypeId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 40, nullable: false),
                    Address = table.Column<string>(maxLength: 150, nullable: false),
                    Postcode = table.Column<string>(nullable: true),
                    PostcodeToLookup = table.Column<string>(nullable: true),
                    Frequency = table.Column<string>(maxLength: 30, nullable: true),
                    DateFrom = table.Column<DateTime>(nullable: false),
                    DateTo = table.Column<DateTime>(nullable: false),
                    Details = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocialContextVenue", x => x.SocialContextVenueId);
                    table.ForeignKey(
                        name: "FK_SocialContextVenue_Notification_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "Notification",
                        principalColumn: "NotificationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SocialContextVenue_PostcodeLookup_PostcodeToLookup",
                        column: x => x.PostcodeToLookup,
                        principalTable: "PostcodeLookup",
                        principalColumn: "Postcode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SocialContextVenue_VenueType_VenueTypeId",
                        column: x => x.VenueTypeId,
                        principalTable: "VenueType",
                        principalColumn: "VenueTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "VenueType",
                columns: new[] { "VenueTypeId", "Category", "Name" },
                values: new object[,]
                {
                    { 1, "Workplace", "Armed forces" },
                    { 66, "Residential", "Homeless shelter" },
                    { 65, "Residential", "Dispersal accommodation " },
                    { 64, "Residential", "Initial accommodation centre " },
                    { 63, "Health and care", "Crisis centre or refuge" },
                    { 62, "Treatment and rehab", "Other treatment or rehab centre" },
                    { 61, "Treatment and rehab", "Mental health rehabilitation centre" },
                    { 60, "Treatment and rehab", "Medical or physical rehabilitation centre" },
                    { 59, "Treatment and rehab", "Drug rehabilitation centre" },
                    { 58, "Treatment and rehab", "Alcohol rehabilitation centre" },
                    { 57, "Place of detention", "Other place of detention" },
                    { 56, "Place of detention", "Youth detention centre" },
                    { 55, "Place of detention", "Prison" },
                    { 54, "Place of detention", "Immigration detention centre" },
                    { 53, "Childcare & education", "Other childcare & education" },
                    { 52, "Childcare & education", "Religious learning centre" },
                    { 51, "Childcare & education", "Private tutoring" },
                    { 50, "Childcare & education", "Adult education" },
                    { 49, "Childcare & education", "University" },
                    { 48, "Childcare & education", "After school clubs" },
                    { 67, "Residential", "Squat" },
                    { 47, "Childcare & education", "College or sixth form" },
                    { 68, "Residential", "Care home" },
                    { 70, "Residential", "Hostel" },
                    { 89, "Transport", "Boat" },
                    { 88, "Transport", "Taxi" },
                    { 87, "Transport", "Plane" },
                    { 86, "Transport", "Metro" },
                    { 85, "Transport", "Tram" },
                    { 84, "Transport", "Train" },
                    { 83, "Transport", "Bus" },
                    { 82, "Transport", "Car" },
                    { 81, "Health and care", "Other heathcare" },
                    { 80, "Health and care", "Health Centre/Clinic" },
                    { 79, "Health and care", "Hospice" },
                    { 78, "Health and care", "Nursing Home" },
                    { 77, "Health and care", "GP Practice" },
                    { 76, "Health and care", "Pharmacy" },
                    { 75, "Health and care", "Walk-in Centre/Minor Injuries" },
                    { 74, "Health and care", "Hospital" },
                    { 73, "Residential", "Other residential" },
                    { 72, "Residential", "Sofa surfing" },
                    { 71, "Residential", "Hall of residence" },
                    { 69, "Residential", "Halfway house" },
                    { 90, "Transport", "Cruise ship" },
                    { 46, "Childcare & education", "Secondary school" },
                    { 44, "Childcare & education", "Nursery" },
                    { 20, "Place of worship", "Church" },
                    { 19, "Workplace", "Other workplace" },
                    { 18, "Workplace", "Recreational centre" },
                    { 17, "Workplace", "Health club or spa" },
                    { 16, "Workplace", "Hair/beauty salon" },
                    { 15, "Workplace", "Warehouse" },
                    { 14, "Workplace", "Retail" },
                    { 13, "Workplace", "Hospitality" },
                    { 12, "Workplace", "Restaurant or cafe" },
                    { 11, "Workplace", "Pub, bar or club" },
                    { 10, "Workplace", "Office" },
                    { 9, "Workplace", "Hospital or medical centre" },
                    { 8, "Workplace", "Farming" },
                    { 7, "Workplace", "Factory" },
                    { 6, "Workplace", "Emergency services" },
                    { 5, "Workplace", "Education" },
                    { 4, "Workplace", "Driving" },
                    { 3, "Workplace", "Construction" },
                    { 2, "Workplace", "Catering" },
                    { 21, "Place of worship", "Temple" },
                    { 45, "Childcare & education", "Primary school" },
                    { 22, "Place of worship", "Mosque" },
                    { 24, "Place of worship", "Multi-faith centre" },
                    { 43, "Childcare & education", "Pre-school or play group" },
                    { 42, "Social", "Other social venue" },
                    { 41, "Social", "Friends house" },
                    { 40, "Social", "Crack house/smoking den" },
                    { 39, "Social", "Job/unemployment centre" },
                    { 38, "Social", "Community centre" },
                    { 37, "Social", "Music classes" },
                    { 36, "Social", "Recreational centre" },
                    { 35, "Social", "Exercise class" },
                    { 34, "Social", "Health club or spa" },
                    { 33, "Social", "Hair/beauty salon" },
                    { 32, "Social", "Shopping centre" },
                    { 31, "Social", "Cinema" },
                    { 30, "Social", "Library" },
                    { 29, "Social", "Restaurant or cafe" },
                    { 28, "Social", "Pub, bar or club" },
                    { 27, "Social", "Arcade/gambling venue" },
                    { 26, "Place of worship", "Other place of worship" },
                    { 25, "Place of worship", "Synagogue" },
                    { 23, "Place of worship", "Community centre" },
                    { 91, "Transport", "Other transport" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_SocialContextVenue_NotificationId",
                table: "SocialContextVenue",
                column: "NotificationId");

            migrationBuilder.CreateIndex(
                name: "IX_SocialContextVenue_PostcodeToLookup",
                table: "SocialContextVenue",
                column: "PostcodeToLookup",
                unique: true,
                filter: "[PostcodeToLookup] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_SocialContextVenue_VenueTypeId",
                table: "SocialContextVenue",
                column: "VenueTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SocialContextVenue");

            migrationBuilder.DropTable(
                name: "VenueType");
        }
    }
}
