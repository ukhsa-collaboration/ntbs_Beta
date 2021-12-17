using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using EFAuditer;
using ExpressiveAnnotations.Attributes;
using ntbs_service.Helpers;
using ntbs_service.Models.Entities.Alerts;
using ntbs_service.Models.Enums;
using ntbs_service.Models.Projections;
using ntbs_service.Models.Validations;

namespace ntbs_service.Models.Entities
{
    // ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
    [Display(Name = "Notification")]
    public partial class Notification : ModelBase, IOwnedEntityForAuditing, INotificationForDrugResistanceImport
    {
        [SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
        public Notification()
        {
            NotificationStatus = NotificationStatus.Draft;
            PatientDetails = new PatientDetails();
            HospitalDetails = new HospitalDetails();
            SocialRiskFactors = new SocialRiskFactors();
            ClinicalDetails = new ClinicalDetails();
            PreviousTbHistory = new PreviousTbHistory();
            ContactTracing = new ContactTracing();
            ImmunosuppressionDetails = new ImmunosuppressionDetails();
            TravelDetails = new TravelDetails();
            VisitorDetails = new VisitorDetails();
            ComorbidityDetails = new ComorbidityDetails();
            MDRDetails = new MDRDetails();
            TestData = new TestData();
            DrugResistanceProfile = new DrugResistanceProfile();
            MBovisDetails = new MBovisDetails();
        }

        #region DB Mapped Fields

        [Display(Name = "NTBS Id")]
        public int NotificationId { get; set; }
        [MaxLength(10)]
        public string LegacySource { get; set; }
        [MaxLength(50)]
        public string ETSID { get; set; }
        // For LTBR records, this contains the first segment of their legacy id, which corresponds to the "patient" entity in LTBR.
        // E.g. records with LTBRIDs 123-1 and 123-2 belong to the same patient, with LTBRPatientId 123.
        // These are all legacy values and are not actively used by the NTBS system.
        [MaxLength(50)]
        public string LTBRPatientId { get; set; }
        [MaxLength(50)]
        public string LTBRID { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? SubmissionDate { get; set; }
        [Display(Name = "Deletion reason")]
        [MaxLength(150)]
        public string DeletionReason { get; set; }
        public int? GroupId { get; set; }
        [Display(Name = "Cluster Id")]
        public string ClusterId { get; set; }

        [Display(Name = "Notification date")]
        [RequiredIf(@"ShouldValidateFull", ErrorMessage = ValidationMessages.FieldRequired)]
        [AssertThat(@"PatientDetails.Dob == null || NotificationDate > PatientDetails.Dob", ErrorMessage = ValidationMessages.DateShouldBeLaterThanDob)]
        [ValidClinicalDate]
        public DateTime? NotificationDate { get; set; }
        public NotificationStatus NotificationStatus { get; set; }

        #endregion

        #region Navigation Properties

        [AssertThat("NotificationSites.Count > 0 || !ShouldValidateFull", ErrorMessage = ValidationMessages.DiseaseSiteIsRequired)]
        [ValidationChildEnumerable]
        public virtual List<NotificationSite> NotificationSites { get; set; }
        [ValidationChild]
        public virtual PatientDetails PatientDetails { get; set; }
        [ValidationChild]
        public virtual ClinicalDetails ClinicalDetails { get; set; }
        [ValidationChild]
        public virtual HospitalDetails HospitalDetails { get; set; }
        [ValidationChild]
        public virtual PreviousTbHistory PreviousTbHistory { get; set; }
        [ValidationChild]
        public virtual ContactTracing ContactTracing { get; set; }
        [ValidationChild]
        public virtual SocialRiskFactors SocialRiskFactors { get; set; }
        [ValidationChild]
        public virtual ImmunosuppressionDetails ImmunosuppressionDetails { get; set; }
        [ValidationChild]
        public virtual TravelDetails TravelDetails { get; set; }
        [ValidationChild]
        public virtual VisitorDetails VisitorDetails { get; set; }
        [ValidationChild]
        public virtual DenotificationDetails DenotificationDetails { get; set; }
        [ValidationChild]
        public virtual ComorbidityDetails ComorbidityDetails { get; set; }
        [ValidationChild]
        public virtual MDRDetails MDRDetails { get; set; }
        [ValidationChild]
        public virtual NotificationGroup Group { get; set; }
        [ValidationChild]
        public virtual TestData TestData { get; set; }
        [ValidationChild]
        public virtual MBovisDetails MBovisDetails { get; set; }
        [ValidationChildEnumerable]
        public virtual ICollection<Alert> Alerts { get; set; }
        [Display(Name = "Social context - venues")]
        [ValidationChildEnumerable]
        public virtual ICollection<SocialContextVenue> SocialContextVenues { get; set; }
        [Display(Name = "Social context - addresses")]
        [ValidationChildEnumerable]
        public virtual ICollection<SocialContextAddress> SocialContextAddresses { get; set; }
        [Display(Name = "Treatment events")]
        [ValidationChildEnumerable]
        public virtual ICollection<TreatmentEvent> TreatmentEvents { get; set; }
        [ValidationChild]
        public virtual DrugResistanceProfile DrugResistanceProfile { get; set; }
        public virtual ICollection<PreviousTbService> PreviousTbServices { get; set; }

        #endregion

        public bool HasBeenNotified => NotificationStatus == NotificationStatus.Notified
                                       || NotificationStatus == NotificationStatus.Closed
                                       || NotificationStatus == NotificationStatus.Legacy;

        public bool HasNonPostMortemEvents => TreatmentEvents != null && TreatmentEvents
            .Any(te => te.TreatmentEventType != TreatmentEventType.DiagnosisMade && !te.TreatmentEventIsDeathEvent);

        public bool IsPostMortemAndHasCorrectEvents =>
            (ClinicalDetails?.IsPostMortem == true)
            && TreatmentEvents != null
            && (TreatmentEvents.Any(x => x.TreatmentEventIsDeathEvent)
            && (TreatmentEvents.Count == 1 || HasCorrectDiagnosisMadeEvent));

        private bool HasCorrectDiagnosisMadeEvent =>
            TreatmentEvents.Count == 2
            && TreatmentEvents.Any(x => x.TreatmentEventType == TreatmentEventType.DiagnosisMade)
            && TreatmentEvents.Single(x => x.TreatmentEventType == TreatmentEventType.DiagnosisMade).EventDate
                >= TreatmentEvents.Single(x => x.TreatmentEventTypeIsOutcome).EventDate;

        public bool ShouldBeClosed()
        {
            var mostRecentTreatmentEvent = TreatmentEvents.GetMostRecentTreatmentEvent();

            return NotificationStatus == NotificationStatus.Notified 
                   && mostRecentTreatmentEvent?.TreatmentOutcomeId != null 
                   && mostRecentTreatmentEvent.TreatmentEventTypeIsOutcome
                   && NotStillOnTreatmentAndOlderThanOneYear(mostRecentTreatmentEvent);

            bool NotStillOnTreatmentAndOlderThanOneYear(TreatmentEvent treatmentEvent)
            {
                var treatmentOutcome = treatmentEvent.TreatmentOutcome
                                       ?? SeedData.TreatmentOutcomes.GetTreatmentOutcomes()
                                           .Single(oc => oc.TreatmentOutcomeId == treatmentEvent.TreatmentOutcomeId);

                return treatmentOutcome.TreatmentOutcomeSubType != TreatmentOutcomeSubType.StillOnTreatment 
                       && treatmentEvent.EventDate < DateTime.Today.AddYears(-1);
            }
        }

        string IOwnedEntityForAuditing.RootEntityType => RootEntities.Notification;
    }
}
