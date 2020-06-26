using System;
using System.ComponentModel.DataAnnotations;
using EFAuditer;
using ExpressiveAnnotations.Attributes;
using Microsoft.EntityFrameworkCore;
using ntbs_service.Models.Enums;
using ntbs_service.Models.ReferenceEntities;
using ntbs_service.Models.Validations;

namespace ntbs_service.Models.Entities
{
    [Owned]
    public class TravelDetails : ModelBase, ITravelOrVisitorDetails, IOwnedEntityForAuditing
    {
        private const int MaxTotalLengthOfStay = 24;
        
        [Display(Name = "Has the patient travelled to one or more high TB incidence countries within last 24 months?")]
        public Status? HasTravel { get; set; }

        [Range(1, 50)]
        [RequiredIf(nameof(AnyCountrySupplied),
            ErrorMessage = ValidationMessages.TravelOrVisitTotalNumberOfCountriesRequired)]
        [AssertThat(nameof(TotalNumberGreaterOrEqualToInputCountries),
            ErrorMessage = ValidationMessages.TotalNumberOfCountriesTravelledToGreaterThanInputNumber)]
        [Display(Name = "total number of countries")]
        public int? TotalNumberOfCountries { get; set; }


        #region First country

        public int? Country1Id { get; set; }

        public virtual Country Country1 { get; set; }

        [Range(1, MaxTotalLengthOfStay)]
        [AssertThat("Country1Id != null", ErrorMessage = ValidationMessages.TravelOrVisitDurationHasCountry)]
        [AssertThat(nameof(TotalLengthWithinLimit), ErrorMessage = ValidationMessages.TravelTotalDurationWithinLimit)]
        [Display(Name = "duration of travel")]
        public int? StayLengthInMonths1 { get; set; }

        #endregion

        #region Second country

        [AssertThat("!ShouldValidateFull || Country1Id != null",
            ErrorMessage = ValidationMessages.TravelIsChronological)]
        [AssertThat("Country2Id != Country1Id", ErrorMessage = ValidationMessages.TravelUniqueCountry)]
        public int? Country2Id { get; set; }

        public Country Country2 { get; set; }

        [Range(1, MaxTotalLengthOfStay)]
        [AssertThat("Country2Id != null", ErrorMessage = ValidationMessages.TravelOrVisitDurationHasCountry)]
        [AssertThat(nameof(TotalLengthWithinLimit), ErrorMessage = ValidationMessages.TravelTotalDurationWithinLimit)]
        [Display(Name = "duration of travel")]
        public int? StayLengthInMonths2 { get; set; }

        #endregion

        #region Third country

        [AssertThat("!ShouldValidateFull || Country2Id != null",
            ErrorMessage = ValidationMessages.TravelIsChronological)]
        [AssertThat("Country3Id != Country1Id && Country3Id != Country2Id",
            ErrorMessage = ValidationMessages.TravelUniqueCountry)]
        public int? Country3Id { get; set; }

        public Country Country3 { get; set; }

        [Range(1, MaxTotalLengthOfStay)]
        [AssertThat("Country3Id != null", ErrorMessage = ValidationMessages.TravelOrVisitDurationHasCountry)]
        [AssertThat(nameof(TotalLengthWithinLimit), ErrorMessage = ValidationMessages.TravelTotalDurationWithinLimit)]
        [Display(Name = "duration of travel")]
        public int? StayLengthInMonths3 { get; set; }

        #endregion

        #region Helper properties for use in expressive annotations

        // ReSharper disable MemberCanBePrivate.Global
        public bool AnyCountrySupplied => Country1Id.HasValue || Country2Id.HasValue || Country3Id.HasValue;

        public bool TotalNumberGreaterOrEqualToInputCountries =>
            TotalNumberOfCountries >=
            Convert.ToInt32(Country1Id.HasValue) +
            Convert.ToInt32(Country2Id.HasValue) +
            Convert.ToInt32(Country3Id.HasValue);

        public bool TotalLengthWithinLimit =>
            MaxTotalLengthOfStay >= TotalDurationOfTravel;

        public int TotalDurationOfTravel =>
            Convert.ToInt32(StayLengthInMonths1) +
            Convert.ToInt32(StayLengthInMonths2) +
            Convert.ToInt32(StayLengthInMonths3);

        #endregion

        string IOwnedEntityForAuditing.RootEntityType => RootEntities.Notification;
    }
}
