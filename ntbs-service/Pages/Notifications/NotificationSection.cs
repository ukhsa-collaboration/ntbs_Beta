using System;
using System.Collections.Generic;
using ntbs_service.Helpers;
using ntbs_service.Models.Entities;

// This file is an answer to a growing number of pain points regarding keeping notification section pages,
// their order and their names, their models and their locations in sync across the app.
//
// It sets out to provide a canonical list of the section pages
// (which does not include sub-pages like "linked notifications",
// nor edit for individual items of a group, like a "Test result"):
// - it defines their order
// - it defines the mapping to their display names (now sourced from corresponding models)
// - it defines the mapping to their urls
// - it defines a model->section mapping, useful e.g. when we need to understand which page to send the user to fix a
//   particular error
namespace ntbs_service.Pages.Notifications
{
    // Numbers assigned manually to make ordering explicit - we'll use this order as the canonical
    // order of the pages
    public enum NotificationSection
    {
        PersonalDetails = 10,
        HospitalDetails = 20,
        ClinicalDetails = 30,
        TestResults = 40,
        ContactTracing = 50,
        SocialRiskFactors = 60,
        TravelVisitorHistory = 70,
        Comorbidities = 80,
        SocialContextAddresses = 90,
        SocialContextVenues = 100,
        PreviousHistory = 110,
        TreatmentEvents = 120,
        MdrDetails = 130,
        MBovisOtherCases = 140,
        MBovisUnpasteurisedMilk = 150,
        MBovisOccupation = 160,
        MBovisAnimalExposure = 170
    }

    public static class NotificationSectionExtensions
    {
        public static string ToDisplayString(this NotificationSection section)
        {
            switch (section)
            {
                case NotificationSection.PersonalDetails:
                    return typeof(PatientDetails).GetDisplayName();
                case NotificationSection.HospitalDetails:
                    return typeof(HospitalDetails).GetDisplayName();
                case NotificationSection.ClinicalDetails:
                    return typeof(ClinicalDetails).GetDisplayName();
                case NotificationSection.TestResults:
                    return typeof(TestData).GetDisplayName();
                case NotificationSection.ContactTracing:
                    return typeof(ContactTracing).GetDisplayName();
                case NotificationSection.SocialRiskFactors:
                    return typeof(SocialRiskFactors).GetDisplayName();
                case NotificationSection.TravelVisitorHistory:
                    return "Travel/visitor history";
                case NotificationSection.Comorbidities:
                    return typeof(ComorbidityDetails).GetDisplayName();
                case NotificationSection.SocialContextAddresses:
                    return typeof(Notification).GetMemberDisplayName(nameof(Notification.SocialContextAddresses));
                case NotificationSection.SocialContextVenues:
                    return typeof(Notification).GetMemberDisplayName(nameof(Notification.SocialContextVenues));
                case NotificationSection.PreviousHistory:
                    return typeof(PreviousTbHistory).GetDisplayName();
                case NotificationSection.TreatmentEvents:
                    return typeof(Notification).GetMemberDisplayName(nameof(Notification.TreatmentEvents));
                case NotificationSection.MdrDetails:
                    return typeof(MDRDetails).GetDisplayName();
                case NotificationSection.MBovisOtherCases:
                    return typeof(MBovisExposureToKnownCase).GetDisplayName();
                case NotificationSection.MBovisUnpasteurisedMilk:
                    return typeof(MBovisUnpasteurisedMilkConsumption).GetDisplayName();
                case NotificationSection.MBovisOccupation:
                    return typeof(MBovisOccupationExposure).GetDisplayName();
                case NotificationSection.MBovisAnimalExposure:
                    return typeof(MBovisAnimalExposure).GetDisplayName();
                default:
                    throw new ArgumentOutOfRangeException(nameof(section), section, null);
            }
        }

