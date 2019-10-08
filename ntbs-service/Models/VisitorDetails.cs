using ExpressiveAnnotations.Attributes;
using Microsoft.EntityFrameworkCore;

namespace ntbs_service.Models
{
    [Owned]
    public class VisitorDetails : ModelBase
    {
        public bool? HasVisitor { get; set; }


        // First country block
        [RequiredIf("!ShouldValidateFull || HasTravel == true")]
        public int? CountryId1 { get; set; }

        public virtual Country Country1 { get; set; }

        [RequiredIf("!ShouldValidateFull || HasTravel == true")]
        [AssertThat(@"StayLengthInMonths1.GetValueOrDefault() != 0
            (
                StayLengthInMonths1.GetValueOrDefault() 
                + StayLengthInMonths2.GetValueOrDefault()
                + StayLengthInMonths3.GetValueOrDefault()
            ) <= 24")]
        public int? StayLengthInMonths1 { get; set; }


        // Second country block
        [AssertThat("!ShouldValidateFull || CountryId1 != null")]
        [AssertThat("CountryId2 != CountryId1")]
        public int? CountryId2 { get; set; }

        public Country Country2 { get; set; }

        [AssertThat("!ShouldValidateFull || CountryId2 != null")]
        [AssertThat(@"StayLengthInMonths2.GetValueOrDefault() != 0
            (
                StayLengthInMonths1.GetValueOrDefault() 
                + StayLengthInMonths2.GetValueOrDefault()
                + StayLengthInMonths3.GetValueOrDefault()
            ) <= 24")]
        public int? StayLengthInMonths2 { get; set; }


        // Third country block
        [AssertThat("!ShouldValidateFull || CountryId2 != null")]
        [AssertThat("CountryId3 != CountryId1 && CountryId3 != CountryId2")]
        public int? CountryId3 { get; set; }

        public Country Country3 { get; set; }

        [AssertThat("!ShouldValidateFull || CountryId3 != null")]
        [AssertThat(@"StayLengthInMonths3.GetValueOrDefault() != 0
            (
                StayLengthInMonths1.GetValueOrDefault() 
                + StayLengthInMonths2.GetValueOrDefault()
                + StayLengthInMonths3.GetValueOrDefault()
            ) <= 24")]
        public int? StayLengthInMonths3 { get; set; }

    }
}