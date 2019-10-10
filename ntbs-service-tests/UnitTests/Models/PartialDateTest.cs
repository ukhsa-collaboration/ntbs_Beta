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
            yield return new object[] { new PartialDate() { Day = null, Month = "2", Year = null}};
        }

        [Theory, MemberData(nameof(InvalidDates))]
        public void IfDateHasNoYear_CanConvertReturnsFalse(PartialDate partialDate)
        {

            //Act
            var canConvert = partialDate.TryConvertToDateTimeRange(out _, out _);

            // Assert
            Assert.False(canConvert);
        }

        [Fact]
        public void ParseableYearOnlyDate_CanConvertReturnsTrueAndDateTimeRange()
        {
            // Arrange
            var partialDate = new PartialDate() { Day = null, Month = null, Year = "2000"};
            var expectedResultRangeStart = new DateTime(2000, 1, 1);
            var expectedResultRangeEnd = new DateTime(2001, 1, 1);
            DateTime? resultRangeStart;
            DateTime? resultRangeEnd;

            // Act
            var canConvert = partialDate.TryConvertToDateTimeRange(out resultRangeStart, out resultRangeEnd);

            // Assert
            Assert.True(canConvert);
            Assert.Equal(expectedResultRangeStart, resultRangeStart);
            Assert.Equal(expectedResultRangeEnd, resultRangeEnd);
        }

        [Fact]
        public void ParseableYearAndMonthDate_CanConvertReturnsTrueAndDateTimeRange()
        {
            // Arrange
            var partialDate = new PartialDate() { Day = null, Month = "2", Year = "2000"};
            var expectedResultRangeStart = new DateTime(2000, 2, 1);
            var expectedResultRangeEnd = new DateTime(2000, 3, 1);
            DateTime? resultRangeStart;
            DateTime? resultRangeEnd;

            // Act
            var canConvert = partialDate.TryConvertToDateTimeRange(out resultRangeStart, out resultRangeEnd);

            // Assert
            Assert.True(canConvert);
            Assert.Equal(expectedResultRangeStart, resultRangeStart);
            Assert.Equal(expectedResultRangeEnd, resultRangeEnd);
        }

        [Fact]
        public void ParseableFullDate_CanConvertReturnsTrueAndDateTimeRange()
        {
            // Arrange
            var partialDate = new PartialDate() { Day = "20", Month = "3", Year = "2000"};
            var expectedResultRangeStart = new DateTime(2000, 3, 20);
            var expectedResultRangeEnd = new DateTime(2000, 3, 21);
            DateTime? resultRangeStart;
            DateTime? resultRangeEnd;

            // Act
            var canConvert = partialDate.TryConvertToDateTimeRange(out resultRangeStart, out resultRangeEnd);

            // Assert
            Assert.True(canConvert);
            Assert.Equal(expectedResultRangeStart, resultRangeStart);
            Assert.Equal(expectedResultRangeEnd, resultRangeEnd);
        }
    }
}