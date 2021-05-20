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
            int runId,
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
            int runId,
            Notification notification)
        {
            CleanData(notification, context, runId);
            return (await GetValidationErrors(context, runId, notification)).ToList();
        }

        /// <summary>
        /// There are instances of validation issues in the legacy data where we've resolved to remove the offending
        /// data points.
        /// As this is a data-lossy action, we want to perform it here (rather than at sql script level), to ensure that
        /// it is recorded in the migration log
        /// </summary>
        private void CleanData(Notification notification,
            PerformContext context,
            int runId)
        {
            var missingDateResults = notification.TestData.ManualTestResults
                .Where(result => !result.TestDate.HasValue)
                .ToList();
            missingDateResults.ForEach(result =>
            {
                var missingDateMessage = "had test results without a date set. " +
                                         "The notification will be imported without this test record.";
                _logger.LogNotificationWarning(context, runId, notification.LegacyId, missingDateMessage);
                notification.TestData.ManualTestResults.Remove(result);
            });

            var dateInFutureResults = notification.TestData.ManualTestResults
                .Where(result => result.TestDate > DateTime.Today)
                .ToList();
            dateInFutureResults.ForEach(result =>
            {
                var dateInFutureMessage = "had test results with date set in future. " +
                                          "The notification will be imported without this test record.";
                _logger.LogNotificationWarning(context, runId, notification.LegacyId, dateInFutureMessage);
                notification.TestData.ManualTestResults.Remove(result);
            });

            var missingResults = notification.TestData.ManualTestResults
                .Where(result => result.Result == null)
                .ToList();
            missingResults.ForEach(result =>
            {
                var missingResultMessage = "had test results without a result recorded. " +
                                           "The notification will be imported without this test record.";
                _logger.LogNotificationWarning(context, runId, notification.LegacyId, missingResultMessage);
                notification.TestData.ManualTestResults.Remove(result);
            });

            // After filtering out invalid tests, we might have none left
            if (!notification.TestData.ManualTestResults.Any())
            {
                notification.TestData.HasTestCarriedOut = false;
            }

            if (ValidateObject(notification.ContactTracing).Any())
            {
                var message = "invalid contact tracing figures. " +
                              "The notification will be imported without contact tracing data.";
                _logger.LogNotificationWarning(context, runId, notification.LegacyId, message);
                notification.ContactTracing = new ContactTracing();
            }
        }

        private async Task<IEnumerable<ValidationResult>> GetValidationErrors(PerformContext context,
            int runId, Notification notification)
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
            await VerifyCaseManager(context, runId, notification);
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

        private async Task VerifyCaseManager(PerformContext context,
            int runId, Notification notification)
        {
            if (!notification.HospitalDetails.CaseManagerId.HasValue)
            {
                return;
            }

            var possibleCaseManager =
                await _referenceDataRepository.GetUserByIdAsync(notification.HospitalDetails.CaseManagerId.Value);
            if (possibleCaseManager != null && !possibleCaseManager.IsCaseManager)
            {
                await _logger.LogNotificationWarning(context, runId, notification.LegacyId,
                    "User set as case manager for notification is not a case manager.");
            }
        }
    }
}
