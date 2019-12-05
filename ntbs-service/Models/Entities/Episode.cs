using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using ExpressiveAnnotations.Attributes;
using Microsoft.EntityFrameworkCore;
using ntbs_service.Models.Validations;

namespace ntbs_service.Models.Entities
{
    [Owned]
    public class Episode : ModelBase
    {
        [MaxLength(200)]
        [RegularExpression(ValidationRegexes.CharacterValidation, ErrorMessage = ValidationMessages.StandardStringFormat)]
        [Display(Name = "Consultant")]
        public string Consultant { get; set; }

        [AssertThat("CaseManagerAllowedForTbService", ErrorMessage = ValidationMessages.CaseManagerMustBeAllowedForSelectedTbService)]
        [DisplayName("Case Manager")]
        public string CaseManagerEmail { get; set; }
        public virtual CaseManager CaseManager { get; set; }

        [RequiredIf(@"ShouldValidateFull", ErrorMessage = ValidationMessages.FieldRequired)]
        [DisplayName("TB Service")]
        public string TBServiceCode { get; set; }
        public virtual TBService TBService { get; set; }

        [RequiredIf(@"ShouldValidateFull", ErrorMessage = ValidationMessages.FieldRequired)]
        [AssertThat(@"HospitalBelongsToTbService", ErrorMessage = ValidationMessages.HospitalMustBelongToSelectedTbSerice)]
        [DisplayName("Hospital")]
        public Guid? HospitalId { get; set; }
        public virtual Hospital Hospital { get; set; }

        [NotMapped]
        public bool HospitalBelongsToTbService => Hospital?.TBService == TBService;

        [NotMapped]
        public bool CaseManagerAllowedForTbService
        {
            get
            {
                // If email not set, or TBService missing (ergo navigation properties not yet retrieved) pass validation
                if (string.IsNullOrEmpty(CaseManagerEmail) || TBService == null)
                {
                    return true;
                }

                if (CaseManager?.CaseManagerTbServices == null || CaseManager.CaseManagerTbServices.Count == 0)
                {
                    return false;
                }

                return CaseManager.CaseManagerTbServices.Any(c => c.TbService == TBService);
            }
        }


        [NotMapped]
        [DisplayName("Case Manager")]
        public string CaseManagerName => CaseManager?.FullName;
    }
}
