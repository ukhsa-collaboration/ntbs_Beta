using System;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;

namespace ntbs_service.Helpers
{
    public static class DrugResistanceHelper
    {
        public static bool IsMdr(DrugResistanceProfile profile, TreatmentRegimen? treatmentRegimen, Status? exposureToKnownCaseStatus)
        {
            // If user-set treatment ...
            return treatmentRegimen == TreatmentRegimen.MdrTreatment
                // ... or lab results indicate MDR, ...
                || profile.DrugResistanceProfileString == "RR/MDR/XDR"
                // ... or if there is any data entered in the MDR pages - otherwise we could be hiding record data
                || exposureToKnownCaseStatus != null;
        }

        public static bool MdrDetailsEntered(Status? exposureToKnownCaseStatus) => exposureToKnownCaseStatus.HasValue;

        public static bool IsMbovis(DrugResistanceProfile profile, MBovisDetails mBovisDetails)
        {
            // If the lab results point to M. bovis species, or if some of the M. bovis questionnaire has already been filled in
            // This might occur with non-M. bovis lab results if the questionnaire was done in a legacy system and migrated in.
            return string.Equals("M. bovis", profile.Species, StringComparison.InvariantCultureIgnoreCase)
                   || mBovisDetails.DataEntered;
        }

        public static bool IsMBovisQuestionnaireComplete(MBovisDetails mBovisDetails)
        {
            return mBovisDetails.ExposureToKnownCasesStatus.HasValue
                   && mBovisDetails.UnpasteurisedMilkConsumptionStatus.HasValue
                   && mBovisDetails.OccupationExposureStatus.HasValue
                   && mBovisDetails.AnimalExposureStatus.HasValue;
        }
    }
}
