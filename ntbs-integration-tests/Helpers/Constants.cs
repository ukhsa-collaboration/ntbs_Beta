using ntbs_service.Helpers;

namespace ntbs_integration_tests.Helpers
{
    public static class Routes
    {
        public const string HomePage = "/";
        public const string SearchPage = "/Search";

        public static string Patient = RouteHelper.GetNotificationEditBasePath("Patient");
        public static string Episode = RouteHelper.GetNotificationEditBasePath("Episode");
        public static string ClinicalDetails = RouteHelper.GetNotificationEditBasePath("ClinicalDetails");
        public static string ContactTracing = RouteHelper.GetNotificationEditBasePath("ContactTracing");
        public static string SocialRiskFactors = RouteHelper.GetNotificationEditBasePath("SocialRiskFactors");
        public static string Travel = RouteHelper.GetNotificationEditBasePath("Travel");
        public static string Comorbidities = RouteHelper.GetNotificationEditBasePath("Comorbidities");
        public static string Immunosuppression = RouteHelper.GetNotificationEditBasePath("Immunosuppression");
        public static string PreviousHistory = RouteHelper.GetNotificationEditBasePath("PreviousHistory");

        public static string Overview = RouteHelper.GetNotificationBasePath("Overview");
        public static string LinkedNotifications = RouteHelper.GetNotificationBasePath("LinkedNotifications");
        public static string Denotify = RouteHelper.GetNotificationBasePath("Denotify");
        public static string Delete = RouteHelper.GetNotificationBasePath("Delete");
    }
}
