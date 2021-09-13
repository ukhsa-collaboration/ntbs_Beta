using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Castle.Core.Internal;
using Hangfire.Server;
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
        Task<TreatmentEvent> AsTransferEvent(MigrationDbTransferEvent rawEvent, PerformContext context, int runId);
        Task<TreatmentEvent> AsOutcomeEvent(MigrationDbOutcomeEvent rawEvent, PerformContext context, int runId);
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
        
        public async Task<TreatmentEvent> AsTransferEvent(MigrationDbTransferEvent rawEvent, PerformContext context, int runId)
        {
            var ev = new TreatmentEvent
            {
                EventDate = rawEvent.EventDate,
                TreatmentEventType = Converter.GetEnumValue<TreatmentEventType>(rawEvent.TreatmentEventType),
                Note = RemoveUnnecessaryNoteInfo(rawEvent.Notes)
            };

            await TryAddTbServiceAndCaseManagerToTreatmentEvent(ev, rawEvent.HospitalId, rawEvent.CaseManager, context, runId);

            return ev;
        }

        public async Task<TreatmentEvent> AsOutcomeEvent(MigrationDbOutcomeEvent rawEvent, PerformContext context, int runId)
        {
            var ev = new TreatmentEvent
            {
                EventDate = rawEvent.EventDate,
                TreatmentEventType = Converter.GetEnumValue<TreatmentEventType>(rawEvent.TreatmentEventType),
                TreatmentOutcomeId = rawEvent.TreatmentOutcomeId,
                Note = rawEvent.Note
            };

            await TryAddTbServiceAndCaseManagerToTreatmentEvent(ev, rawEvent.NtbsHospitalId, rawEvent.CaseManager, context, runId);

            return ev;
        }

        private async Task TryAddTbServiceAndCaseManagerToTreatmentEvent(TreatmentEvent ev, Guid? hospitalId, string caseManagerUsername, PerformContext context, int runId)
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
                await _caseManagerImportService.ImportOrUpdateLegacyUser(caseManagerUsername, ev.TbServiceCode, context, runId);
                ev.CaseManagerId = (await _referenceDataRepository.GetUserByUsernameAsync(caseManagerUsername)).Id;
            }
        }

        private string RemoveUnnecessaryNoteInfo(string note)
        {
            if (note.IsNullOrEmpty())
            {
                return null;
            }
            var trimmedNote = note.Trim();
            var mainPattern =
                @"(Dear|Hi)[0-9a-zA-Z -]+,{0,1}[\n\r ]*(?<caseManagerText>[0-9a-zA-Z \/\-—,.'`@#&+;:$_()<>\\\[\]=\*\?\n\r]*)"
                + @"(?<uselessInfo>Id: [0-9 ]+[\n\r ]*Patient: [a-zA-Z -]+[\n\r ]*Case report date: [0-9 ]{2}\/[0-9]{2}\/[0-9]{4})"
                + @"(?<appendedNote>[0-9a-zA-Z \/\-—,.'`@#&+;:$_()<>\\\[\]=\*\?\n\r]*)";
            var caseManagerPattern = @"You have been identified as the new case manager for the case below\.[\n\r]*";
            var notePatternMatch = Regex.Match(trimmedNote, mainPattern);
            if (notePatternMatch.Success)
            {
                var caseManagerText = notePatternMatch.Groups["caseManagerText"].Value;
                var appendedNote = notePatternMatch.Groups["appendedNote"].Value;
                var returnNote = Regex.IsMatch(caseManagerText, caseManagerPattern) ? appendedNote.Trim() : $"{caseManagerText.Trim()} {appendedNote.Trim()}";
                return returnNote.Length == 0 ? null : returnNote;
            }
            else
            {
                return trimmedNote;
            }
        }
    }
}
