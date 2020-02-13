using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class RenameEpisodeTableToHospitalDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("EXEC sp_rename 'FK_Episode_Hospital_HospitalId', 'FK_HospitalDetails_Hospital_HospitalId'");
            migrationBuilder.Sql("EXEC sp_rename 'FK_Episode_Notification_NotificationId', 'FK_HospitalDetails_Notification_NotificationId'");
            migrationBuilder.Sql("EXEC sp_rename 'FK_Episode_TbService_TbServiceCode', 'FK_HospitalDetails_TbService_TbServiceCode'");
            migrationBuilder.Sql("EXEC sp_rename 'FK_Episode_User_CaseManagerUsername', 'FK_HospitalDetails_User_CaseManagerUsername'");
            migrationBuilder.Sql("EXEC sp_rename 'PK_Episode', 'PK_HospitalDetails'");
            
            migrationBuilder.Sql("EXEC sp_rename 'Episode.[IX_Episode_CaseManagerUsername]', 'IX_HospitalDetails_CaseManagerUsername', 'INDEX'");
            migrationBuilder.Sql("EXEC sp_rename 'Episode.[IX_Episode_TbServiceCode]', 'IX_HospitalDetails_TbServiceCode', 'INDEX'");
            migrationBuilder.Sql("EXEC sp_rename 'Episode.[IX_Episode_HospitalId]', 'IX_HospitalDetails_HospitalId', 'INDEX'");
            
            migrationBuilder.Sql("EXEC sp_rename 'Episode', 'HospitalDetails'");
            
            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("EXEC sp_rename 'HospitalDetails', 'Episode'");
            
            migrationBuilder.Sql("EXEC sp_rename 'FK_HospitalDetails_Hospital_HospitalId', 'FK_Episode_Hospital_HospitalId'");            
            migrationBuilder.Sql("EXEC sp_rename 'FK_HospitalDetails_Notification_NotificationId', 'FK_Episode_Notification_NotificationId'");
            migrationBuilder.Sql("EXEC sp_rename 'FK_HospitalDetails_TbService_TbServiceCode', 'FK_Episode_TbService_TbServiceCode'");
            migrationBuilder.Sql("EXEC sp_rename 'FK_HospitalDetails_User_CaseManagerUsername', 'FK_Episode_User_CaseManagerUsername'");
            migrationBuilder.Sql("EXEC sp_rename 'PK_HospitalDetails', 'PK_Episode'");

            migrationBuilder.Sql("EXEC sp_rename 'Episode.[IX_HospitalDetails_CaseManagerUsername]', 'IX_Episode_CaseManagerUsername', 'INDEX'");
            migrationBuilder.Sql("EXEC sp_rename 'Episode.[IX_HospitalDetails_TbServiceCode]', 'IX_Episode_TbServiceCode', 'INDEX'");
            migrationBuilder.Sql("EXEC sp_rename 'Episode.[IX_HospitalDetails_HospitalId]', 'IX_Episode_HospitalId', 'INDEX'");
        }
    }
}
