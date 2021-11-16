using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Hangfire.Server;
using MoreLinq;
using ntbs_service.DataAccess;
using ntbs_service.DataMigration.RawModels;
using ntbs_service.Helpers;
using ntbs_service.Models;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;
using ntbs_service.Models.ReferenceEntities;
using ntbs_service.Models.Validations;
using ntbs_service.Services;
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
            int runId, List<string> notificationId);
        Task<IEnumerable<IList<Notification>>> GetNotificationsGroupedByPatient(PerformContext context,
            int runId, DateTime rangeStartDate, DateTime endStartDate);
    }

    public class NotificationMapper : INotificationMapper
    {
        private readonly IMigrationRepository _migrationRepository;
        private readonly IReferenceDataRepository _referenceDataRepository;
        private readonly IImportLogger _logger;
        private readonly TreatmentOutcome _postMortemOutcomeType;
        private readonly List<int> _diedOutcomeIds;
        private readonly IPostcodeService _postcodeService;
        private readonly ICaseManagerImportService _caseManagerImportService;
        private readonly ITreatmentEventMapper _treatmentEventMapper;

        public NotificationMapper(IMigrationRepository migrationRepository,
            IReferenceDataRepository referenceDataRepository,
            IImportLogger logger,
            IPostcodeService postcodeService,
            ICaseManagerImportService caseManagerImportService,
            ITreatmentEventMapper treatmentEventMapper)
        {
            _migrationRepository = migrationRepository;
            _referenceDataRepository = referenceDataRepository;
            _logger = logger;
            _postcodeService = postcodeService;
            _caseManagerImportService = caseManagerImportService;
            _treatmentEventMapper = treatmentEventMapper;

            // These are database-based values, but static from the runtime point of view, so we fetch them once here
            _postMortemOutcomeType = _referenceDataRepository.GetTreatmentOutcomeForTypeAndSubType(
                TreatmentOutcomeType.Died,
                TreatmentOutcomeSubType.Unknown).Result;
            _diedOutcomeIds = _referenceDataRepository.GetTreatmentOutcomesForType(TreatmentOutcomeType.Died)
                .Result
                .Select(outcome => outcome.TreatmentOutcomeId)
                .ToList();
        }

        public async Task<IEnumerable<IList<Notification>>> GetNotificationsGroupedByPatient(PerformContext context,
            int runId, DateTime rangeStartDate, DateTime endStartDate)
        {
            var groupedIds = await _migrationRepository.GetGroupedNotificationIdsByDate(rangeStartDate, endStartDate);

            return await GetGroupedResultsAsNotificationAsync(groupedIds, context, runId);
        }

        public async Task<IEnumerable<IList<Notification>>> GetNotificationsGroupedByPatient(PerformContext context,
            int runId, List<string> notificationIds)
        {
            var groupedIds = await _migrationRepository.GetGroupedNotificationIdsById(notificationIds);

            return await GetGroupedResultsAsNotificationAsync(groupedIds, context, runId);
        }

        private async Task<IEnumerable<IList<Notification>>> GetGroupedResultsAsNotificationAsync(IEnumerable<IGrouping<string, string>> groupedIds, PerformContext context, int runId)
        {
            var resultList = new List<IList<Notification>>();
            foreach (var group in groupedIds)
            {
                var legacyIds = group.ToList();
                _logger.LogInformation(context, runId, $"Fetching data for legacy notifications {string.Join(", ", legacyIds)}");
                var legacyNotifications = _migrationRepository.GetNotificationsById(legacyIds);
                var sitesOfDisease = _migrationRepository.GetNotificationSites(legacyIds);
                var manualTestResults = _migrationRepository.GetManualTestResults(legacyIds);
                var socialContextVenues = _migrationRepository.GetSocialContextVenues(legacyIds);
                var socialContextAddresses = _migrationRepository.GetSocialContextAddresses(legacyIds);
                var transferEvents = _migrationRepository.GetTransferEvents(legacyIds);
                var outcomeEvents = _migrationRepository.GetOutcomeEvents(legacyIds);
                var mbovisAnimalExposure = _migrationRepository.GetMigrationMBovisAnimalExposure(legacyIds);
                var mbovisExposureToKnownCase = _migrationRepository.GetMigrationMBovisExposureToKnownCase(legacyIds);
                var mbovisOccupationExposures = _migrationRepository.GetMigrationMBovisOccupationExposures(legacyIds);
                var mbovisUnpasteurisedMilkConsumption = _migrationRepository.GetMigrationMBovisUnpasteurisedMilkConsumption(legacyIds);

                var notificationsForGroup = await CombineDataForGroup(
                    legacyIds,
                    (await legacyNotifications).ToList(),
                    (await sitesOfDisease).ToList(),
                    (await manualTestResults).ToList(),
                    (await socialContextVenues).ToList(),
                    (await socialContextAddresses).ToList(),
                    (await transferEvents).ToList(),
                    (await outcomeEvents).ToList(),
                    (await mbovisAnimalExposure).ToList(),
                    (await mbovisExposureToKnownCase).ToList(),
                    (await mbovisOccupationExposures).ToList(),
                    (await mbovisUnpasteurisedMilkConsumption).ToList(),
                    context,
                    runId
                );
                resultList.Add(notificationsForGroup);
            }
            return resultList;
        }

        private async Task<IList<Notification>> CombineDataForGroup(IEnumerable<string> legacyIds,
            List<MigrationDbNotification> notifications,
            List<MigrationDbSite> sitesOfDisease,
            List<MigrationDbManualTest> manualTestResults,
            List<MigrationDbSocialContextVenue> socialContextVenues,
            List<MigrationDbSocialContextAddress> socialContextAddresses,
            List<MigrationDbTransferEvent> transferEvents,
            List<MigrationDbOutcomeEvent> outcomeEvents,
            List<MigrationDbMBovisAnimal> mbovisAnimalExposures,
            List<MigrationDbMBovisKnownCase> mbovisExposureToKnownCase,
            List<MigrationDbMBovisOccupation> mbovisOccupationExposures,
            List<MigrationDbMBovisMilkConsumption> mbovisUnpasteurisedMilkConsumption,
            PerformContext context,
            int runId)
        {
            var notificationsToReturn = new List<Notification>();

            foreach (var id in legacyIds)
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
                var notificationTransferEvents = new List<TreatmentEvent>();
                foreach (var transfer in transferEvents.Where(sc => sc.OldNotificationId == id))
                {
                    notificationTransferEvents.Add(await _treatmentEventMapper.AsTransferEvent(transfer, context, runId));
                }
                var notificationOutcomeEvents = new List<TreatmentEvent>();
                foreach (var outcome in outcomeEvents.Where(sc => sc.OldNotificationId == id))
                {
                    notificationOutcomeEvents.Add(await _treatmentEventMapper.AsOutcomeEvent(outcome, context, runId));
                }
                var notificationMBovisAnimalExposures = mbovisAnimalExposures
                    .Where(sc => sc.OldNotificationId == id)
                    .Select(AsMBovisAnimalExposure)
                    .ToList();
                var notificationMBovisExposureToKnownCase = mbovisExposureToKnownCase
                    .Where(sc => sc.OldNotificationId == id)
                    .Select(AsMBovisExposureToKnownCase)
                    .ToList();
                var notificationMBovisOccupationExposures = mbovisOccupationExposures
                    .Where(sc => sc.OldNotificationId == id)
                    .Select(AsMBovisOccupationExposure)
                    .ToList();
                var notificationMBovisUnpasteurisedMilkConsumption = mbovisUnpasteurisedMilkConsumption
                    .Where(sc => sc.OldNotificationId == id)
                    .Select(AsMBovisUnpasteurisedMilkConsumption)
                    .ToList();

                var notification = await AsNotificationAsync(rawNotification, context, runId);
                notification.NotificationSites = notificationSites;
                notification.TestData = new TestData
                {
                    HasTestCarriedOut = notificationTestResults.Any(),
                    ManualTestResults = notificationTestResults
                };
                notification.SocialContextAddresses = notificationSocialContextAddresses;
                notification.SocialContextVenues = notificationSocialContextVenues;
                notification.TreatmentEvents = CombineTreatmentEvents(notification,
                    rawNotification,
                    notificationTransferEvents,
                    notificationOutcomeEvents);

                notification.MBovisDetails = ExtractMBovisDetails(notificationMBovisAnimalExposures,
                    notificationMBovisExposureToKnownCase,
                    notificationMBovisOccupationExposures,
                    notificationMBovisUnpasteurisedMilkConsumption);

                if (notification.ShouldBeClosed())
                {
                    notification.NotificationStatus = NotificationStatus.Closed;
                }

                notificationsToReturn.Add(notification);
            }

            return notificationsToReturn;
        }

        private List<TreatmentEvent> CombineTreatmentEvents(Notification notification,
            MigrationDbNotification rawNotification,
            IEnumerable<TreatmentEvent> notificationTransferEvents,
            IEnumerable<TreatmentEvent> notificationOutcomeEvents)
        {
            return notification.ClinicalDetails.IsPostMortem == true
                ? TreatmentEventsForPostMortemNotification(rawNotification)
                : TreatmentEventsForPreMortemNotification(
                    notification,
                    rawNotification,
                    notificationTransferEvents,
                    notificationOutcomeEvents);
        }

        private List<TreatmentEvent> TreatmentEventsForPostMortemNotification(MigrationDbNotification rawNotification)
        {
            // For post mortem cases the death event is the ONLY event we want to import so the final outcome is
            // correctly reported.
            return new List<TreatmentEvent>
            {
                CreateDerivedDeathEvent(rawNotification)
            };
        }

        private List<TreatmentEvent> TreatmentEventsForPreMortemNotification(Notification notification,
            MigrationDbNotification rawNotification,
            IEnumerable<TreatmentEvent> notificationTransferEvents,
            IEnumerable<TreatmentEvent> notificationOutcomeEvents)
        {
            var treatmentEvents = new List<TreatmentEvent>();

            if (notification.NotificationStatus != NotificationStatus.Draft)
            {
                treatmentEvents.Add(NotificationHelper.CreateTreatmentStartEvent(notification));
            }

            treatmentEvents.AddRange(notificationTransferEvents);
            treatmentEvents.AddRange(notificationOutcomeEvents);

            // If there is a death date but no death treatment event, add one in
            if (rawNotification.DeathDate != null
                && !treatmentEvents.Any(e => _diedOutcomeIds.Contains(e.TreatmentOutcomeId ?? 0)))
            {
                treatmentEvents.Add(CreateDerivedDeathEvent(rawNotification));
            }

            return treatmentEvents;
        }

        private TreatmentEvent CreateDerivedDeathEvent(MigrationDbNotification rawNotification) =>
            new TreatmentEvent
            {
                EventDate = rawNotification.DeathDate,
                TreatmentEventType = TreatmentEventType.TreatmentOutcome,
                TreatmentOutcomeId = _postMortemOutcomeType.TreatmentOutcomeId,
                TreatmentOutcome = _postMortemOutcomeType
            };

        private MBovisDetails ExtractMBovisDetails(ICollection<MBovisAnimalExposure> animalExposures,
            ICollection<MBovisExposureToKnownCase> exposureToKnownCase,
            ICollection<MBovisOccupationExposure> occupationExposures,
            ICollection<MBovisUnpasteurisedMilkConsumption> unpasteurisedMilkConsumption)
        {
            var mbovisDetails = new MBovisDetails
            {
                MBovisAnimalExposures = animalExposures,
                MBovisExposureToKnownCases = exposureToKnownCase,
                MBovisOccupationExposures = occupationExposures,
                MBovisUnpasteurisedMilkConsumptions = unpasteurisedMilkConsumption,

                AnimalExposureStatus = null,
                ExposureToKnownCasesStatus = null,
                OccupationExposureStatus = null,
                UnpasteurisedMilkConsumptionStatus = null,
            };

            var anyMBovisExposure = animalExposures.Any()
                                    || exposureToKnownCase.Any()
                                    || occupationExposures.Any()
                                    || unpasteurisedMilkConsumption.Any();
            // If there are no M. bovis exposure events then leave all the statuses as null
            if (!anyMBovisExposure)
            {
                return mbovisDetails;
            }

            // If there are events, then set all statuses to either Yes or Unknown (for we don't know if an absence of
            // data in the old DB was due to a lack of information, or because there was definitely no exposure)
            mbovisDetails.AnimalExposureStatus = animalExposures.Any() ? Status.Yes : Status.Unknown;
            mbovisDetails.ExposureToKnownCasesStatus = exposureToKnownCase.Any() ? Status.Yes : Status.Unknown;
            mbovisDetails.OccupationExposureStatus = occupationExposures.Any() ? Status.Yes : Status.Unknown;
            mbovisDetails.UnpasteurisedMilkConsumptionStatus =
                unpasteurisedMilkConsumption.Any() ? Status.Yes : Status.Unknown;

            return mbovisDetails;
        }

        private async Task<Notification> AsNotificationAsync(MigrationDbNotification rawNotification,
            PerformContext context, int runId)
        {
            var notification = new Notification
            {
                LegacySource = rawNotification.Source,
                ETSID = rawNotification.EtsId,
                LTBRID = rawNotification.LtbrId,
                LTBRPatientId = rawNotification.LtbrPatientId,
                NotificationDate = rawNotification.NotificationDate,
                CreationDate = DateTime.Now
            };
            if (Converter.GetNullableBoolValue(rawNotification.IsDenotified) == true)
            {
                notification.NotificationStatus = NotificationStatus.Denotified;
                notification.DenotificationDetails = new DenotificationDetails
                {
                    DateOfDenotification = rawNotification.DenotificationDate ?? DateTime.Now
                };
                SetDenotificationReasonAndDescription(rawNotification, notification.DenotificationDetails);
            }
            else
            {
                notification.NotificationStatus = NotificationStatus.Notified;
            }

            notification.PatientDetails = await ExtractPatientDetails(rawNotification,
                notification.LegacyId,
                context,
                runId);
            notification.ClinicalDetails = ExtractClinicalDetails(rawNotification);
            notification.TravelDetails = ExtractTravelDetails(rawNotification);
            notification.VisitorDetails = ExtractVisitorDetails(rawNotification);
            notification.ComorbidityDetails = ExtractComorbidityDetails(rawNotification);
            notification.ImmunosuppressionDetails = ExtractImmunosuppressionDetails(rawNotification);
            notification.SocialRiskFactors = ExtractSocialRiskFactors(rawNotification);
            notification.HospitalDetails = await ExtractHospitalDetailsAsync(rawNotification, context, runId);
            notification.ContactTracing = ExtractContactTracingDetails(rawNotification);
            notification.PreviousTbHistory = ExtractPreviousTbHistory(rawNotification);
            notification.MDRDetails = ExtractMdrDetailsAsync(rawNotification);

            return notification;
        }

        private async Task<HospitalDetails> ExtractHospitalDetailsAsync(MigrationDbNotification rawNotification, PerformContext context, int runId)
        {
            var details = new HospitalDetails
            {
                HospitalId = rawNotification.NtbsHospitalId
            };
            var consultant = RemoveCharactersNotIn(ValidationRegexes.CharacterValidationAsciiBasic, rawNotification.Consultant);
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

            if (!string.IsNullOrEmpty(rawNotification.CaseManager))
            {
                await _caseManagerImportService.ImportOrUpdateLegacyUser(rawNotification.CaseManager, details.TBServiceCode, context, runId);
                details.CaseManagerId = (await _referenceDataRepository.GetUserByUsernameAsync(rawNotification.CaseManager)).Id;
            }

            // we are not doing the same check to case manager here, because leaving it empty would make it pass
            // validation - it is not a mandatory field. we don't want to lose it where it exists, so that check
            // is explicitly done during the validation step

            return details;
        }

        private static List<NotificationSite> AsSites(IEnumerable<MigrationDbSite> rawResultRawSites)
        {
            return rawResultRawSites
                .Select(AsNotificationSite)
                // Due to many->one mapping of legacy sites to ntbs sites, there might be clashes we need to deal with..
                .GroupBy(site => site.SiteId)
                .Select(clashingSites =>
                {
                    // .. to avoid data loss, we collect the free text entered in all the possible members of the clash
                    var combinedDescription = string.Join(", ", clashingSites.Select(site => site.SiteDescription));
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

        private static NotificationSite AsNotificationSite(MigrationDbSite result)
        {
            if (result.SiteId == null)
            {
                return null;
            }

            var site = new NotificationSite
            {
                SiteId = (int)result.SiteId,
                SiteDescription = result.SiteDescription
            };
            return site;
        }

        private static ImmunosuppressionDetails ExtractImmunosuppressionDetails(MigrationDbNotification notification)
        {
            var details = new ImmunosuppressionDetails
            {
                Status = Converter.GetStatusFromString(notification.ImmunosuppressionStatus)
            };
            if (details.Status != Status.Yes)
            {
                return details;
            }

            details.HasBioTherapy = Converter.GetNullableBoolValue(notification.HasBioTherapy);
            details.HasTransplantation = Converter.GetNullableBoolValue(notification.HasTransplantation);
            details.HasOther = Converter.GetNullableBoolValue(notification.HasOther);
            details.OtherDescription = RemoveCharactersNotIn(
                ValidationRegexes.CharacterValidationAsciiBasic,
                notification.OtherDescription);

            if (details.HasOther == true && string.IsNullOrWhiteSpace(details.OtherDescription))
            {
                details.OtherDescription = "No description provided in the legacy system";
            }

            if (details.HasBioTherapy != true && details.HasTransplantation != true)
            {
                details.HasOther = true;
                details.OtherDescription = string.IsNullOrWhiteSpace(details.OtherDescription)
                    ? "No immunosuppression type was provided in the legacy record"
                    : details.OtherDescription;
            }

            // Ensure that if a description exists then HasOther is ticked
            if (!string.IsNullOrWhiteSpace(details.OtherDescription))
            {
                details.HasOther = true;
            }

            return details;
        }

        private static ComorbidityDetails ExtractComorbidityDetails(MigrationDbNotification notification)
        {
            var details = new ComorbidityDetails
            {
                DiabetesStatus = Converter.GetStatusFromString(notification.DiabetesStatus),
                LiverDiseaseStatus = Converter.GetStatusFromString(notification.LiverDiseaseStatus),
                RenalDiseaseStatus = Converter.GetStatusFromString(notification.RenalDiseaseStatus),
                HepatitisBStatus = Converter.GetStatusFromString(notification.HepatitisBStatus),
                HepatitisCStatus = Converter.GetStatusFromString(notification.HepatitisCStatus)
            };
            return details;
        }

        private static ClinicalDetails ExtractClinicalDetails(MigrationDbNotification notification)
        {
            var details = new ClinicalDetails
            {
                SymptomStartDate = notification.SymptomOnsetDate,
                FirstPresentationDate = notification.FirstPresentationDate,
                TBServicePresentationDate = notification.TbServicePresentationDate,
                DiagnosisDate = notification.DiagnosisDate ?? notification.StartOfTreatmentDate ?? notification.NotificationDate,
                StartedTreatment = !Converter.GetNullableBoolValue(notification.DidNotStartTreatment),
                TreatmentStartDate = notification.StartOfTreatmentDate,
                IsSymptomatic = Converter.GetNullableBoolValue(notification.IsSymptomatic),
                HomeVisitCarriedOut = Converter.GetStatusFromString(notification.HomeVisitCarriedOut),
                FirstHomeVisitDate = notification.FirstHomeVisitDate,
                HealthcareSetting = Converter.GetEnumValue<HealthcareSetting>(notification.HealthcareSetting),
                HealthcareDescription = notification.HealthcareDescription,
                IsPostMortem = Converter.GetNullableBoolValue(notification.IsPostMortem),
                HIVTestState = Converter.GetEnumValue<HIVTestStatus>(notification.HivTestStatus),
                IsDotOffered = Converter.GetStatusFromString(notification.IsDotOffered),
                DotStatus = Converter.GetEnumValue<DotStatus>(notification.DotStatus),
                EnhancedCaseManagementStatus = Converter.GetStatusFromString(notification.EnhancedCaseManagementStatus),
                BCGVaccinationState = Converter.GetStatusFromString(notification.BCGVaccinationState),
                BCGVaccinationYear = notification.BCGVaccinationYear,
                TreatmentRegimen = Converter.GetEnumValue<TreatmentRegimen>(notification.TreatmentRegimen),
                HealthProtectionTeamReferenceNumber = RemoveCharactersNotIn(ValidationRegexes.CharacterValidationLocalPatientId, notification.HealthProtectionTeamReferenceNumber),
                Notes = notification.Notes
            };
            return details;
        }

        private ContactTracing ExtractContactTracingDetails(MigrationDbNotification rawNotification)
        {
            var details = new ContactTracing
            {
                AdultsIdentified = rawNotification.AdultsIdentified,
                ChildrenIdentified = rawNotification.ChildrenIdentified,
                AdultsScreened = rawNotification.AdultsScreened,
                ChildrenScreened = rawNotification.ChildrenScreened,
                AdultsActiveTB = rawNotification.AdultsActiveTB,
                ChildrenActiveTB = rawNotification.ChildrenActiveTB,
                AdultsLatentTB = rawNotification.AdultsLatentTB,
                ChildrenLatentTB = rawNotification.ChildrenLatentTB,
                AdultsStartedTreatment = rawNotification.AdultsStartedTreatment,
                ChildrenStartedTreatment = rawNotification.ChildrenStartedTreatment,
                AdultsFinishedTreatment = rawNotification.AdultsFinishedTreatment,
                ChildrenFinishedTreatment = rawNotification.ChildrenFinishedTreatment
            };
            return details;
        }

        private PreviousTbHistory ExtractPreviousTbHistory(MigrationDbNotification rawNotification)
        {
            var details = new PreviousTbHistory
            {
                PreviouslyHadTb = Converter.GetStatusFromString(rawNotification.PreviouslyHadTb),
                PreviousTbDiagnosisYear = rawNotification.PreviousTbDiagnosisYear,
                PreviouslyTreated = Converter.GetStatusFromString(rawNotification.PreviouslyTreated),
                PreviousTreatmentCountryId = rawNotification.PreviousTreatmentCountryId
            };
            return details;
        }

        private static TravelDetails ExtractTravelDetails(MigrationDbNotification notification)
        {
            var hasTravel = Converter.GetStatusFromString(notification.HasTravel);
            var totalEnteredByUser = Converter.ToNullableInt(notification.travel_TotalNumberOfCountries);
            var countryCount = new List<int?>
                {
                    notification.travel_Country1, notification.travel_Country2, notification.travel_Country3
                }.Distinct()
                .Count(c => c != null);
            var totalNumberOfCountries = GetCountryTotalOrNull(hasTravel, totalEnteredByUser, countryCount);

            var details = new TravelDetails
            {
                HasTravel = hasTravel,
                TotalNumberOfCountries = totalNumberOfCountries,
                Country1Id = notification.travel_Country1,
                Country2Id = notification.travel_Country2,
                Country3Id = notification.travel_Country3,
                StayLengthInMonths1 = notification.travel_StayLengthInMonths1,
                StayLengthInMonths2 = notification.travel_StayLengthInMonths2,
                StayLengthInMonths3 = notification.travel_StayLengthInMonths3
            };
            RemoveDuplicateCountries(details);
            return details;
        }

        private static VisitorDetails ExtractVisitorDetails(MigrationDbNotification notification)
        {
            var hasVisitor = Converter.GetStatusFromString(notification.HasVisitor);
            var totalEnteredByUser = Converter.ToNullableInt(notification.visitor_TotalNumberOfCountries);
            var countryCount = new List<int?>
                    {
                        notification.visitor_Country1, notification.visitor_Country2, notification.visitor_Country3
                    }.Distinct()
                    .Count(c => c != null);
            var totalNumberOfCountries = GetCountryTotalOrNull(hasVisitor, totalEnteredByUser, countryCount);

            var details = new VisitorDetails
            {
                HasVisitor = hasVisitor,
                TotalNumberOfCountries = totalNumberOfCountries,
                Country1Id = notification.visitor_Country1,
                Country2Id = notification.visitor_Country2,
                Country3Id = notification.visitor_Country3,
                StayLengthInMonths1 = notification.visitor_StayLengthInMonths1,
                StayLengthInMonths2 = notification.visitor_StayLengthInMonths2,
                StayLengthInMonths3 = notification.visitor_StayLengthInMonths3
            };
            RemoveDuplicateCountries(details);
            return details;
        }

        private static void RemoveDuplicateCountries(ITravelOrVisitorDetails details)
        {
            if (details.Country1Id != null && details.Country1Id == details.Country2Id)
            {
                if (details.StayLengthInMonths1 != null && details.StayLengthInMonths2 != null)
                {
                    details.StayLengthInMonths1 = Math.Max(details.StayLengthInMonths1.Value,
                        details.StayLengthInMonths2.Value);
                }

                details.Country2Id = null;
                details.StayLengthInMonths2 = null;
            }

            if (details.Country1Id != null && details.Country1Id == details.Country3Id)
            {
                if (details.StayLengthInMonths1 != null && details.StayLengthInMonths3 != null)
                {
                    details.StayLengthInMonths1 = Math.Max(details.StayLengthInMonths1.Value,
                        details.StayLengthInMonths3.Value);
                }

                details.Country3Id = null;
                details.StayLengthInMonths3 = null;
            }

            if (details.Country2Id != null && details.Country2Id == details.Country3Id)
            {
                if (details.StayLengthInMonths2 != null && details.StayLengthInMonths3 != null)
                {
                    details.StayLengthInMonths2 = Math.Max(details.StayLengthInMonths2.Value,
                        details.StayLengthInMonths3.Value);
                }

                details.Country3Id = null;
                details.StayLengthInMonths3 = null;
            }

            if (details.Country2Id == null && details.Country3Id != null)
            {
                // If country 2 was removed, but not 3, then country 3 need to move to 2
                details.Country2Id = details.Country3Id;
                details.StayLengthInMonths2 = details.StayLengthInMonths3;
                details.Country3Id = null;
                details.StayLengthInMonths3 = null;
            }
        }

        private async Task<PatientDetails> ExtractPatientDetails(MigrationDbNotification notification, string legacyId,
            PerformContext context, int runId)
        {
            var addressLines = new List<string>
            {
                notification.Line1, notification.Line2, notification.City, notification.County
            };
            var addressRaw = string.Join("\n",
                addressLines.Where(line => !string.IsNullOrEmpty(line)).Select(line => line.Trim()));
            var address = RemoveCharactersNotIn(
                ValidationRegexes.CharacterValidationWithNumbersForwardSlashAndNewLine,
                addressRaw);
            var givenName = RemoveCharactersNotIn(ValidationRegexes.CharacterValidation, notification.GivenName);
            var familyName = RemoveCharactersNotIn(ValidationRegexes.CharacterValidation, notification.FamilyName);
            var localPatientId = RemoveCharactersNotIn(
                ValidationRegexes.CharacterValidationLocalPatientId,
                notification.LocalPatientId);

            var details = new PatientDetails
            {
                FamilyName = familyName,
                GivenName = givenName,
                NhsNumber = notification.NhsNumber,
                NhsNumberNotKnown = notification.NhsNumberNotKnown == 1 || notification.NhsNumber == null,
                Dob = notification.DateOfBirth,
                YearOfUkEntry = notification.UkEntryYear,
                CountryId = notification.BirthCountryId ?? Countries.UnknownId,
                LocalPatientId = localPatientId,
                Postcode = notification.Postcode,
                PostcodeLookup = await _postcodeService.FindPostcodeAsync(notification.Postcode)
            };
            details.PostcodeToLookup = details.PostcodeLookup?.Postcode;
            details.NoFixedAbode = notification.NoFixedAbode == 1;
            details.Address = address;
            details.EthnicityId = notification.NtbsEthnicGroupId ?? Ethnicities.NotStatedId;
            details.SexId = notification.NtbsSexId ?? Sexes.UnknownId;
            details.OccupationId = notification.NtbsOccupationId;
            details.OccupationOther = RemoveCharactersNotIn(
                ValidationRegexes.CharacterValidationAsciiBasic,
                notification.OccupationFreetext);

            if (details.Postcode != null && details.PostcodeToLookup == null)
            {
                var warningMessage = $"{typeof(PatientDetails).GetDisplayName()}: {ValidationMessages.PostcodeNotFound}";
                await _logger.LogNotificationWarning(context, runId, legacyId, warningMessage);
            }

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
            var context = new ValidationContext(details) { MemberName = nameof(PatientDetails.NhsNumber) };
            Validator.TryValidateProperty(details.NhsNumber, context, validationResults);
            if (validationResults.Any())
            {
                details.NhsNumber = null;
                details.NhsNumberNotKnown = true;
            }
        }

        private static SocialRiskFactors ExtractSocialRiskFactors(MigrationDbNotification notification)
        {
            var factors = new SocialRiskFactors
            {
                AlcoholMisuseStatus = Converter.GetStatusFromString(notification.AlcoholMisuseStatus),
                MentalHealthStatus = Converter.GetStatusFromString(notification.MentalHealthStatus),
                AsylumSeekerStatus = Converter.GetStatusFromString(notification.AsylumSeekerStatus),
                ImmigrationDetaineeStatus = Converter.GetStatusFromString(notification.ImmigrationDetaineeStatus)
            };

            factors.RiskFactorSmoking.Status = Converter.GetStatusFromString(notification.SmokingStatus);

            ExtractRiskFactor(factors.RiskFactorDrugs,
                notification.riskFactorDrugs_Status,
                notification.riskFactorDrugs_IsCurrent,
                notification.riskFactorDrugs_InPastFiveYears,
                notification.riskFactorDrugs_MoreThanFiveYearsAgo);

            ExtractRiskFactor(factors.RiskFactorHomelessness,
                notification.riskFactorHomelessness_Status,
                notification.riskFactorHomelessness_IsCurrent,
                notification.riskFactorHomelessness_InPastFiveYears,
                notification.riskFactorHomelessness_MoreThanFiveYearsAgo);

            ExtractRiskFactor(factors.RiskFactorImprisonment,
                notification.riskFactorImprisonment_Status,
                notification.riskFactorImprisonment_IsCurrent,
                notification.riskFactorImprisonment_InPastFiveYears,
                notification.riskFactorImprisonment_MoreThanFiveYearsAgo);

            return factors;
        }

        private static void ExtractRiskFactor(RiskFactorDetails riskFactor,
            string status,
            int? isCurrent,
            int? inPastFiveYears,
            int? moreThanFiveYearsAgo)
        {
            riskFactor.Status = Converter.GetStatusFromString(status);
            if (riskFactor.Status != Status.Yes)
            {
                return;
            }

            riskFactor.IsCurrent = Converter.GetNullableBoolValue(isCurrent);
            riskFactor.InPastFiveYears = Converter.GetNullableBoolValue(inPastFiveYears);
            riskFactor.MoreThanFiveYearsAgo = Converter.GetNullableBoolValue(moreThanFiveYearsAgo);
        }

        private static ManualTestResult AsManualTestResult(MigrationDbManualTest rawResult)
        {
            var manualTest = new ManualTestResult
            {
                ManualTestTypeId = rawResult.ManualTestTypeId,
                SampleTypeId = rawResult.SampleTypeId,
                Result = Converter.GetEnumValue<Result>(rawResult.Result),
                TestDate = rawResult.TestDate
            };
            return manualTest;
        }

        private static SocialContextVenue AsSocialContextVenue(MigrationDbSocialContextVenue rawVenue)
        {
            var venue = new SocialContextVenue
            {
                VenueTypeId = rawVenue.VenueTypeId,
                Name = rawVenue.Name,
                Address = rawVenue.Address,
                Postcode = rawVenue.Postcode,
                Frequency = Converter.GetEnumValue<Frequency>(rawVenue.Frequency),
                DateFrom = rawVenue.DateFrom,
                DateTo = rawVenue.DateTo
            };
            var details = RemoveCharactersNotIn(ValidationRegexes.CharacterValidationAsciiBasic, rawVenue.Details);
            venue.Details = details;
            return venue;
        }

        private static SocialContextAddress AsSocialContextAddress(MigrationDbSocialContextAddress rawAddress)
        {
            var address = new SocialContextAddress
            {
                Address = rawAddress.Address,
                Postcode = rawAddress.Postcode,
                DateFrom = rawAddress.DateFrom,
                DateTo = rawAddress.DateTo
            };
            var details = RemoveCharactersNotIn(ValidationRegexes.CharacterValidationAsciiBasic, rawAddress.Details);
            address.Details = details;
            return address;
        }

        private static MBovisAnimalExposure AsMBovisAnimalExposure(MigrationDbMBovisAnimal rawData)
        {
            var animalExposure = new MBovisAnimalExposure
            {
                YearOfExposure = rawData.YearOfExposure,
                AnimalType = Converter.GetEnumValue<AnimalType>(rawData.AnimalType),
                Animal = rawData.Animal,
                AnimalTbStatus = Converter.GetEnumValue<AnimalTbStatus>(rawData.AnimalTbStatus),
                ExposureDuration = rawData.ExposureDuration,
                CountryId = rawData.CountryId,
                OtherDetails = rawData.OtherDetails
            };
            return animalExposure;
        }

        private static MBovisExposureToKnownCase AsMBovisExposureToKnownCase(MigrationDbMBovisKnownCase rawData)
        {
            var caseExposure = new MBovisExposureToKnownCase
            {
                YearOfExposure = rawData.YearOfExposure,
                ExposureSetting = Converter.GetEnumValue<ExposureSetting>(rawData.ExposureSetting),
                ExposureNotificationId = rawData.ExposureNotificationId,
                NotifiedToPheStatus = Converter.GetStatusFromString(rawData.NotifiedToPheStatus) ?? Status.Unknown,
                OtherDetails = rawData.OtherDetails
            };
            return caseExposure;
        }

        private static MBovisOccupationExposure AsMBovisOccupationExposure(MigrationDbMBovisOccupation rawData)
        {
            var occupationExposure = new MBovisOccupationExposure
            {
                YearOfExposure = rawData.YearOfExposure,
                OccupationSetting = Converter.GetEnumValue<OccupationSetting>(rawData.OccupationSetting),
                OccupationDuration = rawData.OccupationDuration,
                CountryId = rawData.CountryId,
                OtherDetails = rawData.OtherDetails
            };
            return occupationExposure;
        }

        private static MBovisUnpasteurisedMilkConsumption AsMBovisUnpasteurisedMilkConsumption(MigrationDbMBovisMilkConsumption rawData)
        {
            var milkConsumption = new MBovisUnpasteurisedMilkConsumption
            {
                YearOfConsumption = rawData.YearOfConsumption,
                MilkProductType = Converter.GetEnumValue<MilkProductType>(rawData.MilkProductType),
                ConsumptionFrequency =
                Converter.GetEnumValue<ConsumptionFrequency>(rawData.ConsumptionFrequency),
                CountryId = rawData.CountryId,
                OtherDetails = rawData.OtherDetails
            };
            return milkConsumption;
        }

        private MDRDetails ExtractMdrDetailsAsync(MigrationDbNotification rawNotification)
        {
            var mdr = new MDRDetails
            {
                MDRTreatmentStartDate = rawNotification.MDRTreatmentStartDate,
                ExpectedTreatmentDurationInMonths = rawNotification.MDRExpectedDuration,
                ExposureToKnownCaseStatus = Converter.GetStatusFromString(rawNotification.mdr_ExposureToKnownTbCase),
                RelationshipToCase = rawNotification.mdr_RelationshipToCase,
                // Notification.mdr_CaseInUKStatus is not used, as in NTBS it's calculated on the fly
                CountryId = rawNotification.mdr_CountryId
            };
            if (!string.IsNullOrEmpty(rawNotification.mdr_RelatedNotificationId))
            {
                mdr.NotifiedToPheStatus = Status.Yes;
                mdr.RelatedNotificationId = int.Parse(rawNotification.mdr_RelatedNotificationId);
            }

            return mdr;
        }

        private void SetDenotificationReasonAndDescription(MigrationDbNotification notification, DenotificationDetails denotificationDetails)
        {
            var reasons = (DenotificationReason[])Enum.GetValues(typeof(DenotificationReason));
            denotificationDetails.Reason = reasons.Any(r => r.GetDisplayName().ToLower() == notification.DenotificationReason?.ToLower())
                ? reasons.Single(r => r.GetDisplayName().ToLower() == notification.DenotificationReason.ToLower())
                : DenotificationReason.Other;
            if (denotificationDetails.Reason == DenotificationReason.Other)
            {
                denotificationDetails.OtherDescription = "Denotified in legacy system, with denotification date " +
                                                         (notification.DenotificationDate?.ToString() ?? "missing");
            }
        }

        private static int? GetCountryTotalOrNull(Status? status, int? countryCount, int countryCount2)
        {
            return status == Status.Yes && countryCount != null && Math.Max(countryCount.Value, countryCount2) != 0
                ? Math.Max(countryCount.Value, countryCount2)
                : (int?)null;
        }
    }
}
