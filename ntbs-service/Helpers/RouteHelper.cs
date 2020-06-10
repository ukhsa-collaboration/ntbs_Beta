using System.Collections.Generic;
using Microsoft.AspNetCore.WebUtilities;

namespace ntbs_service.Helpers
{
    public static class RouteHelper
    {
        private static string NotificationBasePath => "/Notifications/{0}/{1}";
        public static string GetLegacyNotificationPath(string legacyNotificationId) => $"/LegacyNotifications/{legacyNotificationId}";

        public static string GetSubmittingNotificationPath(int notificationId, string subPath)
        {
            return GetNotificationPath(notificationId, subPath, new Dictionary<string, string> { { "isBeingSubmitted", "True" } });
        }

        public static string GetNotificationPath(int notificationId, string subPath, Dictionary<string, string> formData = null)
        {
            var path = subPath;

            if (formData != null)
            {
                path = QueryHelpers.AddQueryString(path, formData);
            }

            return string.Format(NotificationBasePath, notificationId, path);
        }

        public static string GetNotificationOverviewPathWithSectionAnchor(int notificationId, string sectionSubPath)
        {
            var overviewPath = GetNotificationPath(notificationId, NotificationSubPaths.Overview);
            var anchorId = OverviewSubPathToAnchorMap.GetOverviewAnchorId(sectionSubPath);
            return string.IsNullOrEmpty(anchorId) ? overviewPath : $"{overviewPath}#{anchorId}";
        }

        public static string GetAlertPath(int alertId, string subPath)
        {
            return $"/Alerts/{alertId}/{subPath}";
        }

        public static string GetUnmatchedSpecimenPath(string specimenId)
        {
            return $"/LabResults/#specimen-{specimenId}";
        }

        public static string GetContactDetailsSubPath(string subPath)
        {
            return $"/ContactDetails/{subPath}";
        }
    }

    public static class NotificationSubPaths
    {
        public static string EditClinicalDetails => "Edit/ClinicalDetails";
        public static string EditTestResults => "Edit/TestResults";
        public static string EditPatientDetails => "Edit/PatientDetails";
        public static string EditHospitalDetails => "Edit/HospitalDetails";
        public static string EditContactTracing => "Edit/ContactTracing";
        public static string EditSocialRiskFactors => "Edit/SocialRiskFactors";
        public static string EditTravel => "Edit/Travel";
        public static string EditComorbidities => "Edit/Comorbidities";
        public static string EditSocialContextAddresses => "Edit/SocialContextAddresses";
        public static string EditSocialContextVenues => "Edit/SocialContextVenues";
        public static string EditPreviousHistory => "Edit/PreviousHistory";
        public static string EditMDRDetails => "Edit/MDRDetails";
        public static string EditTreatmentEvents => "Edit/TreatmentEvents";
        public static string EditMBovisExposureToKnownCases => "Edit/MBovis/ExposureToKnownCases";
        public static string EditMBovisUnpasteurisedMilkConsumptions => "Edit/MBovis/UnpasteurisedMilkConsumptions";
        public static string EditMBovisOccupationExposures => "Edit/MBovis/OccupationExposures";
        public static string EditMBovisAnimalExposures => "Edit/MBovis/AnimalExposures";
        public static string Overview => string.Empty;
        public static string LinkedNotifications => "LinkedNotifications";
        public static string Denotify => "Denotify";
        public static string Delete => "Delete";
        public static string TransferRequest => "Transfer";
        public static string ActionTransferRequest => "ActionTransfer";
        public static string TransferDeclined => "TransferDeclined";

        public static string EditManualTestResult(int? testResultId) => $"Edit/ManualTestResult/{testResultId}";
        public static string AddManualTestResult => $"Edit/ManualTestResult/New";

        public static string EditSocialContextAddress(int? socialContextAddressId) => $"Edit/SocialContextAddress/{socialContextAddressId}";
        public static string AddSocialContextAddress => "Edit/SocialContextAddress/New";
        public static string EditSocialContextAddressSubPath => "Edit/SocialContextAddress/";

