using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class AddSampleTbServiceAdGroups : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //                          table, key column,   key value, change column, change value
            migrationBuilder.UpdateData("TBService", "Code", "TBS0106", "PHECAdGroup","Global.NIS.NTBS.YH");
            migrationBuilder.UpdateData("TBService", "Code", "TBS0142", "PHECAdGroup", "Global.NIS.NTBS.EMS");
            migrationBuilder.UpdateData("TBService", "Code", "TBS0142", "ServiceAdGroup", "Global.NIS.NTBS.Service_Nottingham");
            migrationBuilder.UpdateData("TBService", "Code", "TBS0008", "ServiceAdGroup", "Global.NIS.NTBS.Service_Ashford");
            migrationBuilder.UpdateData("TBService", "Code", "TBS0001", "ServiceAdGroup", "Global.NIS.NTBS.Service_Abingdon");
        }
        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
