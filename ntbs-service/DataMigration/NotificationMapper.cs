using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Hangfire.Server;
using MoreLinq;
using ntbs_service.DataAccess;
using ntbs_service.Helpers;
using ntbs_service.Models;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;
using ntbs_service.Models.ReferenceEntities;
using ntbs_service.Models.Validations;
using Serilog;
using Countries = ntbs_service.Models.Countries;

// ReSharper disable UseObjectOrCollectionInitializer
// We're not using object initialization syntax in this file, as it obscures errors caused by wrong date types
// on the way out from the dynamic objects

namespace ntbs_service.DataMigration
{
    public interface INotificationMapper
    {
        Task<IEnumerable<IList<Notification>>> GetNotificationsGroupedByPatient(PerformContext context,
            string requestId, List<string> notificationId);
        Task<IEnumerable<IList<Notification>>> GetNotificationsGroupedByPatient(PerformContext context,
            string requestId, DateTime rangeStartDate, DateTime endStartDate);
    }

    public class NotificationMapper : INotificationMapper
    {
        private readonly IMigrationRepository _migrationRepository;
        private readonly IReferenceDataRepository _referenceDataRepository;
        private readonly IImportLogger _logger;

        public NotificationMapper(IMigrationRepository migrationRepository, IReferenceDataRepository referenceDataRepository, IImportLogger logger)
        {
            _migrationRepository = migrationRepository;
            _referenceDataRepository = referenceDataRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<IList<Notification>>> GetNotificationsGroupedByPatient(PerformContext context,
            string requestId, DateTime rangeStartDate, DateTime endStartDate)
        {
            var groupedIds = await _migrationRepository.GetGroupedNotificationIdsByDate(rangeStartDate, endStartDate);

            return await GetGroupedResultsAsNotificationAsync(groupedIds, context, requestId);
        }

        public async Task<IEnumerable<IList<Notification>>> GetNotificationsGroupedByPatient(PerformContext context,
            string requestId, List<string> notificationIds)
        {
            var groupedIds = await _migrationRepository.GetGroupedNotificationIdsById(notificationIds);

            return await GetGroupedResultsAsNotificationAsync(groupedIds, context, requestId);
        }

        private async Task<IEnumerable<IList<Notification>>> GetGroupedResultsAsNotificationAsync(IEnumerable<IGrouping<string, string>> groupedIds, PerformContext context, string requestId)
        {
            var resultList = new List<IList<Notification>>();
            foreach (var group in groupedIds)
            {
                var legacyIds = group.ToList();
                _logger.LogInformation(context, requestId, $"Fetching data for legacy notifications {string.Join(", ", legacyIds)}");
                var legacyNotifications = _migrationRepository.GetNotificationsById(legacyIds);
                var sitesOfDisease = _migrationRepository.GetNotificationSites(legacyIds);
                var manualTestResults = _migrationRepository.GetManualTestResults(legacyIds);
                var socialContextVenues = _migrationRepository.GetSocialContextVenues(legacyIds);
                var socialContextAddresses = _migrationRepository.GetSocialContextAddresses(legacyIds);
                var transferEvents =  _migrationRepository.GetTransferEvents(legacyIds);
                var outcomeEvents = _migrationRepository.GetOutcomeEvents(legacyIds);

                var notificationsForGroup = await CombineDataForGroup(
                    legacyIds,
                    (await legacyNotifications).ToList(),
                    (await sitesOfDisease).ToList(),
                    (await manualTestResults).ToList(),
                    (await socialContextVenues).ToList(),
                    (await socialContextAddresses).ToList(),
                    (await transferEvents).ToList(),
                    (await outcomeEvents).ToList()
                );
                resultList.Add(notificationsForGroup);
            }
            return resultList;
        }

        private async Task<IList<Notification>> CombineDataForGroup(IEnumerable<string> legacyIds,
            IList<dynamic> notifications,
            IList<dynamic> sitesOfDisease,
            IList<dynamic> manualTestResults,
            IList<dynamic> socialContextVenues,
            IList<dynamic> socialContextAddresses,
            IList<dynamic> transferEvents,
            IList<dynamic> outcomeEvents)
        {
            return await Task.WhenAll(legacyIds.Select(async id =>
            {
                var rawNotification = notifications.Single(n => n.OldNotificationId == id);
                var notificationSites = AsSites(sitesOfDisease
                    .Where(s => s.OldNotificationId == id)
                );
                var notificationTestResults = manualTestResults
                    .Where(t => t.OldNotificationId == id)
                    .Select(AsManualTestResult)
                    .ToList();
                var notificationSocialContextVenues = socialContextVenues
                    .Where(sc => sc.OldNotificationId == id)
                    .Select(AsSocialContextVenue)
                    .ToList();
                var notificationSocialContextAddresses = socialContextAddresses
                    .Where(sc => sc.OldNotificationId == id)
                    .Select(AsSocialContextAddress)
                    .ToList();
                var notificationTransferEvents = await Task.WhenAll(transferEvents
                    .Where(sc => sc.OldNotificationId == id)
                    .Select(AsTransferEvent)
                    .ToList()
                );
                var notificationOutcomeEvents = await Task.WhenAll(outcomeEvents
                    .Where(sc => sc.OldNotificationId == id)
                    .Select(AsOutcomeEvent)
                    .ToList()
                );

                Notification notification = await AsNotificationAsync(rawNotification);
                notification.NotificationSites = notificationSites;
                notification.TestData = new TestData
                {
                    HasTestCarriedOut = notificationTestResults.Any(), ManualTestResults = notificationTestResults
                };
                notification.SocialContextAddresses = notificationSocialContextAddresses;
                notification.SocialContextVenues = notificationSocialContextVenues;
                if (notification.NotificationStatus != NotificationStatus.Draft)
                {
                    notification.TreatmentEvents.Add(NotificationHelper.CreateTreatmentStartEvent(notification));
                }
                notificationTransferEvents.ForEach(notification.TreatmentEvents.Add);
                notificationOutcomeEvents.ForEach(notification.TreatmentEvents.Add);
                return notification;
            }));
        }

        private async Task<Notification> AsNotificationAsync(dynamic rawNotification)
        {
            var notification = new Notification();
            notification.ETSID = rawNotification.EtsId;
            notification.LTBRID = rawNotification.LtbrId;
            notification.LTBRPatientId = rawNotification.Source == "LTBR" ? rawNotification.GroupId : null;
            notification.NotificationDate = rawNotification.NotificationDate;
            notification.CreationDate = DateTime.Now;
            notification.PatientDetails = ExtractPatientDetails(rawNotification);
            notification.ClinicalDetails = ExtractClinicalDetails(rawNotification);
            notification.TravelDetails = ExtractTravelDetails(rawNotification);
            notification.VisitorDetails = ExtractVisitorDetails(rawNotification);
            notification.ComorbidityDetails = ExtractComorbidityDetails(rawNotification);
            notification.ImmunosuppressionDetails = ExtractImmunosuppressionDetails(rawNotification);
            notification.SocialRiskFactors = ExtractSocialRiskFactors(rawNotification);
            notification.HospitalDetails = await ExtractHospitalDetailsAsync(rawNotification);
            notification.ContactTracing = ExtractContactTracingDetails(rawNotification);
            notification.PatientTBHistory = ExtractPatientTBHistory(rawNotification);
            notification.TreatmentEvents = await ExtractTreatmentEventsAsync(rawNotification);
            notification.NotificationStatus = rawNotification.DenotificationDate == null
                ? NotificationStatus.Notified
                : NotificationStatus.Denotified;

            return notification;
        }

        private async Task<HospitalDetails> ExtractHospitalDetailsAsync(dynamic rawNotification)
        {
            var details = new HospitalDetails();
            details.HospitalId = rawNotification.NtbsHospitalId;
            details.CaseManagerUsername = rawNotification.CaseManager;
            var consultant = RemoveCharactersNotIn(ValidationRegexes.CharacterValidationWithNumbersForwardSlashExtended, rawNotification.Consultant);
            details.Consultant = consultant;
            // details.TBServiceCode is set below, based on the hospital

            if (details.HospitalId is Guid guid)
            {
                var tbService = (await _referenceDataRepository.GetTbServiceFromHospitalIdAsync(guid));
                if (tbService == null)
                {
                    Log.Error($"No TB service exists for hospital with guid {guid}");
                }
                else
                {
                    // It's OK to only set this where it exists
                    // - the service missing will come up in notification validation  
                    details.TBServiceCode = tbService.Code;
                }
            }
            
            // we are not doing the same check to case manager here, because leaving it empty would make it pass
            // validation - it is not a mandatory field. we don't want to lose it where it exists, so that check
            // is explicitly done during the validation step

            return details;
        }

        private static List<NotificationSite> AsSites(IEnumerable<dynamic> rawResultRawSites)
        {
            return rawResultRawSites
                .Select(AsNotificationSite)
                // Due to many->one mapping of legacy sites to ntbs sites, there might be clashes we need to deal with..
                .GroupBy(site => site.SiteId)
                .Select(clashingSites =>
                {
                    // .. to avoid data loss, we collect the freetext entered in all the possible members of the clash
                    var combinedDescription = String.Join(", ", clashingSites.Select(site => site.SiteDescription));
                    var canonicalSite = clashingSites.First();
                    canonicalSite.SiteDescription = combinedDescription;
                    return canonicalSite;
                })
                .FallbackIfEmpty(new NotificationSite
                {
                    SiteId = (int)SiteId.OTHER,
                    SiteDescription = "Unknown, no site specified in legacy system"
                })
                .ToList();
        }

        private static NotificationSite AsNotificationSite(dynamic result)
        {
            if (result.SiteId == null)
            {
                return null;
            }
            return new NotificationSite
            {
                SiteId = result.SiteId,
                SiteDescription = result.SiteDescription
            };
        }

        private static ImmunosuppressionDetails ExtractImmunosuppressionDetails(dynamic notification)
        {
            var details = new ImmunosuppressionDetails();
            details.Status = Converter.GetStatusFromString(notification.ImmunosuppressionStatus);
            if (details.Status != Status.Yes)
            {
                return details;
            }

            details.HasBioTherapy = Converter.GetNullableBoolValue((int?) notification.HasBioTherapy);
            details.HasTransplantation = Converter.GetNullableBoolValue((int?) notification.HasTransplantation);
            details.HasOther = Converter.GetNullableBoolValue((int?) notification.HasOther);

            if (details.HasBioTherapy == null && details.HasTransplantation == null && details.HasOther == null)
            {
                details.HasOther = true;
                details.OtherDescription = "No immunosuppression type was provided in the legacy record";
            }
            else
            {
                if (details.HasOther == true)
                {
                    if (!string.IsNullOrWhiteSpace(notification.OtherDescription))
                    {
                        var otherDescription = RemoveCharactersNotIn(
                            ValidationRegexes.CharacterValidationWithNumbersForwardSlashExtended,
                            notification.OtherDescription);
                        details.OtherDescription = otherDescription;
                    }
                    else
                    {
                        details.OtherDescription = "No description provided in the legacy system";
                    }
                }
            }

            return details;
        }

        private static ComorbidityDetails ExtractComorbidityDetails(dynamic notification)
        {
            var details = new ComorbidityDetails();
            details.DiabetesStatus = Converter.GetStatusFromString(notification.DiabetesStatus);
            details.LiverDiseaseStatus = Converter.GetStatusFromString(notification.LiverDiseaseStatus);
            details.RenalDiseaseStatus = Converter.GetStatusFromString(notification.RenalDiseaseStatus);
            details.HepatitisBStatus = Converter.GetStatusFromString(notification.HepatitisBStatus);
            details.HepatitisCStatus = Converter.GetStatusFromString(notification.HepatitisCStatus);
            return details;
        }

        private static ClinicalDetails ExtractClinicalDetails(dynamic notification)
        {
            var details = new ClinicalDetails();
            details.SymptomStartDate = notification.SymptomOnsetDate;
            details.FirstPresentationDate = notification.FirstPresentationDate;
            details.TBServicePresentationDate = notification.TbServicePresentationDate;
            details.DiagnosisDate = notification.DiagnosisDate ?? notification.StartOfTreatmentDate ?? notification.NotificationDate;
            details.DidNotStartTreatment = Converter.GetNullableBoolValue(notification.DidNotStartTreatment);
            details.TreatmentStartDate = notification.StartOfTreatmentDate;
            details.MDRTreatmentStartDate = notification.MDRTreatmentStartDate;
            details.IsSymptomatic = Converter.GetNullableBoolValue(notification.IsSymptomatic);
            details.IsPostMortem = Converter.GetNullableBoolValue(notification.IsPostMortem);
            details.HIVTestState = Converter.GetEnumValue<HIVTestStatus>((string) notification.HivTestStatus);
            details.DotStatus = Converter.GetEnumValue<DotStatus>((string) notification.DotStatus);
            details.EnhancedCaseManagementStatus = Converter.GetStatusFromString(notification.EnhancedCaseManagementStatus);
            details.BCGVaccinationState = Converter.GetStatusFromString(notification.BCGVaccinationState);
            details.BCGVaccinationYear = notification.BCGVaccinationYear;
            details.TreatmentRegimen = Converter.GetEnumValue<TreatmentRegimen>((string) notification.TreatmentRegimen);
            details.Notes = notification.Notes;
            return details;
        }

        private ContactTracing ExtractContactTracingDetails(dynamic rawNotification)
        {
            var details = new ContactTracing();
            details.AdultsIdentified = rawNotification.AdultsIdentified;
            details.ChildrenIdentified = rawNotification.ChildrenIdentified;
            details.AdultsScreened = rawNotification.AdultsScreened;
            details.ChildrenScreened = rawNotification.ChildrenScreened;
            details.AdultsActiveTB = rawNotification.AdultsActiveTB;
            details.ChildrenActiveTB = rawNotification.ChildrenActiveTB;
            details.AdultsLatentTB = rawNotification.AdultsLatentTB;
            details.ChildrenLatentTB = rawNotification.ChildrenLatentTB;
            details.AdultsStartedTreatment = rawNotification.AdultsStartedTreatment;
            details.ChildrenStartedTreatment = rawNotification.ChildrenStartedTreatment;
            details.AdultsFinishedTreatment = rawNotification.AdultsFinishedTreatment;
            details.ChildrenFinishedTreatment = rawNotification.ChildrenFinishedTreatment;
            return details;
        }

        private PatientTBHistory ExtractPatientTBHistory(dynamic rawNotification)
        {
            var details = new PatientTBHistory();
            details.PreviouslyHadTB = Converter.GetNullableBoolValue(rawNotification.PreviouslyHadTB);
            details.PreviousTBDiagnosisYear = rawNotification.PreviousTBDiagnosisYear;
            return details;
        }

        private static TravelDetails ExtractTravelDetails(dynamic notification)
        {
            bool? hasTravel = notification.HasTravel;
            var totalNumberOfCountries = hasTravel ?? false ? Converter.ToNullableInt(notification.travel_TotalNumberOfCountries) : null;

            var details = new TravelDetails();
            details.HasTravel = hasTravel;
            details.TotalNumberOfCountries = totalNumberOfCountries;
            details.Country1Id = notification.travel_Country1;
            details.Country2Id = notification.travel_Country2;
            details.Country3Id = notification.travel_Country3;
            details.StayLengthInMonths1 = notification.travel_StayLengthInMonths1;
            details.StayLengthInMonths2 = notification.travel_StayLengthInMonths2;
            details.StayLengthInMonths3 = notification.travel_StayLengthInMonths3;
            return details;
        }

        private static VisitorDetails ExtractVisitorDetails(dynamic notification)
        {
            bool? hasVisitor = notification.HasVisitor;
            var totalNumberOfCountries = hasVisitor ?? false ? Converter.ToNullableInt(notification.visitor_TotalNumberOfCountries) : null;

            var details = new VisitorDetails();
            details.HasVisitor = hasVisitor;
            details.TotalNumberOfCountries = totalNumberOfCountries;
            details.Country1Id = notification.visitor_Country1;
            details.Country2Id = notification.visitor_Country2;
            details.Country3Id = notification.visitor_Country3;
            details.StayLengthInMonths1 = notification.visitor_StayLengthInMonths1;
            details.StayLengthInMonths2 = notification.visitor_StayLengthInMonths2;
            details.StayLengthInMonths3 = notification.visitor_StayLengthInMonths3;
            return details;
        }

        private static PatientDetails ExtractPatientDetails(dynamic notification)
        {
            var addressRaw = string.Join(" \n",
                new string[]
                {
                    notification.Line1, 
                    notification.Line2,
                    notification.City,
                    notification.County,
                    notification.Postcode
                });
            var address = RemoveCharactersNotIn(
                ValidationRegexes.CharacterValidationWithNumbersForwardSlashAndNewLine,
                addressRaw);
            var givenName = RemoveCharactersNotIn(ValidationRegexes.CharacterValidation, notification.GivenName);
            var familyName = RemoveCharactersNotIn(ValidationRegexes.CharacterValidation, notification.FamilyName);
            var localPatientId = RemoveCharactersNotIn(
                ValidationRegexes.CharacterValidationWithNumbersForwardSlashExtended,
                notification.LocalPatientId);

            var details = new PatientDetails();
            details.FamilyName = familyName;
            details.GivenName = givenName;
            details.NhsNumber = notification.NhsNumber;
            details.NhsNumberNotKnown = notification.NhsNumberNotKnown == 1 || notification.NhsNumber == null;
            details.Dob = notification.DateOfBirth;
            details.YearOfUkEntry = notification.UkEntryYear;
            details.UkBorn = notification.UkBorn;
            details.CountryId = notification.BirthCountryId ?? Countries.UnknownId;
            details.LocalPatientId = localPatientId;
            details.Postcode = notification.Postcode;
            details.NoFixedAbode = notification.NoFixedAbode == 1;
            details.Address = address;
            details.EthnicityId = notification.NtbsEthnicGroupId ?? Ethnicities.NotStatedId;
            details.SexId = notification.NtbsSexId ?? Sexes.UnknownId;
            details.OccupationId = notification.NtbsOccupationId;
            details.OccupationOther = notification.NtbsOccupationFreeText;

            ForceValidNhsNumber(details);
            
            return details;
        }

        private static string RemoveCharactersNotIn(string matchingRegex, string input)
        {
            if (input == null) { return null; }

            // We assume the matching regex to be of the format "[someLettersHere]+"
            // and we are aiming to turn it into "[^someLettersHere]"
            var notMatchingRegex = matchingRegex
                .Remove(matchingRegex.Length - 1)
                .Insert(1, "^");
            return new Regex(notMatchingRegex).Replace(input, "");
        }

        private static void ForceValidNhsNumber(PatientDetails details)
        {
            var validationResults = new List<ValidationResult>();
            var context = new ValidationContext(details) { MemberName = nameof(PatientDetails.NhsNumber)};
            Validator.TryValidateProperty(details.NhsNumber, context, validationResults);
            if (validationResults.Any())
            {
                details.NhsNumber = null;
                details.NhsNumberNotKnown = true;
            }
        }

        private static SocialRiskFactors ExtractSocialRiskFactors(dynamic notification)
        {
            var factors = new SocialRiskFactors();
            factors.AlcoholMisuseStatus = Converter.GetStatusFromString(notification.AlcoholMisuseStatus);
            factors.MentalHealthStatus = Converter.GetStatusFromString(notification.MentalHealthStatus);
            factors.AsylumSeekerStatus = Converter.GetStatusFromString(notification.AsylumSeekerStatus);
            factors.ImmigrationDetaineeStatus = Converter.GetStatusFromString(notification.ImmigrationDetaineeStatus);
            
            factors.RiskFactorSmoking.Status = Converter.GetStatusFromString(notification.SmokingStatus);
            
            factors.RiskFactorDrugs.Status = Converter.GetStatusFromString(notification.riskFactorDrugs_Status);
            factors.RiskFactorDrugs.IsCurrent = Converter.GetNullableBoolValue((int?) notification.riskFactorDrugs_IsCurrent);
            factors.RiskFactorDrugs.InPastFiveYears = Converter.GetNullableBoolValue((int?) notification.riskFactorDrugs_InPastFiveYears);
            factors.RiskFactorDrugs.MoreThanFiveYearsAgo = Converter.GetNullableBoolValue((int?) notification.riskFactorDrugs_MoreThanFiveYearsAgo);
            
            factors.RiskFactorHomelessness.Status = Converter.GetStatusFromString(notification.riskFactorHomelessness_Status);
            factors.RiskFactorHomelessness.IsCurrent = Converter.GetNullableBoolValue((int?) notification.riskFactorHomelessness_IsCurrent);
            factors.RiskFactorHomelessness.InPastFiveYears = Converter.GetNullableBoolValue((int?) notification.riskFactorHomelessness_InPastFiveYears);
            factors.RiskFactorHomelessness.MoreThanFiveYearsAgo = Converter.GetNullableBoolValue((int?) notification.riskFactorHomelessness_MoreThanFiveYearsAgo);
            
            factors.RiskFactorImprisonment.Status = Converter.GetStatusFromString(notification.riskFactorImprisonment_Status);
            factors.RiskFactorImprisonment.IsCurrent = Converter.GetNullableBoolValue((int?) notification.riskFactorImprisonment_IsCurrent);
            factors.RiskFactorImprisonment.InPastFiveYears = Converter.GetNullableBoolValue((int?) notification.riskFactorImprisonment_InPastFiveYears);
            factors.RiskFactorImprisonment.MoreThanFiveYearsAgo = Converter.GetNullableBoolValue((int?) notification.riskFactorImprisonment_MoreThanFiveYearsAgo);
            return factors;
        }

        private static ManualTestResult AsManualTestResult(dynamic rawResult)
        {
            var manualTest = new ManualTestResult();
            manualTest.ManualTestTypeId = rawResult.ManualTestTypeId;
            manualTest.SampleTypeId = rawResult.SampleTypeId;
            manualTest.Result =  Converter.GetEnumValue<Result>(rawResult.Result);
            manualTest.TestDate = rawResult.TestDate;
            return manualTest;
        }

        private static SocialContextVenue AsSocialContextVenue(dynamic rawVenue)
        {
            var venue = new SocialContextVenue();
            venue.VenueTypeId = rawVenue.VenueTypeId;
            venue.Name = rawVenue.Name;
            venue.Address = rawVenue.Address;
            venue.Postcode = rawVenue.Postcode;
            venue.Frequency = Converter.GetEnumValue<Frequency>(rawVenue.Frequency);
            venue.DateFrom = rawVenue.DateFrom;
            venue.DateTo = rawVenue.DateTo;
            var details = RemoveCharactersNotIn(ValidationRegexes.CharacterValidationWithNumbersForwardSlashExtended, rawVenue.Details);
            venue.Details = details;
            return venue;
        }

        private static SocialContextAddress AsSocialContextAddress(dynamic rawAddress)
        {
            var address = new SocialContextAddress();
             address.Address = rawAddress.Address;
             address.Postcode = rawAddress.Postcode;
             address.DateFrom = rawAddress.DateFrom;
             address.DateTo = rawAddress.DateTo;
             var details = RemoveCharactersNotIn(ValidationRegexes.CharacterValidationWithNumbersForwardSlashExtended, rawAddress.Details);
             address.Details = details;
            return address;
        }
        
        private async Task<List<TreatmentEvent>> ExtractTreatmentEventsAsync(dynamic notification)
        {
            var treatmentEvents = new List<TreatmentEvent>();
            if (notification.IsPostMortem == 1)
            {
                var deathEventTreatmentOutcome = await _referenceDataRepository.GetTreatmentOutcomeForTypeAndSubType(
                    TreatmentOutcomeType.Died,
                    TreatmentOutcomeSubType.Unknown);
                treatmentEvents.Add(
                    new TreatmentEvent
                    {
                        EventDate = notification.DeathDate,
                        TreatmentEventType = TreatmentEventType.TreatmentOutcome,
                        TreatmentOutcomeId = deathEventTreatmentOutcome.TreatmentOutcomeId,
                        TreatmentOutcome = deathEventTreatmentOutcome
                    });
            }
            return treatmentEvents;
        }

        private async Task<TreatmentEvent> AsTransferEvent(dynamic rawEvent)
        {
            var ev = new TreatmentEvent();
            ev.EventDate = rawEvent.EventDate;
            ev.TreatmentEventType = Converter.GetEnumValue<TreatmentEventType>(rawEvent.TreatmentEventType);
            ev.CaseManagerUsername = rawEvent.CaseManager;

            // ReSharper disable once InvertIf
            if (rawEvent.HospitalId is Guid guid)
            {
                var tbService = (await _referenceDataRepository.GetTbServiceFromHospitalIdAsync(guid));
                if (tbService == null)
                {
                    Log.Warning(
                        $"No TB service exists for hospital with guid {guid} - treatment event recorded without a service");
                }
                else
                {
                    ev.TbServiceCode = tbService.Code;
                }
            }

            return ev;
        }

        private async Task<TreatmentEvent> AsOutcomeEvent(dynamic rawEvent)
        {
            var ev = new TreatmentEvent();
            ev.EventDate = rawEvent.EventDate;
            ev.TreatmentEventType = Converter.GetEnumValue<TreatmentEventType>(rawEvent.TreatmentEventType);
            ev.TreatmentOutcomeId = rawEvent.TreatmentOutcomeId;
            ev.CaseManagerUsername = rawEvent.CaseManager;
            ev.Note = rawEvent.Note;

            // ReSharper disable once InvertIf
            if (rawEvent.HospitalId is Guid guid)
            {
                var tbService = (await _referenceDataRepository.GetTbServiceFromHospitalIdAsync(guid));
                if (tbService == null)
                {
                    Log.Warning(
                        $"No TB service exists for hospital with guid {guid} - treatment event recorded without a service");
                }
                else
                {
                    ev.TbServiceCode = tbService.Code;
                }
            }

            return ev;
        }
    }
}
