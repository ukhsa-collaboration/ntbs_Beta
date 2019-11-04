using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class AddOccupationReferenceData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Occupation",
                columns: new[] { "OccupationId", "HasFreeTextField", "Role", "Sector" },
                values: new object[,]
                {
                    { 1, false, "Works with cattle", "Agricultural/animal care" },
                    { 26, false, "Retired", "Other" },
                    { 25, false, "Prisoner", "Other" },
                    { 24, false, "Housewife/househusband", "Other" },
                    { 23, false, "Child", "Other" },
                    { 22, false, "Other", "Social/prison service" },
                    { 21, false, "Social worker", "Social/prison service" },
                    { 20, false, "Probation officer", "Social/prison service" },
                    { 19, false, "Prison/detention official", "Social/prison service" },
                    { 18, false, "Homeless sector worker", "Social/prison service" },
                    { 17, false, "Other", "Laboratory/Pathology" },
                    { 16, false, "PM attendant", "Laboratory/Pathology" },
                    { 15, false, "Pathologist", "Laboratory/Pathology" },
                    { 14, false, "Microbiologist", "Laboratory/Pathology" },
                    { 13, false, "Laboratory staff", "Laboratory/Pathology" },
                    { 12, false, "Other", "Health care" },
                    { 11, false, "Nurse", "Health care" },
                    { 10, false, "Doctor", "Health care" },
                    { 9, false, "Dentist", "Health care" },
                    { 8, false, "Community care worker", "Health care" },
                    { 7, false, "Other", "Education" },
                    { 6, false, "Teacher incl. nursery", "Education" },
                    { 5, false, "Lecturer", "Education" },
                    { 4, false, "Full-time student", "Education" },
                    { 3, false, "Other", "Agricultural/animal care" },
                    { 2, false, "Works with wild animals", "Agricultural/animal care" },
                    { 27, false, "Unemployed", "Other" },
                    { 28, true, "Other", "Other" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Occupation",
                keyColumn: "OccupationId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Occupation",
                keyColumn: "OccupationId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Occupation",
                keyColumn: "OccupationId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Occupation",
                keyColumn: "OccupationId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Occupation",
                keyColumn: "OccupationId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Occupation",
                keyColumn: "OccupationId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Occupation",
                keyColumn: "OccupationId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Occupation",
                keyColumn: "OccupationId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Occupation",
                keyColumn: "OccupationId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Occupation",
                keyColumn: "OccupationId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Occupation",
                keyColumn: "OccupationId",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Occupation",
                keyColumn: "OccupationId",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Occupation",
                keyColumn: "OccupationId",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Occupation",
                keyColumn: "OccupationId",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Occupation",
                keyColumn: "OccupationId",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Occupation",
                keyColumn: "OccupationId",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Occupation",
                keyColumn: "OccupationId",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Occupation",
                keyColumn: "OccupationId",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Occupation",
                keyColumn: "OccupationId",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Occupation",
                keyColumn: "OccupationId",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Occupation",
                keyColumn: "OccupationId",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Occupation",
                keyColumn: "OccupationId",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Occupation",
                keyColumn: "OccupationId",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Occupation",
                keyColumn: "OccupationId",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Occupation",
                keyColumn: "OccupationId",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Occupation",
                keyColumn: "OccupationId",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Occupation",
                keyColumn: "OccupationId",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Occupation",
                keyColumn: "OccupationId",
                keyValue: 28);
        }
    }
}
