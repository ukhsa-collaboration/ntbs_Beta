using ntbs_service.Helpers;

namespace ntbs_integration_tests.Helpers
{
    public static class Routes
    {
        public static string Patient = RouteHelper.GetNotificationEditPath("Patient");
        public static string Episode = RouteHelper.GetNotificationEditPath("Episode");
        public static string ClinicalDetails = RouteHelper.GetNotificationEditPath("ClinicalDetails");
        public static string ContactTracing = RouteHelper.GetNotificationEditPath("ContactTracing");
        public static string SocialRiskFactors = RouteHelper.GetNotificationEditPath("SocialRiskFactors");
        public static string Travel = RouteHelper.GetNotificationEditPath("Travel");
        public static string Comorbidities = RouteHelper.GetNotificationEditPath("Comorbidities");
        public static string Immunosuppression = RouteHelper.GetNotificationEditPath("Immunosupression");
        public static string PreviousHistory = RouteHelper.GetNotificationEditPath("PreviousHistory");
    }
}