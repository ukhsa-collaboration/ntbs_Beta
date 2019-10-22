using System;
using System.Collections.Generic;
using ntbs_service.Models;
using Xunit;

namespace ntbs_service_unit_tests.Models
{
    public class FormattedDateTest
    {
        public static IEnumerable<object[]> InvalidDates()
        {
            yield return new object[] { new FormattedDate() { Day = null, Month = "2", Year = "2000"}};
            yield return new object[] { new FormattedDate() { Day = "hello", Month = "2", Year = "2000"}};
            yield return new object[] { new FormattedDate() { Day = "31", Month = "2", Year = "2000"}};
        }

        [Theory, MemberData(nameof(InvalidDates))]
        public void IfDateUnparsable_CanConvertReturnsFalse(FormattedDate formattedDate)
        {
            // Act
            var canConvert = formattedDate.TryConvertToDateTime(out _);

            // Assert
            Assert.False(canConvert);
        }

        [Fact]
        public void IfDateParsable_CanConvertReturnsTrueAndDateTime()
        {
            // Arrange
            var formattedDate = new FormattedDate() { Day = "28", Month = "2", Year = "2000"};
            var expectedResult = new DateTime(2000, 2, 28);

            // Act
            var canConvert = formattedDate.TryConvertToDateTime(out DateTime? result);

            // Assert
            Assert.True(canConvert);
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> TestEmptyDates()
        {
            yield return new object[] { new FormattedDate() { Day = null, Month = "2", Year = "2000"}, false};
            yield return new object[] { new FormattedDate() { Day = "31", Month = "2", Year = "2000"}, false};
            yield return new object[] { new FormattedDate() { Day = null, Month = null, Year = null}, true};
            yield return new object[] { new FormattedDate() { Day = null, Month = "", Year = null}, true};
        }

        [Theory, MemberData(nameof(TestEmptyDates))]
        public void IsEmpty_ReturnsCorrectValue(FormattedDate formattedDate, bool expectedResult)
        {
            // Act
            var result = formattedDate.IsEmpty();

            // Assert
            Assert.Equal(expectedResult, result);
        }
    }
}