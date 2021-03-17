using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Hangfire.Server;
using MoreLinq;
using ntbs_service.DataAccess;
using ntbs_service.Helpers;
using ntbs_service.Models.Entities;

namespace ntbs_service.DataMigration
{
    public interface IImportValidator
    {
        Task<List<ValidationResult>> CleanAndValidateNotification(PerformContext context,
            string requestId,
            Notification notification);
    }

    public class ImportValidator : IImportValidator
    {
        private readonly IImportLogger _logger;
        private readonly IReferenceDataRepository _referenceDataRepository;

        public ImportValidator(IImportLogger logger, IReferenceDataRepository referenceDataRepository)
        {
            _logger = logger;
            _referenceDataRepository = referenceDataRepository;
        }

        public async Task<List<ValidationResult>> CleanAndValidateNotification(PerformContext context,
            string requestId,
            Notification notification)
        {
            CleanData(notification, context, requestId);
            return (await GetValidationErrors(context, requestId, notification)).ToList();
        }

        /// <summary>
        /// There are instances of validation issues in the legacy data where we've resolved to remove the offending
        /// data points.
        /// As this is a data-lossy action, we want to perform it here (rather than at sql script level), to ensure that
        /// it is recorded in the migration log
        /// </summary>
        private void CleanData(Notification notification,
            PerformContext context,
            string requestId)
        {
            var missingDateResults = notification.TestData.ManualTestResults
                .Where(result => !result.TestDate.HasValue)
                .ToList();
            missingDateResults.ForEach(result =>
            {
                var missingDateMessage =
                    $"Notification {notification.LegacyId} had test results without a date set. The notification will be imported without this test record.";
                _logger.LogWarning(context, requestId, missingDateMessage);
                notification.TestData.ManualTestResults.Remove(result);
            });

            var dateInFutureResults = notification.TestData.ManualTestResults
                .Where(result => result.TestDate > DateTime.Today)
                .ToList();
            dateInFutureResults.ForEach(result =>
            {
                var dateInFutureMessage =
                    $"Notification {notification.LegacyId} had test results with date set in future. The notification will be imported without this test record.";
                _logger.LogWarning(context, requestId, dateInFutureMessage);
                notification.TestData.ManualTestResults.Remove(result);
            });

            var missingResults = notification.TestData.ManualTestResults
                .Where(result => result.Result == null)
                .ToList();
            missingResults.ForEach(result =>
            {
                var missingResultMessage =
                    $"Notification {notification.LegacyId} had test results without a result recorded. The notification will be imported without this test record.";
                _logger.LogWarning(context, requestId, missingResultMessage);
                notification.TestData.ManualTestResults.Remove(result);
            });

            // After filtering out invalid tests, we might have none left
            if (!notification.TestData.ManualTestResults.Any())
            {
                notification.TestData.HasTestCarriedOut = false;
            }

            if (ValidateObject(notification.ContactTracing).Any())
            {
                var message =
                    $"Notification {notification.LegacyId} invalid contact tracing figures. The notification will be imported without contact tracing data.";
                _logger.LogWarning(context, requestId, message);
                notification.ContactTracing = new ContactTracing();
            }
        }

        private async Task<IEnumerable<ValidationResult>> GetValidationErrors(PerformContext context,
            string requestId, Notification notification)
        {
            var singletonModels = new List<ModelBase>
            {
                notification.PatientDetails,
                notification.ClinicalDetails,
                notification.TravelDetails,
                notification.VisitorDetails,
                notification.ComorbidityDetails,
                notification.ImmunosuppressionDetails,
                notification.SocialRiskFactors,
                notification.HospitalDetails,
                notification.ContactTracing,
                notification.PreviousTbHistory,
                notification.TestData,
                notification.MBovisDetails,
                notification.MDRDetails
            };
            var modelCollections = new List<IEnumerable<ModelBase>>
            {
                notification.TestData.ManualTestResults,
                notification.SocialContextAddresses,
                notification.SocialContextVenues,
                notification.TreatmentEvents,
                notification.MBovisDetails.MBovisAnimalExposures,
                notification.MBovisDetails.MBovisExposureToKnownCases,
                notification.MBovisDetails.MBovisOccupationExposures,
                notification.MBovisDetails.MBovisUnpasteurisedMilkConsumptions,
            }.Where(collection => collection != null).ToList();

            // Set correct validation context everywhere
            NotificationHelper.SetShouldValidateFull(notification);
            void SetValidationContext(ModelBase model) => model.SetValidationContext(notification);
            singletonModels.ForEach(SetValidationContext);
            // patient details has special treatment due to the await-ed results below 
            notification.PatientDetails.SetValidationContext(notification);
            modelCollections.ForEach(collection => collection.ForEach(SetValidationContext));

            // Validate all models
            var validationsResults = new List<ValidationResult>();
            validationsResults.AddRange(ValidateObject(notification));
            singletonModels.Select(ValidateObject)
                .ForEach(results => validationsResults.AddRange(results));
            validationsResults.AddRange(await ValidateAndSetCaseManager(context, requestId, notification.HospitalDetails));
            validationsResults.AddRange(
                modelCollections.SelectMany(collection => collection.SelectMany(ValidateObject)));

            return validationsResults;
        }

        private static IEnumerable<ValidationResult> ValidateObject<T>(T objectToValidate) where T : ModelBase
        {
            var validationContext = new ValidationContext(objectToValidate);
            var validationsResults = new List<ValidationResult>();

            Validator.TryValidateObject(objectToValidate, validationContext, validationsResults, true);

            return validationsResults;
        }

        private async Task<IEnumerable<ValidationResult>> ValidateAndSetCaseManager(PerformContext context,
            string requestId,HospitalDetails details)
        {
            var validationsResults = new List<ValidationResult>();

            if (string.IsNullOrEmpty(details.CaseManagerUsername))
            {
                return validationsResults;
            }

            var possibleCaseManager =
                await _referenceDataRepository.GetUserByUsernameAsync(details.CaseManagerUsername);
            if (possibleCaseManager != null)
            {
                if (!possibleCaseManager.IsCaseManager)
                {
                    _logger.LogWarning(context, requestId, "User set as case manager for notification is not a case manager.");
                }
                return validationsResults;
            }

            var message = $"Case manager {details.CaseManagerUsername} is not present in NTBS database";
            validationsResults.Add(new ValidationResult(message, new[] { nameof(details.CaseManagerUsername) }));

            return validationsResults;
        }
    }
}
