using ntbs_service.Services;
using Xunit;

namespace ntbs_service_unit_tests.Services
{
    public class LegacySearchBuilderTest
    {
        readonly LegacySearchBuilder builder;

        public LegacySearchBuilderTest()
        {
            builder = new LegacySearchBuilder();
        }

        [Fact]
        public void SearchById_ReturnsCorrectSqlQueryAndParameters()
        {
            var (sqlQuery, parameters) = ((ILegacySearchBuilder)builder.FilterById("1")).GetResult();

            Assert.Contains("WHERE dmg.OldNotificationId = @id OR n.GroupId = @id AND n.Source = 'LTBR' OR dmg.NhsNumber = @id", sqlQuery);
            Assert.Equal("1", parameters.id);
        }

        [Fact]
        public void SearchByFamilyName_ReturnsCorrectSqlQueryAndParameters()
        {
            var (sqlQuery, parameters) = ((ILegacySearchBuilder)builder.FilterByFamilyName("Smith")).GetResult();

            Assert.Contains("WHERE dmg.FamilyName LIKE @familyName", sqlQuery);
            Assert.Equal("%Smith%", parameters.familyName);
        }

        [Fact]
        public void SearchByFamilyNameAndId_ReturnsCorrectSqlQueryAndParameters()
        {
            var (sqlQuery, parameters) = ((ILegacySearchBuilder)builder.FilterById("1").FilterByFamilyName("Smith")).GetResult();

            Assert.Contains("WHERE dmg.OldNotificationId = @id OR n.GroupId = @id AND n.Source = 'LTBR' OR dmg.NhsNumber = @id", sqlQuery);
            Assert.Contains("AND dmg.FamilyName LIKE @familyName", sqlQuery);
            Assert.Equal("1", parameters.id);
            Assert.Equal("%Smith%", parameters.familyName);
        }

        [Fact]
        public void SearchByGivenName_ReturnsCorrectSqlQueryAndParameters()
        {
            var (sqlQuery, parameters) = ((ILegacySearchBuilder)builder.FilterByGivenName("Bob")).GetResult();

            Assert.Contains(@"WHERE dmg.GivenName LIKE @givenName", sqlQuery);
            Assert.Equal("%Bob%", parameters.givenName);
        }
    }
}
