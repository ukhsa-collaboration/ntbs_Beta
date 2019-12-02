using Microsoft.AspNetCore.WebUtilities;

namespace ntbs_service.Helpers
{
    public static class RouteHelper
    {
        public static string NotificationBasePath => "/Notifications/{0}/{1}";

        public static string GetNotificationPath(string subPath, int id, bool isBeingSubmitted = false)
        {
            var path = subPath;
            if (isBeingSubmitted)
            {
                path = QueryHelpers.AddQueryString(path, "isBeingSubmitted", "True");
            }

            return string.Format(NotificationBasePath, id, path);
        }

        public static string GetAlertPath(int id, string subPath)
        {
            return string.Format("/Alerts/{0}/{1}", id, subPath);
        }
    }

    public static class NotificationSubPaths
    {
        public static string EditClinicalDetails => "Edit/ClinicalDetails";
        public static string EditPatientDetails => "Edit/PatientDetails";
        public static string EditEpisode => "Edit/Episode";
        public static string EditContactTracing => "Edit/ContactTracing";
        public static string EditSocialRiskFactors => "Edit/SocialRiskFactors";
        public static string EditTravel => "Edit/Travel";
        public static string EditComorbidities => "Edit/Comorbidities";
        public static string EditImmunosuppression => "Edit/Immunosuppression";
        public static string EditPreviousHistory => "Edit/PreviousHistory";
        public static string EditMDRDetails => "Edit/MDRDetails";
        public static string Overview => string.Empty;
        public static string LinkedNotifications => "LinkedNotifications";
        public static string Denotify => "Denotify";
        public static string Delete => "Delete";
    }

    public static class AlertSubPaths
    {
        public static string Dismiss => "Dismiss";
    }
}
