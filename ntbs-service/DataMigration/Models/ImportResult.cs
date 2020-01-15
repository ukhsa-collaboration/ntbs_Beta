using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ntbs_service.Models.Entities;

namespace ntbs_service.DataMigration
{
    public class ImportResult
    {
        public string PatientName { get; set; }
        public Dictionary<string, List<string>> ValidationErrors { get; set; } = new Dictionary<string, List<string>>();
        public bool IsValid => ValidationErrors.Values.All(x => x == null);

        public ImportResult(string patientName)
        {
            PatientName = patientName;
        }

        public void AddValidationErrorsMessages(string LegacyId, List<ValidationResult> validationResults)
        {
            if (!ValidationErrors.ContainsKey(LegacyId))
            {
                ValidationErrors.Add(LegacyId, validationResults.Select(x => x.ErrorMessage).ToList());
            }
        }

        public void AddGroupError(string error)
        {
            ValidationErrors.Add("groupFailedToImport", new List<string> {error});
        }

        public void AddValidNotification(string legacyId)
        {
            ValidationErrors.Add(legacyId, null);
        }
    }
}
