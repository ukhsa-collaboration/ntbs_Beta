using ntbs_service.Helpers;
using Xunit;

namespace ntbs_service_unit_tests.Helpers
{
    public class UrlHelperTest
    {
        [Fact]
        public void SanitiseHttpsUrl_ReturnsInput_ForHttpsUrl()
        {
            // Arrange
            const string inputUrl = "https://ntbs.phe.nhs.uk/Region/E45000010";

            // Act
            var result = UrlHelper.SanitiseHttpsUrl(inputUrl);

            // Assert
            Assert.Equal(inputUrl, result);
        }

        [Fact]
        public void SanitiseHttpsUrl_ReturnsRoot_ForHttpUrl()
        {
            // Arrange
            const string inputUrl = "http://ntbs.phe.nhs.uk/Region/E45000010";

            // Act
            var result = UrlHelper.SanitiseHttpsUrl(inputUrl);

            // Assert
            Assert.Equal("/", result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void SanitiseHttpsUrl_ReturnsRoot_ForNullOrEmpty(string inputUrl)
        {
            // Act
            var result = UrlHelper.SanitiseHttpsUrl(inputUrl);

            // Assert
            Assert.Equal("/", result);
        }

        [Fact]
        public void SanitiseHttpsUrl_ReturnsRoot_ForInlineJavaScript()
        {
            // Arrange
            const string inputUrl = "javascript:window.alert(\"Hello there\")";

            // Act
            var result = UrlHelper.SanitiseHttpsUrl(inputUrl);

            // Assert
            Assert.Equal("/", result);
        }

        [Fact]
        public void SanitiseHttpsUrl_ReturnsSanitisedUrl_WhenUrlHasInvalidCharacters()
        {
            // Arrange
            const string inputUrl = "https://ntbs.phe.nhs.uk/Region/E45000010\"></a> <script><script> <a href=\"/";

            // Act
            var result = UrlHelper.SanitiseHttpsUrl(inputUrl);

            // Assert
            Assert.Equal(
                "https://ntbs.phe.nhs.uk/Region/E45000010%22%3E%3C/a%3E%20%3Cscript%3E%3Cscript%3E%20%3Ca%20href=%22/",
                result);
        }
    }
}
