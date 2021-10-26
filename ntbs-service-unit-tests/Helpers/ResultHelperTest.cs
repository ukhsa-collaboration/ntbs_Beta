using ntbs_service.Models.Enums;
using ntbs_service.Models.ReferenceEntities;
using Xunit;

namespace ntbs_service_tests.UnitTests.Helpers
{
    public class ResultHelperTest
    {
        [Theory]
        [InlineData(Result.Positive, ManualTestTypeId.Culture, true)]
        [InlineData(Result.Positive, ManualTestTypeId.ChestXRay, false)]
        [InlineData(Result.Negative, ManualTestTypeId.LineProbeAssay, true)]
        [InlineData(Result.Negative, ManualTestTypeId.ChestCT, false)]
        [InlineData(Result.Awaiting, ManualTestTypeId.ChestCT, true)]
        [InlineData(Result.Awaiting, ManualTestTypeId.Smear, true)]
        [InlineData(Result.ConsistentWithTbCavities, ManualTestTypeId.ChestCT, true)]
        [InlineData(Result.ConsistentWithTbCavities, ManualTestTypeId.Histology, false)]
        [InlineData(Result.ConsistentWithTbOther, ManualTestTypeId.ChestXRay, true)]
        [InlineData(Result.ConsistentWithTbOther, ManualTestTypeId.Pcr, false)]
        [InlineData(Result.NotConsistentWithTb, ManualTestTypeId.ChestCT, true)]
        [InlineData(Result.NotConsistentWithTb, ManualTestTypeId.Culture, false)]
        public void IsValidForTestType_ReturnsExpectedValue(Result result, ManualTestTypeId manualTestTypeId, bool expected)
        {
            // Act
            var isValid = result.IsValidForTestType((int)manualTestTypeId);

            // Assert
            Assert.Equal(expected, isValid);
        }
    }
}
