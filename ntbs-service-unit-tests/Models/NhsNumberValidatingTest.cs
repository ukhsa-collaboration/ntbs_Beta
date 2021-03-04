using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ntbs_service.Models.Entities;
using Xunit;

namespace ntbs_service_unit_tests.Models
{
    public class NhsNumberValidatingTest
    {

        [Fact]
        public void CheckNhsNumberValidationAttributeReturnsInvalid_ForIncorrectStringWithLetters()
        {
            var nhsNumber = "123ab";
            var IsValid = ValidateNhsNumber(nhsNumber);
            Assert.False(IsValid);
        }

        [Fact]
        public void CheckNhsNumberValidationAttributeReturnsInvalid_ForIncorrectLength()
        {
            var nhsNumber = "123";
            var IsValid = ValidateNhsNumber(nhsNumber);
            Assert.False(IsValid);
        }

        [Fact]
        public void CheckNhsNumberValidationAttributeReturnsValid_ForTestFirstDigit()
        {
            var nhsNumber = "9123456780";
            var IsValid = ValidateNhsNumber(nhsNumber);
            Assert.True(IsValid);
        }

        public static IEnumerable<object[]> MonodigitNhsNumbers()
        {
            return new List<string>()
                {
                    "0000000000",
                    "1111111111",
                    "9999999999"
                }
                .Select(number => new object[] { number });
        }

        [Theory, MemberData(nameof(MonodigitNhsNumbers))]
        public void CheckNhsNumberValidationAttributeReturnsInvalid_ForMonodigitNumbers(string nhsNumber)
        {
            var IsValid = ValidateNhsNumber(nhsNumber);
            Assert.False(IsValid);
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
        public void CheckNhsNumberValidationAttributeReturnsValid_ForScottishFirstDigits(string nhsNumber)
        {
            var IsValid = ValidateNhsNumber(nhsNumber);
            Assert.True(IsValid);
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
        public void CheckNhsNumberValidationAttributeReturnsInvalid_ForInvalidCheckDigits(string nhsNumber)
        {
            var IsValid = ValidateNhsNumber(nhsNumber);
            Assert.False(IsValid);
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
        public void CheckNhsNumberValidationAttributeReturnsValid_ForValidCheckDigits(string nhsNumber)
        {
            var IsValid = ValidateNhsNumber(nhsNumber);
            Assert.True(IsValid);
        }

        public bool ValidateNhsNumber(string nhsNumber)
        {
            var target = new PatientDetails() { NhsNumber = nhsNumber };
            var context = new ValidationContext(target);
            var results = new List<ValidationResult>();
            return Validator.TryValidateObject(target, context, results, true);
        }
    }
}
