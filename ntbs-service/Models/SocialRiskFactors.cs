using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ntbs_service.Models.Enums;
using ntbs_service.Models.Validations;

namespace ntbs_service.Models
{
    [Owned]
    public class SocialRiskFactors
    {
        public SocialRiskFactors() {
            RiskFactorHomelessness = new RiskFactorHomelessness();
            RiskFactorDrugs = new RiskFactorDrugs();
            RiskFactorMentalHealth = new RiskFactorMentalHealth();
            RiskFactorImprisonment = new RiskFactorImprisonment();
        }
        public Status? AlcoholMisuseStatus { get; set; }
        public Status? SmokingStatus { get; set; }
        public RiskFactorBase RiskFactorDrugs { get; set; }
        public RiskFactorBase RiskFactorHomelessness { get; set; }
        public RiskFactorBase RiskFactorMentalHealth { get; set; }
        public RiskFactorBase RiskFactorImprisonment { get; set; }
    }
}