        public static string ToSubPath(this NotificationSection section)
        {
            switch (section)
            {
                case NotificationSection.PersonalDetails:
                    return NotificationSubPaths.EditPatientDetails;
                case NotificationSection.HospitalDetails:
                    return NotificationSubPaths.EditHospitalDetails;
                case NotificationSection.ClinicalDetails:
                    return NotificationSubPaths.EditClinicalDetails;
                case NotificationSection.TestResults:
                    return NotificationSubPaths.EditTestResults;
                case NotificationSection.ContactTracing:
                    return NotificationSubPaths.EditContactTracing;
                case NotificationSection.SocialRiskFactors:
                    return NotificationSubPaths.EditSocialRiskFactors;
                case NotificationSection.TravelVisitorHistory:
                    return NotificationSubPaths.EditTravel;
                case NotificationSection.Comorbidities:
                    return NotificationSubPaths.EditComorbidities;
                case NotificationSection.SocialContextAddresses:
                    return NotificationSubPaths.EditSocialContextAddresses;
                case NotificationSection.SocialContextVenues:
                    return NotificationSubPaths.EditSocialContextVenues;
                case NotificationSection.PreviousHistory:
                    return NotificationSubPaths.EditPreviousHistory;
                case NotificationSection.TreatmentEvents:
                    return NotificationSubPaths.EditTreatmentEvents;
                case NotificationSection.MdrDetails:
                    return NotificationSubPaths.EditMDRDetails;
                case NotificationSection.MBovisOtherCases:
                    return NotificationSubPaths.EditMBovisExposureToKnownCases;
                case NotificationSection.MBovisUnpasteurisedMilk:
                    return NotificationSubPaths.EditMBovisUnpasteurisedMilkConsumptions;
                case NotificationSection.MBovisOccupation:
                    return NotificationSubPaths.EditMBovisOccupationExposures;
                case NotificationSection.MBovisAnimalExposure:
                    return NotificationSubPaths.EditMBovisAnimalExposures;
                default:
                    throw new ArgumentOutOfRangeException(nameof(section), section, null);
            }
        }

        public static bool IsMdr(this NotificationSection section)
        {
            return section == NotificationSection.MdrDetails;
        }

        public static bool IsMBovis(this NotificationSection section)
        {
            // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
            switch (section)
            {
                case NotificationSection.MBovisOtherCases:
                case NotificationSection.MBovisUnpasteurisedMilk:
                case NotificationSection.MBovisOccupation:
                case NotificationSection.MBovisAnimalExposure:
                    return true;
                default:
                    return false;
            }
        }
    }

    public static class NotificationSectionFactory
    {
        private static readonly Dictionary<Type, NotificationSection> TypeToModelDictionary =
            new Dictionary<Type, NotificationSection>
            {
                {typeof(PatientDetails), NotificationSection.PersonalDetails},
                {typeof(HospitalDetails), NotificationSection.HospitalDetails},
                {typeof(ClinicalDetails), NotificationSection.ClinicalDetails},
                {typeof(NotificationSite), NotificationSection.ClinicalDetails},
                {typeof(TestData), NotificationSection.TestResults},
                {typeof(ManualTestResult), NotificationSection.TestResults},
                {typeof(ContactTracing), NotificationSection.ContactTracing},
                {typeof(SocialRiskFactors), NotificationSection.SocialRiskFactors},
                {typeof(TravelDetails), NotificationSection.TravelVisitorHistory},
                {typeof(VisitorDetails), NotificationSection.TravelVisitorHistory},
                {typeof(ComorbidityDetails), NotificationSection.Comorbidities},
                {typeof(ImmunosuppressionDetails), NotificationSection.Comorbidities},
                {typeof(SocialContextAddress), NotificationSection.SocialContextAddresses},
                {typeof(SocialContextVenue), NotificationSection.SocialContextVenues},
                {typeof(PreviousTbHistory), NotificationSection.PreviousHistory},
                {typeof(TreatmentEvent), NotificationSection.TreatmentEvents},
                {typeof(MDRDetails), NotificationSection.MdrDetails},
                {typeof(MBovisExposureToKnownCase), NotificationSection.MBovisOtherCases},
                {typeof(MBovisUnpasteurisedMilkConsumption), NotificationSection.MBovisUnpasteurisedMilk},
                {typeof(MBovisOccupationExposure), NotificationSection.MBovisOccupation},
                {typeof(MBovisAnimalExposure), NotificationSection.MBovisAnimalExposure},
            };

        public static NotificationSection FromModel(Type model)
        {
            return TypeToModelDictionary[model];
        }
    }
}
