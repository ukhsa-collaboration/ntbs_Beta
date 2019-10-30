namespace ntbs_service.Helpers
{
    public static class RouteHelper
    {
        public static string NotificationBasePath => "/Notifications/{0}/{1}";

        public static string GetNotificationPath(string subPath, int id, bool isBeingSubmitted = false)
        {
            var pathWithQueryString = string.Concat(subPath, isBeingSubmitted ? "?isBeingSubmitted=True" : string.Empty);
            return string.Format(NotificationBasePath, id, pathWithQueryString);
        }
    }

    public static class NotificationSubPaths
    {
        public static string EditClinicalDetails => "Edit/ClinicalDetails";
        public static string EditPatient => "Edit/Patient";
        public static string EditEpisode => "Edit/Episode";
        public static string EditContactTracing => "Edit/ContactTracing";
        public static string EditSocialRiskFactors => "Edit/SocialRiskFactors";
        public static string EditTravel => "Edit/Travel";
        public static string EditComorbidities => "Edit/Comorbidities";
        public static string EditImmunosuppression => "Edit/Immunosuppression";
        public static string EditPreviousHistory => "Edit/PreviousHistory";
        public static string Overview => string.Empty;
        public static string LinkedNotifications => "LinkedNotifications";
        public static string Denotify => "Denotify";
    }
}
