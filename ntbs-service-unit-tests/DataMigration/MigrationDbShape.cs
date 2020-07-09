using System;

// ReSharper disable ClassNeverInstantiated.Global

// DTO classes with low-level representations of the data coming in from the migration database
// These were created to match the state of the migration db's tables and views at the time of writing.
//
// The following script was useful for getting an up-to-date list of the columns:
// select * from (
//      select
//      	tab.name,
//      	column_id,
//          t.name as data_type,
//          col.name as column_name
//      from sys.tables as tab
//          inner join sys.columns as col
//              on tab.object_id = col.object_id
//          left join sys.types as t
//          on col.user_type_id = t.user_type_id
//      union
//      
//      select object_name(c.object_id) as name,
//      		column_id,
//             type_name(user_type_id) as data_type,
//             c.name as column_name
//      from sys.columns c
//      join sys.views v 
//           on v.object_id = c.object_id
//      
//  ) tablesAndViews
// 
// where name LIKE '' -- Insert name here
// 		 
// order by name, column_id
namespace ntbs_service_unit_tests.DataMigration
{
    public abstract class MigrationDbRecord
    {
        public string OldNotificationId { get; set; }
    }

    // MigrationNotificationsView
    public class MigrationDbNotification : MigrationDbRecord
    {
        public string Source { get; set; }
        public Guid? NtbsHospitalId { get; set; }
        public string EtsId { get; set; }
        public string LtbrId { get; set; }
        public string GroupId { get; set; }
        public DateTime? NotificationDate { get; set; }
        public int? IsDenotified { get; set; }
        public DateTime? DenotificationDate { get; set; }
        public string HivTestStatus { get; set; }
        public string Consultant { get; set; }
        public string CaseManager { get; set; }
        public string Notes { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public string Postcode { get; set; }
        public string FamilyName { get; set; }
        public string GivenName { get; set; }
        public string NhsNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public bool UkBorn { get; set; }
        public int? BirthCountryId { get; set; }
        public int? UkEntryYear { get; set; }
        public int? NtbsEthnicGroupId { get; set; }
        public int? NtbsSexId { get; set; }
        public int? NhsNumberNotKnown { get; set; }
        public int? NoFixedAbode { get; set; }
        public int? NtbsOccupationId { get; set; }
        public string OccupationFreetext { get; set; }
        public string LocalPatientId { get; set; }
        public DateTime? DeathDate { get; set; }
        public string HasVisitor { get; set; }
        public int? visitor_Country1 { get; set; }
        public int? visitor_Country2 { get; set; }
        public int? visitor_Country3 { get; set; }
        public string visitor_TotalNumberOfCountries { get; set; }
        public int? visitor_StayLengthInMonths1 { get; set; }
        public int? visitor_StayLengthInMonths2 { get; set; }
        public int? visitor_StayLengthInMonths3 { get; set; }
        public string HasTravel { get; set; }
        public int? travel_Country1 { get; set; }
        public int? travel_Country2 { get; set; }
        public int? travel_Country3 { get; set; }
        public string travel_TotalNumberOfCountries { get; set; }
        public int? travel_StayLengthInMonths1 { get; set; }
        public int? travel_StayLengthInMonths2 { get; set; }
        public int? travel_StayLengthInMonths3 { get; set; }
        public DateTime? SymptomOnsetDate { get; set; }
        public int? IsSymptomatic { get; set; }
        public DateTime? FirstPresentationDate { get; set; }
        public string HealthcareSetting { get; set; }
        public string HealthcareDescription { get; set; }
        public DateTime? FirstHomeVisitDate { get; set; }
        public string HomeVisitCarriedOut { get; set; }
        public DateTime? TbServicePresentationDate { get; set; }
        public DateTime? DiagnosisDate { get; set; }
        public DateTime? StartOfTreatmentDate { get; set; }
        public int? DidNotStartTreatment { get; set; }
        public int? IsPostMortem { get; set; }
        public string TreatmentRegimen { get; set; }
        public DateTime? MDRTreatmentStartDate { get; set; }
        public string DotStatus { get; set; }
        public string IsDotOffered { get; set; }
        public string EnhancedCaseManagementStatus { get; set; }
        public string BCGVaccinationState { get; set; }
        public int? BCGVaccinationYear { get; set; }
        public string DiabetesStatus { get; set; }
        public string HepatitisBStatus { get; set; }
        public string HepatitisCStatus { get; set; }
        public string LiverDiseaseStatus { get; set; }
        public string RenalDiseaseStatus { get; set; }
        public string ImmunosuppressionStatus { get; set; }
        public int? HasBioTherapy { get; set; }
        public int? HasTransplantation { get; set; }
        public int? HasOther { get; set; }
        public string OtherDescription { get; set; }
        public string ImmigrationDetaineeStatus { get; set; }
        public string AsylumSeekerStatus { get; set; }
        public string AlcoholMisuseStatus { get; set; }
        public string SmokingStatus { get; set; }
        public string MentalHealthStatus { get; set; }
        public string riskFactorDrugs_Status { get; set; }
        public int? riskFactorDrugs_IsCurrent { get; set; }
        public int? riskFactorDrugs_InPastFiveYears { get; set; }
        public int? riskFactorDrugs_MoreThanFiveYearsAgo { get; set; }
        public string riskFactorHomelessness_Status { get; set; }
        public int? riskFactorHomelessness_IsCurrent { get; set; }
        public int? riskFactorHomelessness_InPastFiveYears { get; set; }
        public int? riskFactorHomelessness_MoreThanFiveYearsAgo { get; set; }
        public string riskFactorImprisonment_Status { get; set; }
        public int? riskFactorImprisonment_IsCurrent { get; set; }
        public int? riskFactorImprisonment_InPastFiveYears { get; set; }
        public int? riskFactorImprisonment_MoreThanFiveYearsAgo { get; set; }
        public int? AdultsIdentified { get; set; }
        public int? ChildrenIdentified { get; set; }
        public int? AdultsScreened { get; set; }
        public int? ChildrenScreened { get; set; }
        public int? AdultsActiveTB { get; set; }
        public int? ChildrenActiveTB { get; set; }
        public int? AdultsLatentTB { get; set; }
        public int? ChildrenLatentTB { get; set; }
        public int? AdultsStartedTreatment { get; set; }
        public int? ChildrenStartedTreatment { get; set; }
        public int? AdultsFinishedTreatment { get; set; }
        public int? ChildrenFinishedTreatment { get; set; }
        public string PreviouslyHadTb { get; set; }
        public int? PreviousTbDiagnosisYear { get; set; }
        public string PreviouslyTreated { get; set; }
        public int? PreviousTreatmentCountryId { get; set; }
        public string mdr_ExposureToKnownTbCase { get; set; }
        public string mdr_RelationshipToCase { get; set; }
        public string mdr_CaseInUKStatus { get; set; }
        public string mdr_RelatedNotificationId { get; set; }
        public int? mdr_CountryId { get; set; }
    }

