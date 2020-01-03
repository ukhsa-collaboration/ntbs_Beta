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
        public ValidNhsNumberAttribute validationAttribute;
        public NhsNumberValidatingTest()
        {
            validationAttribute = new ValidNhsNumberAttribute();
        }

        [Fact]
        public void CheckNhsNumberValidationAttributeReturnsInvalid_ForIncorrectStringWithLetters() {
            var nhsNumber = "123ab";
            var result = validationAttribute.IsValid(nhsNumber);

            Assert.False(result);
        }

        [Fact]
        public void CheckNhsNumberValidationAttributeReturnsInvalid_ForIncorrectLength() {
            var nhsNumber = "123";
            var result = validationAttribute.IsValid(nhsNumber);

            Assert.False(result);
        }

        [Fact]
        public void CheckNhsNumberValidationAttributeReturnsValid_ForTestFirstDigit() {
            var nhsNumber = "9123456780";
            var result = validationAttribute.IsValid(nhsNumber);

            Assert.True(result);
        }

        public static IEnumerable<object[]> ScottishNhsNumbers()
        {
            return new List<string>()
            {
                "0123456789",
                "1234567890",
                "2345678901"
            }
            .Select(number => new object[] { number });
        }

        [Theory, MemberData(nameof(ScottishNhsNumbers))]
        public void CheckNhsNumberValidationAttributeReturnsValid_ForScottishFirstDigits(string nhsNumber) {
            var result = validationAttribute.IsValid(nhsNumber);

            Assert.True(result);
        }

        public static IEnumerable<object[]> GetInvalidNhsNumbers()
        {
            return new List<string>()
            {
                "4123456789",
                "5234567890",
                "6345678901"
            }
            .Select(number => new object[] { number });
        }
        
        [Theory, MemberData(nameof(GetInvalidNhsNumbers))]
        public void CheckNhsNumberValidationAttributeReturnsInvalid_ForInvalidCheckDigits(string nhsNumber) {
            var result = validationAttribute.IsValid(nhsNumber);

            Assert.False(result);
        }

        public static IEnumerable<object[]> GetValidNhsNumbers()
        {
            return new List<string>()
            {
                "6511195635",
                "8881441519",
                "5864552852"
            }
            .Select(number => new object[] { number });
        }

        [Theory, MemberData(nameof(GetValidNhsNumbers))]
        public void CheckNhsNumberValidationAttributeReturnsValid_ForValidCheckDigits(string nhsNumber) {
            var result = validationAttribute.IsValid(nhsNumber);

            Assert.True(result);
        }
    }
}