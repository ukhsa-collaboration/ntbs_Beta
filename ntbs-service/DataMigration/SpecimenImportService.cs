using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire.Server;
using ntbs_service.Models.Entities;
using ntbs_service.Services;

namespace ntbs_service.DataMigration
{
    public interface ISpecimenImportService
    {
        Task ImportReferenceLabResultsAsync(PerformContext context, int runId,
            IList<Notification> notifications, ImportResult importResult);
    }

    public class SpecimenImportService : ISpecimenImportService
    {
        private readonly IImportLogger _logger;
        private readonly ISpecimenService _specimenService;

        public SpecimenImportService(IImportLogger logger,ISpecimenService specimenService)
        {
            _logger = logger;
            _specimenService = specimenService;
        }

        /// <summary>
        /// We have to run the reference lab result matches after the notifications have been imported into the main db,
        /// since the matches are stored externally - we need to know what the generated NTBS ids are beforehand.
        /// </summary>
        public async Task ImportReferenceLabResultsAsync(PerformContext context,
            int runId,
            IList<Notification> notifications,
            ImportResult importResult)
        {
            var legacyIds = notifications.Select(n => n.ETSID);
            var matches = await _specimenService.GetLegacyReferenceLaboratoryMatches(legacyIds);
            foreach (var (legacyId, referenceLaboratoryNumber) in matches)
            {
                var notificationId = notifications.Single(n => n.ETSID == legacyId).NotificationId;
                var success = await _specimenService.MatchSpecimenAsync(notificationId,
                    referenceLaboratoryNumber,
                    AuditService.AuditUserSystem,
                    isMigrating: true);
                if (!success)
                {
                    var error = $"Failed to set the specimen match for reference lab number: {referenceLaboratoryNumber}. " +
                                $"The notification is already imported, manual intervention needed!";
                    await _logger.LogNotificationError(context, runId, legacyId, error);
                    importResult.AddNotificationError(legacyId, error);
                }
            }
        }
    }
}