    // NotificationSite
    public class MigrationDbSite : MigrationDbRecord
    {
        public string Source { get; set; }
        public int? SiteId { get; set; }
        public string SiteDescription { get; set; }
        public string FreeTextUsedToDetermineSiteId { get; set; }
    }
    
    // ManualTestResults
    public class MigrationDbManualTest : MigrationDbRecord
    {
        public string Source { get; set; }
        public int? ManualTestTypeId { get; set; }
        public int? SampleTypeId { get; set; }
        public string Result { get; set; }
        public DateTime? TestDate { get; set; }
    }

    // MigrationSocialContextVenueView
    public class MigrationDbSocialContextVenue : MigrationDbRecord
    {
        public int? VenueTypeId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Postcode { get; set; }
        public string Frequency { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string Details { get; set; }
    }

    // MigrationSocialContextAddressView
    public class MigrationDbSocialContextAddress : MigrationDbRecord
    {
        public string Address { get; set; }
        public string Postcode { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string Details { get; set; }
    }

    // MigrationTransferEventsView
    public class MigrationDbTransferEvent : MigrationDbRecord
    {
        public DateTime? EventDate { get; set; }
        public string TreatmentEventType { get; set; }
        public Guid? HospitalId { get; set; }
        public string CaseManager { get; set; }
    }

    // MigrationTreatmentOutcomeEventsView
    public class MigrationDbOutcomeEvent : MigrationDbRecord
    {
        public DateTime? EventDate { get; set; }
        public string TreatmentEventType { get; set; }
        public int? TreatmentOutcomeId { get; set; }
        public string Note { get; set; }
        public string CaseManager { get; set; }
        public Guid? NtbsHospitalId { get; set; }
    }

    // MigrationMBovisAnimalExposureView
    public class MigrationDbMBovisAnimal : MigrationDbRecord
    {
        public int? YearOfExposure { get; set; }
        public string AnimalType { get; set; }
        public string Animal { get; set; }
        public string AnimalTbStatus { get; set; }
        public int? ExposureDuration { get; set; }
        public int? CountryId { get; set; }
        public string OtherDetails { get; set; }
    }

    // MigrationMBovisExposureToKnownCaseView
    public class MigrationDbMBovisKnownCase : MigrationDbRecord
    {
        public int? YearOfExposure { get; set; }
        public string ExposureSetting { get; set; }
        public int? ExposureNotificationId { get; set; }
        public string NotifiedToPheStatus { get; set; }
        public string OtherDetails { get; set; }
    }

    // MigrationMBovisOccupationExposuresView
    public class MigrationDbMBovisOccupation : MigrationDbRecord
    {
        public int? YearOfExposure { get; set; }
        public string OccupationSetting { get; set; }
        public int? OccupationDuration { get; set; }
        public int? CountryId { get; set; }
        public string OtherDetails { get; set; }
    }

    // MigrationMBovisUnpasteurisedMilkConsumptionView
    public class MigrationDbMBovisMilkConsumption : MigrationDbRecord
    {
        public int? YearOfConsumption { get; set; }
        public string MilkProductType { get; set; }
        public string ConsumptionFrequency { get; set; }
        public int? CountryId { get; set; }
        public string OtherDetails { get; set; }
    }
}
