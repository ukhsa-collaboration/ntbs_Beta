using System;
using Microsoft.AspNetCore.Http;

namespace ntbs_service.Services
{
    public interface ISessionStateService
    {
        void UpdateSessionActivity(ISession session);
        bool IsUpdatedRecently(ISession session);
    }
    
    public class SessionStateService : ISessionStateService
    {
        public void UpdateSessionActivity(ISession session)
        {
            session.SetString("LastActivityTimestamp", DateTime.Now.ToString());
        }

        public bool IsUpdatedRecently(ISession session)
        {
            var dateString = session.GetString("LastActivityTimestamp");
            var isActive = true;
            if (DateTime.TryParse(dateString, out var date))
            {
                isActive = date.AddMinutes(13) > DateTime.Now;
            }

            return isActive;
        }
    }
}
