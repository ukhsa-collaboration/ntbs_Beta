using ntbs_service.Models.Validations;
using Xunit;

namespace ntbs_service_unit_tests.Models
{
    public class ContainsNoTabsValidatingTest
    {
        [Fact]
        public void EmptyString_IsValid()
        {
            Assert.True(ValidateContainsNoTabs(""));
        }

        [Fact]
        public void NullString_IsValid()
        {
            Assert.True(ValidateContainsNoTabs(null));
        }

        [Fact]
        public void StringWithNormalText_IsValid()
        {
            Assert.True(ValidateContainsNoTabs("Hello there"));
        }

        [Fact]
        public void StringWithOtherWhitespace_IsValid()
        {
            Assert.True(ValidateContainsNoTabs("Hello there\r\nGeneral Kenobi!"));
        }

        [Fact]
        public void StringWithJustTabs_IsInvalid()
        {
            Assert.False(ValidateContainsNoTabs("\t\t"));
        }

        [Fact]
        public void StringWithNormalTextAndTabs_IsInvalid()
        {
            Assert.False(ValidateContainsNoTabs("Hello there\tGeneral Kenobi!"));
        }

        private static bool ValidateContainsNoTabs(string notes)
        {
            var validator = new ContainsNoTabsAttribute();

            return validator.IsValid(notes);
        }
    }
}
