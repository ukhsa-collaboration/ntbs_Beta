using System.Collections.Generic;
using ntbs_service.Services;
using Xunit;

namespace ntbs_service_unit_tests.Services
{
    public class SpecimenQueryHelperTest
    {
        public static IEnumerable<object[]> FormatEnumerableData =>
            new List<object[]>
            {
                new object[] {new List<string>(), string.Empty},
                new object[] {null, string.Empty},
                new object[] {new List<string> {"1", "2"}, "1,2"}
            };

        [Theory]
        [MemberData(nameof(FormatEnumerableData))]
        public void FormatEnumerableParams_FormatsParamsAsExpected(IEnumerable<string> input,
            string expectedOutput)
        {
            Assert.Equal(expectedOutput, SpecimenQueryHelper.FormatEnumerableParams(input));
        }
    }
}
