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
            var date = DateTime.Parse(session.GetString("LastActivityTimestamp"));
            return date.AddMinutes(13) > DateTime.Now;
        }
    }
}
