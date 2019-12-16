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
        public void SearchById_ReturnsMatchOnNotificationId()
        {
            var (sqlQuery, parameters) = ((ILegacySearchBuilder)builder.FilterById("1")).GetResult();

            Assert.Equal("WHERE n.OldNotificationId = @id OR n.GroupId = @id AND n.Source = 'LTBR' OR dmg.NhsNumber = @id", sqlQuery);
            Assert.Equal("1", parameters.id);
        }
    }
}
