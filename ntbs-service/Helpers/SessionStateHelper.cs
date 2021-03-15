using System;
using Microsoft.AspNetCore.Http;

namespace ntbs_service.Helpers
{
    public static class SessionStateHelper
    {
        public static void UpdateSessionActivity(ISession session)
        {
            session.SetString("LastActivityTimestamp", DateTime.Now.ToString());
        }

        public static bool IsUpdatedRecently(ISession session)
        {
            var activityTimestamp = session.GetString("LastActivityTimestamp");
            return activityTimestamp != null && DateTime.Parse(activityTimestamp).AddMinutes(30) > DateTime.Now;
        }
    }
}
