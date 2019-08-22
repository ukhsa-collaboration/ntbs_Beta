using System;
using System.Collections.Generic;
using ntbs_service.Helpers;
using Xunit;

namespace ntbs_service_tests.UnitTests.Helpers
{
    public class ValidationHelperTest
    {
        public static IEnumerable<object[]> ValidDates()
        {
            yield return new object[] { new DateTime(1900, 1, 1) };
            yield return new object[] { new DateTime(1967, 4, 1) };
            yield return new object[] { DateTime.Now };
        }

        public static IEnumerable<object[]> InvalidDates()
        {
            yield return new object[] { new DateTime(1789, 7, 14) };
            yield return new object[] { DateTime.Now.AddDays(1) };
        }

        [Theory, MemberData(nameof(ValidDates))]
        public void DatesWithinValidityRange_AreValid(DateTime dateTime)
        {
            // Act
            var isValid = DateValidationHelper.IsValidDate(dateTime);

            // Assert
            Assert.True(isValid);
        }

        [Theory, MemberData(nameof(InvalidDates))]
        public void DatesOutsideValidityRange_AreInvalid(DateTime dateTime)
        {
            // Act
            var isValid = DateValidationHelper.IsValidDate(dateTime);

            // Assert
            Assert.False(isValid);
        }
    }
}