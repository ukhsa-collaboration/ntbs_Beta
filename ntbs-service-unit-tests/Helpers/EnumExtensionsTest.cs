using System.ComponentModel;
using ntbs_service.Helpers;
using Xunit;

namespace ntbs_service_tests.UnitTests.Helpers
{
    public class EnumExtensionsTest
    {

        private enum TestEnum
        {
            [DisplayName("display name")]
            withDisplayAttribute,
            withoutDisplayAttribute
        };

        [Fact]
        public void GetDisplayName_ReturnsNameFromDisplayAttribute()
        {
            // Act
            var displayName = TestEnum.withDisplayAttribute.GetDisplayName();

            // Assert
            Assert.Equal("display name", displayName);
        }

        [Fact]
        public void GetDisplayName_ReturnsEmptyStringIfNoDisplayAttribute()
        {
            // Act
            var displayName = TestEnum.withoutDisplayAttribute.GetDisplayName();

            // Assert
            Assert.Equal(string.Empty, displayName);
        }
    }
}
