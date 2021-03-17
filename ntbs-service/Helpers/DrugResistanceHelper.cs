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

        public static bool IsMbovis(DrugResistanceProfile profile)
        {
            // If the lab results point to M. bovis species ...
            return string.Equals("M. bovis", profile.Species, StringComparison.InvariantCultureIgnoreCase);
        }

        public static bool IsMBovisQuestionnaireComplete(MBovisDetails mBovisDetails)
        {
            return mBovisDetails.HasExposureToKnownCases.HasValue
                   && mBovisDetails.HasUnpasteurisedMilkConsumption.HasValue
                   && mBovisDetails.HasOccupationExposure.HasValue
                   && mBovisDetails.HasAnimalExposure.HasValue;
        }
    }
}
