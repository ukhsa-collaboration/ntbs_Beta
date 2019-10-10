namespace ntbs_service.Helpers
{
    public static class RouteHelper
    {
        public static string GetFullNotificationEditPath(string subPath, int id, bool isBeingSubmitted)
        {
            return $"{GetNotificationEditPath(subPath)}?id={id}&isBeingSubmitted={isBeingSubmitted}";
        }

        public static string GetNotificationEditPath(string subPath)
        {
            return $"/Notifications/Edit/{subPath}";
        }

        public static string GetFullNotificationPath(string subPath, int id)
        {
            return $"/Notifications/{subPath}?id={id}";
        }
    }
}