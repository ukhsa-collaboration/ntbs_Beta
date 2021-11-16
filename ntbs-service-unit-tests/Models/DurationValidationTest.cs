using ntbs_service.Models.Validations;
using Xunit;

namespace ntbs_service_unit_tests.Models
{
    public class DurationValidationTest
    {
        [Theory]
        [InlineData("12", true)]
        [InlineData(" 12 ", true)]
        [InlineData("12-36", true)]
        [InlineData("12- 36", true)]
        [InlineData("11+", true)]
        [InlineData("11 +", true)]
        [InlineData("12 55 ", false)]
        [InlineData("12-36-12", false)]
        [InlineData("10-", false)]
        [InlineData("11+1", false)]
        [InlineData("a long time", false)]
        public void DurationIsValidCheck_ReturnsExpectedValue(string duration, bool expected)
        {
            // Arrange
            var attribute = new ValidDurationAttribute();

            // Act
            var result = attribute.IsValid(duration);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
