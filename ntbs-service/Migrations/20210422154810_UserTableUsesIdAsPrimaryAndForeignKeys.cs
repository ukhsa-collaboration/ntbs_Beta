using Microsoft.EntityFrameworkCore.Migrations;
using ntbs_service.Models.Entities;

namespace ntbs_service.Migrations
{
    public partial class UserTableUsesIdAsPrimaryAndForeignKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop foreign and primary keys and indexes
            migrationBuilder.DropForeignKey(
                name: "FK_Alert_User_CaseManagerUsername",
                table: "Alert");

            migrationBuilder.DropForeignKey(
                name: "FK_CaseManagerTbService_User_CaseManagerUsername",
                table: "CaseManagerTbService");

            migrationBuilder.DropForeignKey(
                name: "FK_HospitalDetails_User_CaseManagerUsername",
                table: "HospitalDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_TreatmentEvent_User_CaseManagerUsername",
                table: "TreatmentEvent");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CaseManager",
                table: "User");
            
            migrationBuilder.CreateIndex(
                name: "IX_User_Username",
                table: "User",
                column: "Username",
                unique: true,
                filter: "[Username] IS NOT NULL");

            migrationBuilder.DropIndex(
                name: "IX_TreatmentEvent_CaseManagerUsername",
                table: "TreatmentEvent");

            migrationBuilder.DropIndex(
                name: "IX_HospitalDetails_CaseManagerUsername",
                table: "HospitalDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CaseManagerTbService",
                table: "CaseManagerTbService");

            migrationBuilder.DropIndex(
                name: "IX_Alert_CaseManagerUsername",
                table: "Alert");
            
            // Add new columns
            migrationBuilder.AddColumn<int>(
                    name: "Id",
                    table: "User",
                    type: "int",
                    nullable: false)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "CaseManagerId",
                table: "TreatmentEvent",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CaseManagerId",
                table: "HospitalDetails",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CaseManagerId",
                table: "CaseManagerTbService",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CaseManagerId",
                table: "Alert",
                type: "int",
                nullable: true);
            
            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
                column: "Id");
            
            // Update values of new columns
            migrationBuilder.Sql(
                @"UPDATE [CaseManagerTbService] SET CaseManagerId = (SELECT Id FROM [User] WHERE Username = CaseManagerUsername)");
            migrationBuilder.Sql(
                @"UPDATE [Alert] SET CaseManagerId = (SELECT Id FROM [User] WHERE Username = CaseManagerUsername)");
            migrationBuilder.Sql(
                @"UPDATE [HospitalDetails] SET CaseManagerId = (SELECT Id FROM [User] WHERE Username = CaseManagerUsername)");
            migrationBuilder.Sql(
                @"UPDATE [TreatmentEvent] SET CaseManagerId = (SELECT Id FROM [User] WHERE Username = CaseManagerUsername)");
            
            // Add foreign and primary keys and indexes
            migrationBuilder.AddPrimaryKey(
                name: "PK_CaseManagerTbService",
                table: "CaseManagerTbService",
                columns: new[] { "CaseManagerId", "TbServiceCode" });

            migrationBuilder.CreateIndex(
                name: "IX_TreatmentEvent_CaseManagerId",
                table: "TreatmentEvent",
                column: "CaseManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_HospitalDetails_CaseManagerId",
                table: "HospitalDetails",
                column: "CaseManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Alert_CaseManagerId",
                table: "Alert",
                column: "CaseManagerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Alert_User_CaseManagerId",
                table: "Alert",
                column: "CaseManagerId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CaseManagerTbService_User_CaseManagerId",
                table: "CaseManagerTbService",
                column: "CaseManagerId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HospitalDetails_User_CaseManagerId",
                table: "HospitalDetails",
                column: "CaseManagerId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TreatmentEvent_User_CaseManagerId",
                table: "TreatmentEvent",
                column: "CaseManagerId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            // Drop old columns
            migrationBuilder.DropColumn(
                name: "CaseManagerUsername",
                table: "TreatmentEvent");

            migrationBuilder.DropColumn(
                name: "CaseManagerUsername",
                table: "HospitalDetails");

            migrationBuilder.DropColumn(
                name: "CaseManagerUsername",
                table: "CaseManagerTbService");

            migrationBuilder.DropColumn(
                name: "CaseManagerUsername",
                table: "Alert");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CaseManagerUsername",
                table: "TreatmentEvent",
                type: "nvarchar(64)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CaseManagerUsername",
                table: "HospitalDetails",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CaseManagerUsername",
                table: "CaseManagerTbService",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CaseManagerUsername",
                table: "Alert",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: true);
            
            migrationBuilder.DropForeignKey(
                name: "FK_Alert_User_CaseManagerId",
                table: "Alert");

            migrationBuilder.DropForeignKey(
                name: "FK_CaseManagerTbService_User_CaseManagerId",
                table: "CaseManagerTbService");

            migrationBuilder.DropForeignKey(
                name: "FK_HospitalDetails_User_CaseManagerId",
                table: "HospitalDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_TreatmentEvent_User_CaseManagerId",
                table: "TreatmentEvent");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_Username",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_TreatmentEvent_CaseManagerId",
                table: "TreatmentEvent");

            migrationBuilder.DropIndex(
                name: "IX_HospitalDetails_CaseManagerId",
                table: "HospitalDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CaseManagerTbService",
                table: "CaseManagerTbService");

            migrationBuilder.DropIndex(
                name: "IX_Alert_CaseManagerId",
                table: "Alert");
            
            // Update values of new columns
            migrationBuilder.Sql(
                @"UPDATE [CaseManagerTbService] SET CaseManagerUsername = (SELECT Username FROM [User] WHERE Id = CaseManagerId)");
            migrationBuilder.Sql(
                @"UPDATE [Alert] SET CaseManagerUsername = (SELECT Username FROM [User] WHERE Id = CaseManagerId)");
            migrationBuilder.Sql(
                @"UPDATE [HospitalDetails] SET CaseManagerUsername = (SELECT Username FROM [User] WHERE Id = CaseManagerId)");
            migrationBuilder.Sql(
                @"UPDATE [TreatmentEvent] SET CaseManagerUsername = (SELECT Username FROM [User] WHERE Id = CaseManagerId)");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "User",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(64)",
                oldMaxLength: 64,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CaseManager",
                table: "User",
                column: "Username");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CaseManagerTbService",
                table: "CaseManagerTbService",
                columns: new[] { "CaseManagerUsername", "TbServiceCode" });

            migrationBuilder.CreateIndex(
                name: "IX_TreatmentEvent_CaseManagerUsername",
                table: "TreatmentEvent",
                column: "CaseManagerUsername");

            migrationBuilder.CreateIndex(
                name: "IX_HospitalDetails_CaseManagerUsername",
                table: "HospitalDetails",
                column: "CaseManagerUsername");

            migrationBuilder.CreateIndex(
                name: "IX_Alert_CaseManagerUsername",
                table: "Alert",
                column: "CaseManagerUsername");

            migrationBuilder.AddForeignKey(
                name: "FK_Alert_User_CaseManagerUsername",
                table: "Alert",
                column: "CaseManagerUsername",
                principalTable: "User",
                principalColumn: "Username",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CaseManagerTbService_User_CaseManagerUsername",
                table: "CaseManagerTbService",
                column: "CaseManagerUsername",
                principalTable: "User",
                principalColumn: "Username",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HospitalDetails_User_CaseManagerUsername",
                table: "HospitalDetails",
                column: "CaseManagerUsername",
                principalTable: "User",
                principalColumn: "Username",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TreatmentEvent_User_CaseManagerUsername",
                table: "TreatmentEvent",
                column: "CaseManagerUsername",
                principalTable: "User",
                principalColumn: "Username",
                onDelete: ReferentialAction.Restrict);
            
            migrationBuilder.DropColumn(
                name: "Id",
                table: "User");

            migrationBuilder.DropColumn(
                name: "CaseManagerId",
                table: "TreatmentEvent");

            migrationBuilder.DropColumn(
                name: "CaseManagerId",
                table: "HospitalDetails");

            migrationBuilder.DropColumn(
                name: "CaseManagerId",
                table: "CaseManagerTbService");

            migrationBuilder.DropColumn(
                name: "CaseManagerId",
                table: "Alert");
        }
    }
}
