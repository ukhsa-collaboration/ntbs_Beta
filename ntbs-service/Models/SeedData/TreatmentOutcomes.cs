using System.Collections.Generic;
using ntbs_service.Models.Enums;
using ntbs_service.Models.ReferenceEntities;

namespace ntbs_service.Models.SeedData
{
    public class TreatmentOutcomes
    {
        public const int DiedAndUnknownOutcomeId = 10;

        public static IEnumerable<TreatmentOutcome> GetTreatmentOutcomes()
        {
            return new List<TreatmentOutcome>
            {
                new TreatmentOutcome { TreatmentOutcomeId = 1, TreatmentOutcomeType = TreatmentOutcomeType.Completed, TreatmentOutcomeSubType = TreatmentOutcomeSubType.StandardTherapy },
                new TreatmentOutcome { TreatmentOutcomeId = 2, TreatmentOutcomeType = TreatmentOutcomeType.Completed, TreatmentOutcomeSubType = TreatmentOutcomeSubType.MdrRegimen },
                new TreatmentOutcome { TreatmentOutcomeId = 3, TreatmentOutcomeType = TreatmentOutcomeType.Completed, TreatmentOutcomeSubType = TreatmentOutcomeSubType.Other },

                new TreatmentOutcome { TreatmentOutcomeId = 4, TreatmentOutcomeType = TreatmentOutcomeType.Cured, TreatmentOutcomeSubType = TreatmentOutcomeSubType.StandardTherapy },
                new TreatmentOutcome { TreatmentOutcomeId = 5, TreatmentOutcomeType = TreatmentOutcomeType.Cured, TreatmentOutcomeSubType = TreatmentOutcomeSubType.MdrRegimen },
                new TreatmentOutcome { TreatmentOutcomeId = 6, TreatmentOutcomeType = TreatmentOutcomeType.Cured, TreatmentOutcomeSubType = TreatmentOutcomeSubType.Other },

                new TreatmentOutcome { TreatmentOutcomeId = 7, TreatmentOutcomeType = TreatmentOutcomeType.Died, TreatmentOutcomeSubType = TreatmentOutcomeSubType.TbCausedDeath },
                new TreatmentOutcome { TreatmentOutcomeId = 8, TreatmentOutcomeType = TreatmentOutcomeType.Died, TreatmentOutcomeSubType = TreatmentOutcomeSubType.TbContributedToDeath },
                new TreatmentOutcome { TreatmentOutcomeId = 9, TreatmentOutcomeType = TreatmentOutcomeType.Died, TreatmentOutcomeSubType = TreatmentOutcomeSubType.TbIncidentalToDeath },
                new TreatmentOutcome { TreatmentOutcomeId = DiedAndUnknownOutcomeId, TreatmentOutcomeType = TreatmentOutcomeType.Died, TreatmentOutcomeSubType = TreatmentOutcomeSubType.Unknown },

                new TreatmentOutcome { TreatmentOutcomeId = 11, TreatmentOutcomeType = TreatmentOutcomeType.Lost, TreatmentOutcomeSubType = TreatmentOutcomeSubType.PatientLeftUk },
                new TreatmentOutcome { TreatmentOutcomeId = 12, TreatmentOutcomeType = TreatmentOutcomeType.Lost, TreatmentOutcomeSubType = TreatmentOutcomeSubType.PatientNotLeftUk },
                new TreatmentOutcome { TreatmentOutcomeId = 13, TreatmentOutcomeType = TreatmentOutcomeType.Lost, TreatmentOutcomeSubType = TreatmentOutcomeSubType.Other },

                new TreatmentOutcome { TreatmentOutcomeId = 14, TreatmentOutcomeType = TreatmentOutcomeType.TreatmentStopped, TreatmentOutcomeSubType = null },

                new TreatmentOutcome { TreatmentOutcomeId = 15, TreatmentOutcomeType = TreatmentOutcomeType.NotEvaluated, TreatmentOutcomeSubType = TreatmentOutcomeSubType.TransferredAbroad },
                new TreatmentOutcome { TreatmentOutcomeId = 16, TreatmentOutcomeType = TreatmentOutcomeType.NotEvaluated, TreatmentOutcomeSubType = TreatmentOutcomeSubType.StillOnTreatment },
                new TreatmentOutcome { TreatmentOutcomeId = 17, TreatmentOutcomeType = TreatmentOutcomeType.NotEvaluated, TreatmentOutcomeSubType = TreatmentOutcomeSubType.Other },
                
                new TreatmentOutcome { TreatmentOutcomeId = 18, TreatmentOutcomeType = TreatmentOutcomeType.Failed, TreatmentOutcomeSubType = TreatmentOutcomeSubType.CulturePositive },
                new TreatmentOutcome { TreatmentOutcomeId = 19, TreatmentOutcomeType = TreatmentOutcomeType.Failed, TreatmentOutcomeSubType = TreatmentOutcomeSubType.AdditionalResistance },
                new TreatmentOutcome { TreatmentOutcomeId = 20, TreatmentOutcomeType = TreatmentOutcomeType.Failed, TreatmentOutcomeSubType = TreatmentOutcomeSubType.AdverseReaction },
                new TreatmentOutcome { TreatmentOutcomeId = 21, TreatmentOutcomeType = TreatmentOutcomeType.Failed, TreatmentOutcomeSubType = TreatmentOutcomeSubType.Other }
            };
        }
    }
}
