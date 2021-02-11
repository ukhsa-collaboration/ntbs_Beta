using ntbs_service.Helpers;
using Xunit;

namespace ntbs_service_unit_tests.Helpers
{
    public class NameFormattingHelperTest
    {
        [Theory]
        [InlineData(null, null)]
        [InlineData("", "")]
        [InlineData("Alice", "Alice")]
        [InlineData("John Smith", "John Smith")]
        [InlineData("A O'Double-Barrelled", "A O'Double-Barrelled")]
        [InlineData("Иван Петрович Сидоров", "Иван Петрович Сидоров")]
        [InlineData("  John Smith ", "John Smith")]
        [InlineData("DOE, Jane (TAUNTON AND SOMERSET NHS FOUNDATION TRUST)", "DOE, Jane")]
        [InlineData("DOE Jane (R54) SJUHNT", "DOE Jane")]
        public void FormatDisplayName_FormatsCorrectly(string initialDisplayName, string expectedFormattedName)
        {
            // Act
            var actualFormattedName = NameFormattingHelper.FormatDisplayName(initialDisplayName);

            // Assert
            Assert.Equal(expectedFormattedName, actualFormattedName);
        }
    }
}
