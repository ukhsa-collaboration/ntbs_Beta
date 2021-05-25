using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ntbs_service.DataMigration
{
    public class ImportResult
    {
        public string PatientName { get; set; }
        public Dictionary<string, List<string>> ValidationErrors { get; set; } = new Dictionary<string, List<string>>();
        public bool IsValid => ValidationErrors.Values.All(errorList => !errorList.Any());
        public Dictionary<string, int> NtbsIds { get; set; } = new Dictionary<string, int>();

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
            ValidationErrors.Add("groupFailedToImport", new List<string> { error });
        }

        public void AddValidNotification(string legacyId)
        {
            ValidationErrors.Add(legacyId, new List<string>());
        }

        public void AddNotificationError(string legacyId, string error)
        {
            if (ValidationErrors.ContainsKey(legacyId))
            {
                ValidationErrors[legacyId].Add(error);
            }
            else
            {
                ValidationErrors.Add(legacyId, new List<string> { error });
            }
        }
    }
}
