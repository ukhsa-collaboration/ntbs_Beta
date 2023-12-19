using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using ntbs_service.Models.Entities.Alerts;
using ntbs_service.Models.Validations;

namespace ntbs_service.Models.ViewModels
{
    public class TransferRequestActionViewModel : TransferViewModel
    {
        [BindProperty]
        [Required(ErrorMessage = "Please accept or decline the transfer")]
        public bool? AcceptTransfer { get; set; }
        
        [BindProperty]
        [MaxLength(200)]
        [RegularExpression(ValidationRegexes.CharacterValidationAsciiSpaceTozAndEmDash,
            ErrorMessage = ValidationMessages.InvalidCharacter)]
        [Display(Name = "Explanatory comment")]
        public string DeclineTransferReason { get; set; }

        [BindProperty]
        public int AlertId { get; set; }
        
        [ValidateNever]
        public TransferAlert TransferAlert { get; set; }
        public SelectList Hospitals { get; set; }
        public SelectList CaseManagers { get; set; }
        
        [BindProperty]
        public Guid TargetHospitalId { get; set; }
        
        [BindProperty]
        public int? TargetCaseManagerId { get; set; }
    }
}
