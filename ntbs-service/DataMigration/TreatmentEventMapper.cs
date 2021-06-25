using System;
using System.Linq;
using System.Threading.Tasks;
using ntbs_service.DataAccess;
using ntbs_service.DataMigration.RawModels;
using ntbs_service.Helpers;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;
using Serilog;

namespace ntbs_service.DataMigration
{
    public interface ITreatmentEventMapper
    {
        Task<TreatmentEvent> AsTransferEvent(MigrationDbTransferEvent rawEvent);
        Task<TreatmentEvent> AsOutcomeEvent(MigrationDbOutcomeEvent rawEvent);
    }
    public class TreatmentEventMapper : ITreatmentEventMapper
    {
        private readonly ICaseManagerImportService _caseManagerImportService;
        private readonly IReferenceDataRepository _referenceDataRepository;

        public TreatmentEventMapper(ICaseManagerImportService caseManagerImportService,
            IReferenceDataRepository referenceDataRepository)
        {
            _caseManagerImportService = caseManagerImportService;
            _referenceDataRepository = referenceDataRepository;
        }
        
        public async Task<TreatmentEvent> AsTransferEvent(MigrationDbTransferEvent rawEvent)
        {
            var ev = new TreatmentEvent
            {
                EventDate = rawEvent.EventDate,
                TreatmentEventType = Converter.GetEnumValue<TreatmentEventType>(rawEvent.TreatmentEventType)
            };

            await TryAddTbServiceAndCaseManagerToTreatmentEvent(ev, rawEvent.HospitalId, rawEvent.CaseManager);

            return ev;
        }

        public async Task<TreatmentEvent> AsOutcomeEvent(MigrationDbOutcomeEvent rawEvent)
        {
            var ev = new TreatmentEvent
            {
                EventDate = rawEvent.EventDate,
                TreatmentEventType = Converter.GetEnumValue<TreatmentEventType>(rawEvent.TreatmentEventType),
                TreatmentOutcomeId = rawEvent.TreatmentOutcomeId,
                Note = rawEvent.Note
            };

            await TryAddTbServiceAndCaseManagerToTreatmentEvent(ev, rawEvent.NtbsHospitalId, rawEvent.CaseManager);

            return ev;
        }

        private async Task TryAddTbServiceAndCaseManagerToTreatmentEvent(TreatmentEvent ev, Guid? hospitalId, string caseManagerUsername)
        {
            if (hospitalId is Guid guid)
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

            if (!string.IsNullOrEmpty(caseManagerUsername))
            {
                await _caseManagerImportService.ImportOrUpdateLegacyUser(caseManagerUsername, ev.TbServiceCode);
                ev.CaseManagerId = (await _referenceDataRepository.GetUserByUsernameAsync(caseManagerUsername)).Id;
            }
        }
    }
}
