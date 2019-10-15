namespace ntbs_service.Helpers
{
    public static class RouteHelper
    {
        public static string GetFullNotificationEditPath(string subPath, int id, bool isBeingSubmitted)
        {
            return $"{GetNotificationEditBasePath(subPath)}?id={id}&isBeingSubmitted={isBeingSubmitted}";
        }

        public static string GetNotificationEditBasePath(string subPath)
        {
            return $"/Notifications/Edit/{subPath}";
        }

        public static string GetFullNotificationPath(string subPath, int id)
        {
            return $"{GetNotificationBasePath(subPath)}?id={id}";
        }

        public static string GetNotificationBasePath(string subPath)
        {
            return $"/Notifications/{subPath}";
        }
    }
}