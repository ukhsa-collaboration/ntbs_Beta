using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class FixRelationshipTypeToPostcodes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Looks like this index doesn't exist, in spite of what EF is convinced off
            // migrationBuilder.DropIndex(
            //     name: "IX_Patients_PostcodeToLookup",
            //     table: "Patients");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_PostcodeToLookup",
                table: "Patients",
                column: "PostcodeToLookup");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Patients_PostcodeToLookup",
                table: "Patients");

            // migrationBuilder.CreateIndex(
            //     name: "IX_Patients_PostcodeToLookup",
            //     table: "Patients",
            //     column: "PostcodeToLookup",
            //     unique: true,
            //     filter: "[PatientDetails_PostcodeToLookup] IS NOT NULL");
        }
    }
}
