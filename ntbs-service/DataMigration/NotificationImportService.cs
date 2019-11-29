using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using EFAuditer;
using ntbs_service.DataAccess;
using ntbs_service.Models;
using ntbs_service.Models.Enums;
using Serilog;

namespace ntbs_service.DataMigration
{

    public interface INotificationImportService
    {
        Task<IList<Notification>> ImportNotificationsAsync(string notificationId);
    }

    public class NotificationImportService : INotificationImportService
    {
        private readonly INotificationSearcher notificationSearcher;
        private readonly INotificationRepository notificationRepository;
        private readonly NtbsContext _context;

        public NotificationImportService(INotificationSearcher notificationSearcher,
                             INotificationRepository notificationRepository,
                             NtbsContext context)
        {
            this.notificationSearcher = notificationSearcher;
            this.notificationRepository = notificationRepository;
            _context = context;
        }

        public async Task<IList<Notification>> ImportNotificationsAsync(string notificationId)
        {
            LogInformation($"Request to import Notification with Id={notificationId}");

            if (notificationRepository.NotificationWithLtbrOrEtsIdExists(notificationId)) 
            {
                LogInformation($"Notification with Id={notificationId} already exists in database");
                return null;
            }

            var notificationTask = notificationSearcher.Search(notificationId);
            Task.WaitAll(notificationTask);
            var notifications = notificationTask.Result.ToList();

            LogInformation($"{notifications.Count} notifications included linked ones found to import");
            var notificationsToSave = new List<Notification>();
            foreach (var notification in notifications)
            {
                var linkedNotificationId = notification.LTBRID ?? notification.ETSID;  
                LogInformation($"Validating notification with Id={linkedNotificationId}");

                var validationErrors = GetValidationErrors(notification);
                
                if (validationErrors.Count() == 0)
                {
                    notificationsToSave.Add(notification);
                    LogInformation($"No validation errors found");
                }
                else
                {
                    LogInformation($"{validationErrors.Count()} validation errors found:");
                    foreach (var validationError in validationErrors)
                    {
                        LogInformation(validationError.ErrorMessage);
                    }
                }
            }
            LogInformation($"Importing {notificationsToSave.Count()} valid notifications");
            var savedNotificaitons = await AddLinkedNotificationsAsync(notificationsToSave);


            LogInformation($"Finished importing notification with Id={notificationId} and linked notifications");
            return savedNotificaitons;
        }

        private IEnumerable<ValidationResult> GetValidationErrors(Notification notification) 
        {
            var validationsResults = new List<ValidationResult>();

            notification.ShouldValidateFull = true;  
            notification.PatientDetails.ShouldValidateFull = true;  
            notification.ClinicalDetails.ShouldValidateFull = true;  
            notification.TravelDetails.ShouldValidateFull = true;  
            notification.VisitorDetails.ShouldValidateFull = true;  

            validationsResults.AddRange(ValidateObject(notification));
            validationsResults.AddRange(ValidateObject(notification.PatientDetails));
            validationsResults.AddRange(ValidateObject(notification.ClinicalDetails));
            validationsResults.AddRange(ValidateObject(notification.TravelDetails));
            validationsResults.AddRange(ValidateObject(notification.VisitorDetails));

            return validationsResults;
        }

        private IEnumerable<ValidationResult> ValidateObject<T>(T objectToValidate)
        {
            var validationContext = new ValidationContext(objectToValidate);
            var validationsResults = new List<ValidationResult>();  

            Validator.TryValidateObject(objectToValidate, validationContext, validationsResults, true);

            return validationsResults;
        }

        private void LogInformation(string message)
        {
            Log.Information($"NOTIFICATION IMPORT: {message}");
        }

        private async Task<List<Notification>> AddLinkedNotificationsAsync(List<Notification> notifications)
        {
            var group = new NotificationGroup();
            _context.NotificationGroup.Add(group);

            notifications = notifications.Select(n => {n.GroupId = group.NotificationGroupId; return n;}).ToList();
            _context.Notification.AddRange(notifications);

            _context.AddAuditCustomField(CustomFields.AuditDetails, AuditType.Imported);
            await _context.SaveChangesAsync();
            return notifications;
        }
    }
}
