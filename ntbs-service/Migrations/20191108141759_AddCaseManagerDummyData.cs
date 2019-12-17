using Microsoft.EntityFrameworkCore.Migrations;
using ntbs_service.Models;
using ntbs_service.Models.Entities;

namespace ntbs_service.Migrations
{
    public partial class AddCaseManagerDummyData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "CaseManager",
                columns: new[] { nameof(User.Username), nameof(User.GivenName), nameof(User.FamilyName) },
                values: new object[,]
                {
                    {"Conor.Sheehan@ntbs.phe.com", "Conor", "Sheehan"},
                    {"Hereward.Mills@ntbs.phe.com", "Hereward", "Mills"},
                    {"Christian.McCaffery@ntbs.phe.com", "Christian", "McCaffery"},
                    {"Laura.Hughes@ntbs.phe.com", "Laura", "Hughes"},
                    {"Farman.Farmanov@ntbs.phe.com", "Farman", "Farmanov"},
                    {"Jan.Mikolajczak@ntbs.phe.com", "Jan", "Mikolajczak"},
                    {"Nancy.Pickering@ntbs.phe.com", "Nancy", "Pickering"},
                    {"Tehreem.Mohiyuddin@ntbs.phe.com", "Tehreem", "Mohiyuddin"},
                    {"Andy.Forrest@ntbs.phe.com", "Andy", "Forrest"},
                    {"Yingxin.Jiang@ntbs.phe.com", "Yingxin", "Jiang"},
                    {"pheNtbs_nhsUser1@ntbs.phe.com", "Jane", "Smith"},
                    {"pheNtbs_nhsUser2@ntbs.phe.com", "John", "Smith"},
                });

            // Add all currently mapped AD services to users whom are mapped to real people
            migrationBuilder.Sql("INSERT INTO [dbo].[CaseManagerTbService] ([TbServiceCode], [CaseManagerEmail])" +
                                 "SELECT [Code], [Email]" +
                                 "FROM [dbo].[TbService]" +
                                 "CROSS JOIN (VALUES ('Conor.Sheehan@ntbs.phe.com'), ('Hereward.Mills@ntbs.phe.com'), ('Christian.McCaffery@ntbs.phe.com'), ('Laura.Hughes@ntbs.phe.com'), ('Farman.Farmanov@ntbs.phe.com'),('Jan.Mikolajczak@ntbs.phe.com'),('Nancy.Pickering@ntbs.phe.com'),('Tehreem.Mohiyuddin@ntbs.phe.com'),('Andy.Forrest@ntbs.phe.com'),('Yingxin.Jiang@ntbs.phe.com')" +
                                 ") Emails (Email)" +
                                 "WHERE [ServiceAdGroup] IN ('Global.NIS.NTBS.Service_Abingdon', 'Global.NIS.NTBS.Service_Ashford', 'Global.NIS.NTBS.Service_Nottingham')");

            // Add test NHS user as a case manager to their currently mapped AD services
            migrationBuilder.Sql("INSERT INTO [dbo].[CaseManagerTbService] ([TbServiceCode], [CaseManagerEmail])" +
                                 "SELECT [Code], [Email]" +
                                 "FROM [dbo].[TbService]" +
                                 "CROSS JOIN (VALUES ('pheNtbs_nhsUser1@ntbs.phe.com')) Emails (Email)" +
                                 "WHERE [ServiceAdGroup] IN ('Global.NIS.NTBS.Service_Nottingham')");

            // Add test NHS user as a case manager to their currently mapped AD services
            migrationBuilder.Sql("INSERT INTO [dbo].[CaseManagerTbService] ([TbServiceCode], [CaseManagerEmail])" +
                                 "SELECT [Code], [Email]" +
                                 "FROM [dbo].[TbService]" +
                                 "CROSS JOIN (VALUES ('pheNtbs_nhsUser2@ntbs.phe.com')) Emails (Email)" +
                                 "WHERE [ServiceAdGroup] IN ('Global.NIS.NTBS.Service_Abingdon', 'Global.NIS.NTBS.Service_Ashford')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [CaseManagerTbService]");
            migrationBuilder.Sql("DELETE FROM [CaseManager]");
        }
    }
}
