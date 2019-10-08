using System;
using System.Collections.Generic;
using ntbs_service.Models;
using Xunit;

namespace ntbs_service_tests.UnitTests.Models
{
    public class PartialDateTest
    {
        public static IEnumerable<object[]> InvalidDates()
        {
            yield return new object[] { new FormattedDate() { Day = null, Month = "2", Year = null}};
            yield return new object[] { new FormattedDate() { Day = "hello", Month = "2", Year = "2000"}};
            yield return new object[] { new FormattedDate() { Day = "31", Month = null, Year = "2000"}};
        }

        [Theory, MemberData(nameof(InvalidDates))]
        public void IfDateUnparseable_CanConvertReturnsFalse(PartialDate partialDate)
        {
            // Arrange
            DateTime? dateTimeRangeStart;
            DateTime? dateTimeRangeEnd;

            //Act
            var canConvert = partialDate.TryConvertToDateTimeRange(out dateTimeRangeStart, out dateTimeRangeEnd);

            // Assert
            Assert.False(canConvert);
        }

        [Fact]
        public void ParseableYearOnlyDate_CanConvertReturnsTrueAndDateTimeRange()
        {
            // Arrange
            var partialDate = new PartialDate() { Day = "", Month = "", Year = "2000"};
            var expectedResultRangeStart = new DateTime(2000, 2, 28);
            var expectedResultRangeEnd = new DateTime(2000, 2, 28);
            DateTime? resultRangeStart;
            DateTime? resultRangeEnd;

            // Act
            var canConvert = partialDate.TryConvertToDateTimeRange(out resultRangeStart, out resultRangeEnd);

            // Assert
            Assert.True(canConvert);
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void ParseableYearAndMonthDate_CanConvertReturnsTrueAndDateTimeRange()
        {
            // Arrange
            var formattedDate = new FormattedDate() { Day = "", Month = "2", Year = "2000"};
            var expectedResult = new DateTime(2000, 2, 28);
            DateTime? result;

            // Act
            var canConvert = formattedDate.TryConvertToDateTime(out result);

            // Assert
            Assert.True(canConvert);
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void ParseableFullDate_CanConvertReturnsTrueAndDateTimeRange()
        {
            // Arrange
            var formattedDate = new FormattedDate() { Day = "28", Month = "2", Year = "2000"};
            var expectedResult = new DateTime(2000, 2, 28);
            DateTime? result;

            // Act
            var canConvert = formattedDate.TryConvertToDateTime(out result);

            // Assert
            Assert.True(canConvert);
            Assert.Equal(expectedResult, result);
        }
    }
}