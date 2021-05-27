using ntbs_service.Models.Entities;
using Xunit;

namespace ntbs_service_unit_tests.Models.Entities
{
    public class NotificationDisplayTest
    {
        [Theory]
        [InlineData("LTBR", "165-9", "16590", "165-9")]
        [InlineData("ETS", "165-9", "16590", "16590")]
        [InlineData(null, "165-9", "16590", "165-9")]
        [InlineData(null, null, "16590", "16590")]
        [InlineData(null, null, null, null)]
        public void NotificationCorrectlySetsLegacyId(string legacySource, string ltbrId, string etsId,
            string expectedLegacyId)
        {
            // Arrange
            var notification = new Notification
            {
                LTBRID = ltbrId,
                ETSID = etsId,
                LegacySource = legacySource
            };
            
            // Assert
            Assert.Equal(expectedLegacyId, notification.LegacyId);
        }
    }
}