        public static string EditSocialContextVenue(int? socialContextVenueId) => $"Edit/SocialContextVenue/{socialContextVenueId}";
        public static string AddSocialContextVenue => "Edit/SocialContextVenue/New";
        public static string EditSocialContextVenueSubPath => "Edit/SocialContextVenue/";

        public static string EditTreatmentEvent(int treatmentEventId) => $"Edit/TreatmentEvent/{treatmentEventId}";
        public static string AddTreatmentEvent => $"Edit/TreatmentEvent/New";

        public static string EditMBovisExposureToKnownCase(int mBovisExposureToKnownCaseId) => $"Edit/MBovis/ExposureToKnownCase/{mBovisExposureToKnownCaseId}";
        public static string AddMBovisExposureToKnownCase => "Edit/MBovis/ExposureToKnownCase/New";
        
        public static string EditMBovisUnpasteurisedMilkConsumption(int mBovisUnpasteurisedMilkConsumptionId) => $"Edit/MBovis/UnpasteurisedMilkConsumption/{mBovisUnpasteurisedMilkConsumptionId}";
        public static string AddMBovisUnpasteurisedMilkConsumption => "Edit/MBovis/UnpasteurisedMilkConsumption/New";
        
        public static string EditMBovisOccupationExposure(int mBovisOccupationExposureId) => $"Edit/MBovis/OccupationExposure/{mBovisOccupationExposureId}";
        public static string AddMBovisOccupationExposure => "Edit/MBovis/OccupationExposure/New";
        
        public static string EditMBovisAnimalExposure(int mBovisAnimalExposureId) => $"Edit/MBovis/AnimalExposure/{mBovisAnimalExposureId}";
        public static string AddMBovisAnimalExposure => "Edit/MBovis/AnimalExposure/New";
    }

    public static class OverviewSubPathToAnchorMap
    {
        private static readonly Dictionary<string, string> _subPathToAnchorId = new Dictionary<string, string>
        {
            {NotificationSubPaths.EditPatientDetails, "overview-patient-details"},
            {NotificationSubPaths.EditHospitalDetails, "overview-hospital-details"},
            {NotificationSubPaths.EditClinicalDetails, "overview-clinical-details"},
            {NotificationSubPaths.EditContactTracing, "overview-contact-tracing"},
            {NotificationSubPaths.EditTestResults, "overview-test-results"},
            {NotificationSubPaths.EditSocialRiskFactors, "overview-social-risks"},
            {NotificationSubPaths.EditComorbidities, "overview-comorbidities"},
            {NotificationSubPaths.EditTravel, "overview-travel-and-visitors"},
            {NotificationSubPaths.EditSocialContextAddresses, "overview-social-addresses"},
            {NotificationSubPaths.EditTreatmentEvents, "overview-episodes"},
            {NotificationSubPaths.EditSocialContextVenues, "overview-social-venues"},
            {NotificationSubPaths.EditPreviousHistory, "overview-previous-history"},
            {NotificationSubPaths.EditMDRDetails, "overview-mdr-details"},
            {NotificationSubPaths.EditMBovisExposureToKnownCases, "overview-mbovis-exposure-details"},
            {NotificationSubPaths.EditMBovisUnpasteurisedMilkConsumptions, "overview-mbovis-milk-consumption-details"},
            {NotificationSubPaths.EditMBovisOccupationExposures, "overview-mbovis-occupation-exposure"},
            {NotificationSubPaths.EditMBovisAnimalExposures, "overview-mbovis-animal-exposure-details"}
        };

        public static string GetOverviewAnchorId(string notificationSubPath)
        {
            if (string.IsNullOrEmpty(notificationSubPath))
            {
                return null;
            }
            
            return _subPathToAnchorId.ContainsKey(notificationSubPath) ? _subPathToAnchorId[notificationSubPath] : null;
        }
    }

    public static class AlertSubPaths
    {
        public static string Dismiss => "Dismiss";
    }

    public static class ContactDetailsSubPaths
    {
        public static string Edit => "Edit";
    }
}
