using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using EFAuditer;
using ntbs_service.DataAccess;
using ntbs_service.Helpers;
using ntbs_service.Models;
using ntbs_service.Models.Enums;
using ntbs_service.Services;
using Serilog;

namespace ntbs_service.DataMigration
{

    public interface INotificationImportService
    {
        Task<IList<Notification>> ImportNotificationsAsync(string requestId, string notificationId);
    }

    public class NotificationImportService : INotificationImportService
    {

        private readonly INotificationMapper _notificationMapper;
        private readonly INotificationRepository _notificationRepository;
        private readonly INotificationImportRepository _notificationImportRepository;
        private readonly IPostcodeService _postcodeService;
        private readonly IImportLogger _logger;

        public NotificationImportService(INotificationMapper notificationMapper,
                             INotificationRepository notificationRepository,
                             INotificationImportRepository notificationImportRepository,
                             IPostcodeService postcodeService,
                             IImportLogger logger)
        {
            _notificationMapper = notificationMapper;
            _notificationRepository = notificationRepository;
            _notificationImportRepository = notificationImportRepository;
            _postcodeService = postcodeService;
            _logger = logger;
        }

        public async Task<IList<Notification>> ImportNotificationsAsync(string requestId, string notificationId)
        {
            _logger.LogInformation(requestId, $"Request to import Notification with Id={notificationId}");

            if (_notificationRepository.NotificationWithLegacyIdExists(notificationId)) 
            {
                _logger.LogInformation(requestId, $"Notification with Id={notificationId} already exists in database");
                return null;
            }

            var notifications = (await _notificationMapper.Get(notificationId)).ToList();
            if (notifications.Count == 0)
            {
                _logger.LogInformation(requestId, $"No notifications found with Id={notificationId}");
                return null;
            }

            LookupAndAssignPostcode(notifications);

            var patientName = notifications.FirstOrDefault().FullName;
            _logger.LogInformation(requestId, $"{notifications.Count} notifications found to import for {patientName}");

            bool isAnyNotificationInvalid = false;
            foreach (var notification in notifications)
            {
                var linkedNotificationId = notification.LTBRID ?? notification.ETSID;  
                _logger.LogInformation(requestId, $"Validating notification with Id={linkedNotificationId}");

                var validationErrors = GetValidationErrors(notification);
                if (validationErrors.Count() == 0)
                {
                    _logger.LogInformation(requestId, $"No validation errors found");
                }
                else
                {
                    isAnyNotificationInvalid = true;
                    _logger.LogWarning(requestId, $"{validationErrors.Count()} validation errors found for notification with Id={linkedNotificationId}:");
                    foreach (var validationError in validationErrors)
                    {
                        _logger.LogWarning(requestId, validationError.ErrorMessage);
                    }
                }
            }

            if (isAnyNotificationInvalid)
            {
                _logger.LogInformation(requestId, $"Terminating importing notifications for a patient due to validation errors");
                return null;
            }

            _logger.LogInformation(requestId, $"Importing {notifications.Count()} valid notifications");
            var savedNotifications = await _notificationImportRepository.AddLinkedNotificationsAsync(notifications);

            _logger.LogInformation(requestId, $"Finished importing notification for {patientName}");
            var newIdsString = string.Join(" ,", savedNotifications.Select(x => x.NotificationId));

            _logger.LogInformation(requestId, $"Imported notifications have following Ids: {newIdsString}");
            
            return savedNotifications;
        }

        private void LookupAndAssignPostcode(List<Notification> notifications)
        {
            var postcodes = _postcodeService.FindPostcodes(notifications.Select(x => x.PatientDetails.Postcode).ToList());
            notifications.ForEach(n => {
                var normalisedPostcode = n.PatientDetails.Postcode?.Replace(" ", "").ToUpper();
                var lookedUpPostcode = postcodes.FirstOrDefault(p => p.Postcode == normalisedPostcode);
                n.PatientDetails.PostcodeToLookup = lookedUpPostcode?.Postcode;
                n.PatientDetails.PostcodeLookup = lookedUpPostcode;
            });
        }

        private IEnumerable<ValidationResult> GetValidationErrors(Notification notification) 
        {
            var validationsResults = new List<ValidationResult>();

            NotificationHelper.SetShouldValidateFull(notification);

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
    }
}
