using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class UpdateSampleTbServiceMappings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //                          table, key column,   key value, change column, change value
            migrationBuilder.UpdateData("TBService", "Code", "TBS0040", "ServiceAdGroup", "Global.NIS.NTBS.Service_Hull");
            migrationBuilder.UpdateData("TBService", "Code", "TBS0107", "ServiceAdGroup", "Global.NIS.NTBS.Service_Leicester");
            migrationBuilder.UpdateData("TBService", "Code", "TBS0116", "ServiceAdGroup", "Global.NIS.NTBS.Service_Luton");
            migrationBuilder.UpdateData("TBService", "Code", "TBS0178", "ServiceAdGroup", "Global.NIS.NTBS.Service_Bolton");

            migrationBuilder.UpdateData("PHEC", "Code", "PHECNI", "AdGroup", "Global.NIS.NTBS.NI");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData("TBService", "Code", "TBS0040", "ServiceAdGroup", null);
            migrationBuilder.UpdateData("TBService", "Code", "TBS0107", "ServiceAdGroup", null);
            migrationBuilder.UpdateData("TBService", "Code", "TBS0116", "ServiceAdGroup", null);
            migrationBuilder.UpdateData("TBService", "Code", "TBS0178", "ServiceAdGroup", null);

            migrationBuilder.UpdateData("PHEC", "Code", "PHECNI", "AdGroup", null);
        }
    }
}
