using ntbs_service.Helpers;
using ntbs_service.Models.Enums;
using Xunit;

namespace ntbs_service_unit_tests.Helpers
{
    public class SanitizeHelperTest
    {
        [Fact]
        public void SanitizeExtensionMethodRemovesCurlyBrackets()
        {
            // Arrange
            string input = "{{abc}ef}g}}h{}";

            // Act
            string result = input.Sanitize();

            // Assert
            Assert.Equal("abcefgh", result);
        }
    }
}
