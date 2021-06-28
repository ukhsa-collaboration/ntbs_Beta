using System;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Validations;
using ntbs_service_unit_tests.Helpers;
using Xunit;

namespace ntbs_service_unit_tests.Models.Entities
{
    public class SocialContextBaseTest
    {
        public static TheoryData<DateTime?, DateTime?> ValidSocialContextDates => new TheoryData<DateTime?, DateTime?>
        {
            { new DateTime(2021, 06, 20), new DateTime(2021, 06, 21) },
            { new DateTime(2021, 06, 20), new DateTime(2021, 06, 20) },
            { new DateTime(2020, 12, 31), new DateTime(2021, 01, 01) },
            { new DateTime(2021, 06, 20), null },
            { null, new DateTime(2021, 06, 20) },
            { null, null }
        };

        [Theory]
        [MemberData(nameof(ValidSocialContextDates))]
        public void VenueIsValidWhenToDateLaterThanOrSameAsFromDate(DateTime? fromDate, DateTime? toDate)
        {
            // given
            var venue = ValidVenue();
            venue.DateFrom = fromDate;
            venue.DateTo = toDate;

            // when
            var results = ValidationHelper.ValidateObject(venue);

            // then
            Assert.Empty(results);
        }

        public static TheoryData<DateTime?, DateTime?> InvalidSocialContextDates => new TheoryData<DateTime?, DateTime?>
        {
            { new DateTime(2021, 06, 20), new DateTime(2021, 06, 19) },
            { new DateTime(2021, 01, 01), new DateTime(2020, 12, 31) }
        };

        [Theory]
        [MemberData(nameof(InvalidSocialContextDates))]
        public void VenueIsInvalidWhenToDateEarlierThanFromDate(DateTime? fromDate, DateTime? toDate)
        {
            // given
            var venue = ValidVenue();
            venue.DateFrom = fromDate;
            venue.DateTo = toDate;

            // when
            var results = ValidationHelper.ValidateObject(venue);

            // then
            Assert.Contains(results, r => r.ErrorMessage == ValidationMessages.VenueDateToShouldBeLaterThanDateFrom);
        }

        private static SocialContextVenue ValidVenue() =>
            new SocialContextVenue
            {
                Address = "123 Fake Street"
            };
    }
}
