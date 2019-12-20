using System;
using System.Collections.Generic;
using ntbs_service.Models;
using Xunit;
using ntbs_service.Models.Validations;
using System.Linq;

namespace ntbs_service_unit_tests.Models
{
    public class NhsNumberValidatingTest
    {

        [Fact]
        public void CheckNhsNumberValidationAttributeReturnsInvalid_ForIncorrectStringWithLetters() {
            var nhsNumber = "123ab";
            var attribute = new ValidNhsNumberAttribute();
            var result = attribute.IsValid(nhsNumber);

            Assert.False(result);
        }

        [Fact]
        public void CheckNhsNumberValidationAttributeReturnsInvalid_ForIncorrectLength() {
            var nhsNumber = "123";
            var attribute = new ValidNhsNumberAttribute();
            var result = attribute.IsValid(nhsNumber);

            Assert.False(result);
        }

        [Fact]
        public void CheckNhsNumberValidationAttributeReturnsValid_ForTestFirstDigit() {
            var nhsNumber = "9123456780";
            var attribute = new ValidNhsNumberAttribute();
            var result = attribute.IsValid(nhsNumber);

            Assert.True(result);
        }

        private static readonly List<string> ExcludedFirstDigitNumbers = new List<string>()
        {
            "0123456789",
            "1234567890",
            "2345678901"
        };

        public static IEnumerable<object[]> ScottishNhsNumbers()
        {
            return ExcludedFirstDigitNumbers.Select(number => new object[] { number });
        }

        [Theory, MemberData(nameof(ScottishNhsNumbers))]
        public void CheckNhsNumberValidationAttributeReturnsValid_ForExcludedFirstDigits(string nhsNumber) {
            var attribute = new ValidNhsNumberAttribute();
            var result = attribute.IsValid(nhsNumber);

            Assert.True(result);
        }

        private static readonly List<string> InvalidNhsNumbers = new List<string>()
        {
            "0123456789",
            "1234567890",
            "2345678901"
        };

        public static IEnumerable<object[]> GetInvalidNhsNumbers()
        {
            return ExcludedFirstDigitNumbers.Select(number => new object[] { number });
        }
        
        [Theory, MemberData(nameof(GetInvalidNhsNumbers))]
        public void CheckNhsNumberValidationAttributeReturnsInvalid_ForInvalidCheckDigits(string nhsNumber) {
            var attribute = new ValidNhsNumberAttribute();
            var result = attribute.IsValid(nhsNumber);

            Assert.False(result);
        }

        private static readonly List<string> ValidNhsNumbers = new List<string>()
        {
            "0123456789",
            "1234567890",
            "2345678901"
        };

        public static IEnumerable<object[]> GetValidNhsNumbers()
        {
            return ValidNhsNumbers.Select(number => new object[] { number });
        }

        [Theory, MemberData(nameof(GetValidNhsNumbers))]
        public void CheckNhsNumberValidationAttributeReturnsValid_ForValidCheckDigits(string nhsNumber) {
            var attribute = new ValidNhsNumberAttribute();
            var result = attribute.IsValid(nhsNumber);

            Assert.True(result);
        }
    }
}