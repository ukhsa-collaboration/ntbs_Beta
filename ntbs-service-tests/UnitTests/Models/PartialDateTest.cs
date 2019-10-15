using System;
using System.Collections.Generic;
using ntbs_service.Models;
using Xunit;
using ntbs_service.Models.Validations;

namespace ntbs_service_tests.UnitTests.Models
{
    public class PartialDateTest
    {

        [Fact]
        public void IfDateHasNoYear_CanConvertReturnsFalse()
        {
            // Arrange
            var partialDate = new PartialDate() { Day = null, Month = "2", Year = null};

            // Act
            var canConvert = partialDate.TryConvertToDateTimeRange(out _, out _);

            // Assert
            Assert.False(canConvert);
        }

        public static IEnumerable<object[]> Dates()
        {
            yield return new object[] { 
                new PartialDate() { Day = null, Month = null, Year = "2000"}, 
                new DateTime(2000, 1, 1), 
                new DateTime(2001, 1, 1)
            };
            yield return new object[] { 
                new PartialDate() { Day = null, Month = "2", Year = "2000"},
                new DateTime(2000, 2, 1), 
                new DateTime(2000, 3, 1)
            };
            yield return new object[] { new PartialDate() { Day = "20", Month = "3", Year = "2000"}, new DateTime(2000, 3, 20), new DateTime(2000, 3, 21)};
        }

        [Theory, MemberData(nameof(Dates))]
        public void ParseableYearOnlyDate_CanConvertReturnsTrueAndDateTimeRange(PartialDate partialDate, DateTime expectedResultRangeStart, DateTime expectedResultRangeEnd)
        {
            // Arrange
            // Act
            var canConvert = partialDate.TryConvertToDateTimeRange(out DateTime? resultRangeStart, out DateTime? resultRangeEnd);

            // Assert
            Assert.True(canConvert);
            Assert.Equal(expectedResultRangeStart, resultRangeStart);
            Assert.Equal(expectedResultRangeEnd, resultRangeEnd);
        }

        [Fact]
        public void CheckPartialDateAttributeReturnsFalseForNoYear() {
            var partialDate = new PartialDate() {Day = "1", Month = "2", Year = null};
            var attribute = new ValidPartialDateAttribute();
            var result = attribute.IsValid(partialDate);

            Assert.False(result);
        }

        [Fact]
        public void CheckPartialDateAttributeReturnsFalseForDayButNoMonth() {
            var partialDate = new PartialDate() {Day = "1", Month = null, Year = "1990"};
            var attribute = new ValidPartialDateAttribute();
            var result = attribute.IsValid(partialDate);

            Assert.False(result);
        }
    }
}