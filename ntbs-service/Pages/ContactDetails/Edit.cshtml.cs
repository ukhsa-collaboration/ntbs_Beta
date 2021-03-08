using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ntbs_service.DataAccess;
using ntbs_service.Helpers;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Validations;
using ntbs_service.Services;

namespace ntbs_service.Pages.ContactDetails
{
    public class EditModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;
        private readonly ValidationService _validationService;

        public EditModel(IUserService userService, IUserRepository userRepository)
        {
            _userService = userService;
            _userRepository = userRepository;
            _validationService = new ValidationService(this);
        }

        [BindProperty]
        public User ContactDetails { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            ContactDetails = await _userService.GetUser(User);
            ContactDetails.CaseManagerTbServices = ContactDetails.CaseManagerTbServices
                .OrderBy(x => x.TbService.Name)
                .ThenBy(x => x.TbService.PHEC.Name)
                .ToList();

            return Page();
        }

        public bool IsValid(string key)
        {
            return _validationService.IsValid(key);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userService.GetUser(User);
            if (ContactDetails.Username != user.Username)
            {
                return StatusCode((int)HttpStatusCode.Forbidden);
            }

            ValidateModel();
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _userRepository.UpdateUserContactDetails(ContactDetails);
            return RedirectToPage("/ContactDetails/Index");
        }

        private void ValidateModel()
        {
            TryValidateModel(ContactDetails, nameof(ContactDetails));
            if (ContactDetails.ArePrimaryContactDetailsMissing)
            {
                ModelState.AddModelError("ContactDetails", ValidationMessages.SupplyCaseManagerPrimaryParameter);
            }

            if (!ModelState.IsValid)
            {
                ViewData["EditPageErrorDictionary"] = EditPageValidationErrorGenerator.MapToDictionary(ModelState);
            }
        }

        public ContentResult OnGetValidateCaseManagerProperty(string key, string value)
        {
            var user = new User();
            return _validationService.GetPropertyValidationResult(user, key, value);
        }
    }
}
