using System.Linq;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Projections;

namespace ntbs_service.Helpers
{
    internal static class QueryableExtensionMethods
    {
        public static IQueryable<NotificationForHomePage> SelectNotificationForHomePage(
            this IQueryable<Notification> queryable)
        {
            return queryable
                .Select(n => new NotificationForHomePage
                {
                    NotificationId = n.NotificationId,
                    CreationDate = n.CreationDate,
                    NotificationDate = n.NotificationDate,
                    GivenName = n.PatientDetails.GivenName,
                    FamilyName = n.PatientDetails.FamilyName,
                    TbServiceName = n.HospitalDetails.TBService.Name,
                    CaseManagerName = n.HospitalDetails.CaseManager.DisplayName
                });
        }
    }
}
