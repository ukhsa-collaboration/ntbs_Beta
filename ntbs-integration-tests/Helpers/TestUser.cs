using System.Collections.Generic;
using ntbs_service.Models.Enums;

namespace ntbs_integration_tests.Helpers
{
    public interface ITestUser
    {
        string Username { get; }
        string DisplayName { get; }
        UserType Type { get; }
        IEnumerable<string> AdGroups { get; }
        // When setting the TbServiceCodes, make sure that the user's also have the corresponding AD groups,
        // as defined by the seed data in tbservice.csv. Otherwise, they still won't have access to the
        // TB Services.
        IEnumerable<string> TbServiceCodes { get; }
    }

    // Tech debt: Some of the AD groups which are hardcoded here should actually be taken from config.
    public class TestUser : ITestUser
    {
        public int Id { get; }
        public string Username { get; }
        public string DisplayName { get; }
        public UserType Type { get; }
        public IEnumerable<string> AdGroups { get; }
        public IEnumerable<string> TbServiceCodes { get; }

        private TestUser(
            int id,
            string username,
            string displayName,
            UserType type,
            IEnumerable<string> adGroups,
            IEnumerable<string> tbServiceCodes = null)
        {
            Id = id;
            Username = username;
            DisplayName = displayName;
            Type = type;
            AdGroups = adGroups;
            TbServiceCodes = tbServiceCodes ?? new List<string>();
        }

        public static IEnumerable<TestUser> GetAll() => new[]
        {
            NhsUserForAbingdonAndPermitted,
            NhsUserWithNoTbServices,
            PheUserWithPermittedPhecCode,
            NationalTeamUser,
            AbingdonCaseManager,
            AbingdonCaseManager2,
            Developer
        };

        public static TestUser NhsUserForAbingdonAndPermitted = new TestUser(
            1234,
            "abingdon@nhs.uk",
            "Abingdon Permitted",
            UserType.NhsUser,
            new[] { "Global.NIS.NTBS.Service_Abingdon", "Global.NIS.NTBS.Service_Ashford" },
            tbServiceCodes: new[]
            {
                Utilities.TBSERVICE_ABINGDON_COMMUNITY_HOSPITAL_ID,
                Utilities.PERMITTED_SERVICE_CODE
            });

        public static TestUser NhsUserWithNoTbServices = new TestUser(
            2345,
            "no-service@nhs.uk",
            "No Service",
            UserType.NhsUser,
            new string[] { });

        public static TestUser PheUserWithPermittedPhecCode = new TestUser(
            3456,
            "permitted-phec@phe.com",
            "Permitted Phec",
            UserType.PheUser,
            new[] { "Global.NIS.NTBS.Admin", "Global.NIS.NTBS.SoE" });

        public static TestUser NationalTeamUser = new TestUser(
            4567,
            "national-team@ntbs.com",
            "National Team",
            UserType.NationalTeam,
            new[] { "Global.NIS.NTBS.Admin", "Global.NIS.NTBS.NTS" });

        public static TestUser AbingdonCaseManager = new TestUser(
            5678,
            Utilities.CASEMANAGER_ABINGDON_EMAIL,
            "TestCase TestManager",
            UserType.NhsUser,
            new[] { "Global.NIS.NTBS.Service_Abingdon" },
            tbServiceCodes: new[] { Utilities.TBSERVICE_ABINGDON_COMMUNITY_HOSPITAL_ID });

        public static TestUser AbingdonCaseManager2 = new TestUser(
            6789,
            Utilities.CASEMANAGER_ABINGDON_EMAIL2,
            "TestCase TestManager",
            UserType.NhsUser,
            new[] { "Global.NIS.NTBS.Service_Abingdon" },
            tbServiceCodes: new[] { Utilities.TBSERVICE_ABINGDON_COMMUNITY_HOSPITAL_ID });

        public static TestUser Developer = new TestUser(
            7890,
            "Developer@ntbs.phe.com",
            "BaseTestCase BaseTestManager",
            UserType.NationalTeam,
            new[] { "Global.NIS.NTBS.NTS" });
    }
}
