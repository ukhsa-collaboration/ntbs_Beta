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
using ntbs_service.Models.Validations;

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
            await CleanData(notification, context, runId);
            return (await GetValidationErrors(context, runId, notification)).ToList();
        }

        /// <summary>
        /// There are instances of validation issues in the legacy data where we've resolved to remove the offending
        /// data points.
        /// As this is a data-lossy action, we want to perform it here (rather than at sql script level), to ensure that
        /// it is recorded in the migration log
        /// </summary>
        private async Task CleanData(Notification notification,
            PerformContext context,
            int runId)
        {
            TrimAndCleanStringProperties(notification);

            await CleanTestData(notification, context, runId);

            await CleanContactTracingData(notification, context, runId);
        }

        private static void TrimAndCleanStringProperties(object entity)
        {
            // Clean strings properties of this object
            entity?.GetType()
                .GetProperties()
                .Where(prop => prop.PropertyType == typeof(string)
                               && prop.CanWrite
                               && (Attribute.IsDefined(prop, typeof(RegularExpressionAttribute))
                                   || Attribute.IsDefined(prop, typeof(MaxLengthAttribute))))
                .ForEach(prop => prop.SetValue(entity, TrimAndCleanString((string)prop.GetValue(entity))));

            // Clean strings properties of this object's entity children
            entity?
                .GetType()
                .GetProperties()
                .Where(prop => Attribute.IsDefined(prop, typeof(ValidationChildAttribute)))
                .ForEach(prop => TrimAndCleanStringProperties(prop.GetValue(entity)));

            // Clean strings properties of this object's enumerable children
            entity?
                .GetType()
                .GetProperties()
                .Where(prop => Attribute.IsDefined(prop, typeof(ValidationChildEnumerableAttribute)))
                .Select(prop => prop.GetValue(entity))
                .Where(enumerable => enumerable != null)
                .OfType<IEnumerable<object>>()
                // Flatten
                .SelectMany(element => element)
                .ForEach(TrimAndCleanStringProperties);
        }

        private static string TrimAndCleanString(string s) => s?.Trim().Replace("\t", " ");

        private async Task CleanTestData(Notification notification, PerformContext context, int runId)
        {
            var missingDateResults = notification.TestData.ManualTestResults
                .Where(result => !result.TestDate.HasValue)
                .ToList();
            foreach (var result in missingDateResults)
            {
                const string missingDateMessage = "had test results without a date set. " +
                                                  "The notification will be imported without this test record.";
                await _logger.LogNotificationWarning(context, runId, notification.LegacyId, missingDateMessage);
                notification.TestData.ManualTestResults.Remove(result);
            }

            var dateInFutureResults = notification.TestData.ManualTestResults
                .Where(result => result.TestDate > DateTime.Today)
                .ToList();
            foreach (var result in dateInFutureResults)
            {
                const string dateInFutureMessage = "had test results with date set in future. " +
                                                   "The notification will be imported without this test record.";
                await _logger.LogNotificationWarning(context, runId, notification.LegacyId, dateInFutureMessage);
                notification.TestData.ManualTestResults.Remove(result);
            }

            var missingResults = notification.TestData.ManualTestResults
                .Where(result => result.Result == null)
                .ToList();
            foreach (var result in missingResults)
            {
                const string missingResultMessage = "had test results without a result recorded. " +
                                                    "The notification will be imported without this test record.";
                await _logger.LogNotificationWarning(context, runId, notification.LegacyId, missingResultMessage);
                notification.TestData.ManualTestResults.Remove(result);
            }

            // After filtering out invalid tests, we might have none left
            if (!notification.TestData.ManualTestResults.Any())
            {
                notification.TestData.HasTestCarriedOut = false;
            }
        }

        private async Task CleanContactTracingData(Notification notification, PerformContext context, int runId)
        {
            if (!ValidateObject(notification.ContactTracing).Any())
            {
                // If there are no issues in the contact tracing
                return;
            }

            var messages = new List<string>();
            var lostData = new List<string>();

            // The order we clean properties is important, because if we overwrite one value (eg. AdultsScreened) with
            // null then we also need to overwrite the values that are necessarily lower than it (eg. AdultsActiveTB)

            CleanValidationProperty(nameof(ContactTracing.AdultsScreened), notification, messages, lostData);
            CleanValidationProperty(nameof(ContactTracing.ChildrenScreened), notification, messages, lostData);

            // Because Active + Latent <= Screened, these have to be cleaned together
            CleanValidationPropertyPair(nameof(ContactTracing.AdultsActiveTB), nameof(ContactTracing.AdultsLatentTB),
                notification, messages, lostData);
            CleanValidationPropertyPair(nameof(ContactTracing.ChildrenActiveTB),
                nameof(ContactTracing.ChildrenLatentTB), notification, messages, lostData);


            CleanValidationProperty(nameof(ContactTracing.AdultsStartedTreatment), notification, messages, lostData);
            CleanValidationProperty(nameof(ContactTracing.ChildrenStartedTreatment), notification, messages, lostData);

            CleanValidationProperty(nameof(ContactTracing.AdultsFinishedTreatment), notification, messages, lostData);
            CleanValidationProperty(nameof(ContactTracing.ChildrenFinishedTreatment), notification, messages, lostData);

            foreach (var message in messages)
            {
                await _logger.LogNotificationWarning(context, runId, notification.LegacyId, message);
            }

            var contactTracingNotes =
                "The following contact tracing values were invalid and were removed from the notification:\n"
                + string.Join(", ", lostData);

            if (string.IsNullOrWhiteSpace(notification.ClinicalDetails.Notes))
            {
                notification.ClinicalDetails.Notes = contactTracingNotes;
            }
            else
            {
                notification.ClinicalDetails.Notes += notification.ClinicalDetails.Notes.Last() == '.' ? "" : ".";
                notification.ClinicalDetails.Notes += "\n" + contactTracingNotes;
            }
        }

        private static void CleanValidationProperty(string propertyName, Notification notification,
            List<string> messages, List<string> lostData)
        {
            var property = typeof(ContactTracing).GetProperty(propertyName);
            var propertyValue = property?.GetValue(notification.ContactTracing);

            var validationResults =
                ValidateProperty(notification.ContactTracing, propertyValue, propertyName).ToList();
            if (validationResults.Any())
            {
                property?.SetValue(notification.ContactTracing, null);
                messages.AddRange(validationResults.Select(result => result.ErrorMessage));
                lostData.Add($"{propertyName}: {propertyValue}");
            }
        }

        private static void CleanValidationPropertyPair(string propertyName1, string propertyName2,
            Notification notification, List<string> messages, List<string> lostData)
        {
            var property1 = typeof(ContactTracing).GetProperty(propertyName1);
            var propertyValue1 = property1?.GetValue(notification.ContactTracing);

            var property2 = typeof(ContactTracing).GetProperty(propertyName2);
            var propertyValue2 = property2?.GetValue(notification.ContactTracing);

            var validationResults1 =
                ValidateProperty(notification.ContactTracing, propertyValue1, propertyName1).ToList();
            var validationResults2 =
                ValidateProperty(notification.ContactTracing, propertyValue2, propertyName2).ToList();

            if (validationResults1.Any() || validationResults2.Any())
            {
                property1?.SetValue(notification.ContactTracing, null);
                property2?.SetValue(notification.ContactTracing, null);

                messages.AddRange(validationResults1.Select(result => result.ErrorMessage));
                messages.AddRange(validationResults2.Select(result => result.ErrorMessage));

                if (propertyValue1 != null) lostData.Add($"{propertyName1}: {propertyValue1}");
                if (propertyValue2 != null) lostData.Add($"{propertyName2}: {propertyValue2}");
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

        private static IEnumerable<ValidationResult> ValidateProperty(object objectToValidate, object propertyValue,
            string propertyName)
        {
            var validationContext = new ValidationContext(objectToValidate, null, null) { MemberName = propertyName };
            var validationsResults = new List<ValidationResult>();

            Validator.TryValidateProperty(propertyValue, validationContext, validationsResults);

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
