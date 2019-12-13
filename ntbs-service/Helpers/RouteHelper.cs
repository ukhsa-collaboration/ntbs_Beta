using System.Collections.Generic;
using Microsoft.AspNetCore.WebUtilities;

namespace ntbs_service.Helpers
{
    public static class RouteHelper
    {
        public static string NotificationBasePath => "/Notifications/{0}/{1}";

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

        public static string GetAlertPath(int alertId, string subPath)
        {
            return $"/Alerts/{alertId}/{subPath}";
        }
    }

    public static class NotificationSubPaths
    {
        public static string EditClinicalDetails => "Edit/ClinicalDetails";
        public static string EditTestResults => "Edit/TestResults";
        public static string EditPatientDetails => "Edit/PatientDetails";
        public static string EditEpisode => "Edit/Episode";
        public static string EditContactTracing => "Edit/ContactTracing";
        public static string EditSocialRiskFactors => "Edit/SocialRiskFactors";
        public static string EditTravel => "Edit/Travel";
        public static string EditComorbidities => "Edit/Comorbidities";
        public static string EditImmunosuppression => "Edit/Immunosuppression";
        public static string EditSocialContextVenues => "Edit/SocialContextVenues";
        public static string EditPreviousHistory => "Edit/PreviousHistory";
        public static string EditMDRDetails => "Edit/MDRDetails";
        public static string EditTreatmentEvents => "Edit/TreatmentEvents";
        public static string Overview => string.Empty;
        public static string LinkedNotifications => "LinkedNotifications";
        public static string Denotify => "Denotify";
        public static string Delete => "Delete";

        public static string EditManualTestResult(int? testResultId) => $"Edit/ManualTestResult/{testResultId}";
        public static string AddManualTestResult => $"Edit/ManualTestResult/New";

        public static string EditSocialContextVenue(int? socialContextVenueId) => $"Edit/SocialContextVenue/{socialContextVenueId}";
        public static string AddSocialContextVenue => "Edit/SocialContextVenue/New";
        public static string EditSocialContextVenueSubPath => "Edit/SocialContextVenue/";

        public static string EditTreatmentEvent(int treatmentEventId) => $"Edit/TreatmentEvent/{treatmentEventId}";
        public static string AddTreatmentEvent => $"Edit/TreatmentEvent/New";
    }

    public static class AlertSubPaths
    {
        public static string Dismiss => "Dismiss";
    }
}
