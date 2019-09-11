using System;
using System.ComponentModel.DataAnnotations;
using ntbs_service.Models.Enums;

namespace ntbs_service.Models.Validations
{
    public class ValidRiskFactorAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var riskFactor = (RiskFactorBase) value;

            var isAnyChosen = riskFactor.InPastFiveYears || riskFactor.IsCurrent || riskFactor.MoreThanFiveYearsAgo;
            if (riskFactor.Status == Status.Yes && !isAnyChosen) {
                return new ValidationResult(GetErrorMessage());
            } else {
                return ValidationResult.Success;
            }
        }

        private string GetErrorMessage()
        {
            return "At least one field should be selected";
        }
    }
}