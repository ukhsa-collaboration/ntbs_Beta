using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ntbs_service.Models.Validations;
using ntbs_service.Services;

namespace ntbs_service.Pages.ServiceDirectory
{
    public class ServiceDirectorySearchBase : PageModel
    {
        private readonly ValidationService _validationService;
        
        [BindProperty(SupportsGet = true)]
        [Display(Name = "Search keyword")]
        [RegularExpression(ValidationRegexes.CharacterValidation, 
            ErrorMessage = ValidationMessages.StandardStringFormat)]
        public string SearchKeyword { get; set; }

        protected ServiceDirectorySearchBase()
        {
            _validationService = new ValidationService(this);
        }
        
        public ContentResult OnGetValidateProperty(string key, string value)
        {
            return _validationService.GetPropertyValidationResult(this, key, value);
        }

        public bool IsValid(string key)
        {
            return _validationService.IsValid(key);
        }
    }
}
