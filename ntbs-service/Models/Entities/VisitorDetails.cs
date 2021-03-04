using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EFAuditer;
using ExpressiveAnnotations.Attributes;
using ntbs_service.Models.Enums;
using ntbs_service.Models.ReferenceEntities;
using ntbs_service.Models.Validations;

namespace ntbs_service.Models.Entities
{
    [NotMapped]
    [Display(Name = "Visitor details")]
    public class VisitorDetails : ModelBase, ITravelOrVisitorDetails, IOwnedEntityForAuditing
    {
        private const int MaxTotalLengthOfStay = 24;

        [Display(Name = "Has the patient received visitors from one or more high TB incidence countries within last 24 months?")]
        public Status? HasVisitor { get; set; }

        [Range(1, 50)]
        [RequiredIf(nameof(AnyCountrySupplied),
            ErrorMessage = ValidationMessages.TravelOrVisitTotalNumberOfCountriesRequired)]
        [AssertThat(nameof(TotalNumberGreaterOrEqualToInputCountries),
            ErrorMessage = ValidationMessages.TotalNumberOfCountriesVisitedFromGreaterThanInputNumber)]
        [Display(Name = "total number of countries")]
        public int? TotalNumberOfCountries { get; set; }


        #region First country

        public int? Country1Id { get; set; }

        public virtual Country Country1 { get; set; }

        [Range(1, MaxTotalLengthOfStay)]
        [AssertThat("Country1Id != null", ErrorMessage = ValidationMessages.TravelOrVisitDurationHasCountry)]
        [AssertThat(nameof(TotalLengthWithinLimit), ErrorMessage = ValidationMessages.VisitTotalDurationWithinLimit)]
        [Display(Name = "duration of visit")]
        public int? StayLengthInMonths1 { get; set; }

        #endregion

        #region Second country

        [AssertThat("!ShouldValidateFull || Country1Id != null",
            ErrorMessage = ValidationMessages.VisitIsChronological)]
        [AssertThat("Country2Id != Country1Id", ErrorMessage = ValidationMessages.VisitUniqueCountry)]
        public int? Country2Id { get; set; }

        public Country Country2 { get; set; }

        [Range(1, MaxTotalLengthOfStay)]
        [AssertThat("Country2Id != null", ErrorMessage = ValidationMessages.TravelOrVisitDurationHasCountry)]
        [AssertThat(nameof(TotalLengthWithinLimit), ErrorMessage = ValidationMessages.VisitTotalDurationWithinLimit)]
        [Display(Name = "duration of visit")]
        public int? StayLengthInMonths2 { get; set; }

        #endregion

        #region Third country

        [AssertThat("!ShouldValidateFull || Country2Id != null",
            ErrorMessage = ValidationMessages.VisitIsChronological)]
        [AssertThat("Country3Id != Country1Id && Country3Id != Country2Id",
            ErrorMessage = ValidationMessages.VisitUniqueCountry)]
        public int? Country3Id { get; set; }

        public Country Country3 { get; set; }

        [Range(1, MaxTotalLengthOfStay)]
        [AssertThat("Country3Id != null", ErrorMessage = ValidationMessages.TravelOrVisitDurationHasCountry)]
        [AssertThat(nameof(TotalLengthWithinLimit), ErrorMessage = ValidationMessages.VisitTotalDurationWithinLimit)]
        [Display(Name = "duration of visit")]
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
            MaxTotalLengthOfStay >= TotalDurationOfVisit;

        public int TotalDurationOfVisit =>
            Convert.ToInt32(StayLengthInMonths1) +
            Convert.ToInt32(StayLengthInMonths2) +
            Convert.ToInt32(StayLengthInMonths3);

        #endregion

        string IOwnedEntityForAuditing.RootEntityType => RootEntities.Notification;
    }
}
