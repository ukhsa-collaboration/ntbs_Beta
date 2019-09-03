using System.Threading.Tasks;
using ntbs_service.DataAccess;
using ntbs_service.Models;

namespace ntbs_service.Services
{
    public interface INotificationService
    {
        Task<Notification> GetNotificationAsync(int? id);
        Task UpdateTimelineAsync(Notification notification, ClinicalTimeline timeline);
    }

    public class NotificationService : INotificationService
    {
        private INotificationRepository repository;
        private NtbsContext context;
        public NotificationService(INotificationRepository repository, NtbsContext context) {
            this.repository = repository;
            this.context = context;
        }

        public async Task<Notification> GetNotificationAsync(int? id) {
            return await repository.GetNotificationAsync(id);
        }

        public async Task UpdateTimelineAsync(Notification notification, ClinicalTimeline timeline)
        {
            UpdateTimelineFlags(timeline);
            context.Attach(notification);
            notification.ClinicalTimeline = timeline;

            await context.SaveChangesAsync();
        }
        private void UpdateTimelineFlags(ClinicalTimeline timeline)
        {
            if (timeline.DidNotStartTreatment) 
            {
                timeline.TreatmentStartDate = null;
            }
            if (!timeline.IsPostMortem) 
            {
                timeline.DeathDate = null;
            }
        }
    }
}