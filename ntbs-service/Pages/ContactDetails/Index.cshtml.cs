using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Primitives;
using ntbs_service.DataAccess;
using ntbs_service.Helpers;
using ntbs_service.Models;
using ntbs_service.Models.Entities;
using ntbs_service.Models.ReferenceEntities;
using ntbs_service.Services;

namespace ntbs_service.Pages.ContactDetails
{
    public class IndexModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IReferenceDataRepository _referenceDataRepository;
        private readonly IUserHelper _userHelper;

        public IndexModel(IUserService userService, IReferenceDataRepository userRepository, IUserHelper userHelper)
        {
            _userService = userService;
            _referenceDataRepository = userRepository;
            _userHelper = userHelper;
        }

        public User ContactDetails { get; set; }
        
        public IList<PHEC> RegionalMemberships { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? UserId { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            ContactDetails = UserId.HasValue
                ? await _referenceDataRepository.GetUserByIdAsync(UserId.Value)
                : await _userService.GetUser(User);

            if (ContactDetails == null)
            {
                return NotFound();
            }

            RegionalMemberships = await this._referenceDataRepository.GetPhecsByAdGroups(ContactDetails.AdGroups);

            ViewData["IsEditable"] = _userHelper.CurrentUserMatchesUsernameOrIsAdmin(HttpContext, ContactDetails.Username)
                && !_userHelper.UserIsReadOnly(User);

            ContactDetails.CaseManagerTbServices = ContactDetails?.CaseManagerTbServices
                .OrderBy(x => x.TbService.PHEC.Name)
                .ThenBy(x => x.TbService.Name)
                .ToList();

            ViewData["Referer"] = StringValues.IsNullOrEmpty(Request.Headers["Referer"])
                ? "/"
                : Request.Headers["Referer"].ToString();

            PrepareBreadcrumbs();

            return Page();
        }

        private void PrepareBreadcrumbs()
        {
            var region = ContactDetails.CaseManagerTbServices.FirstOrDefault()?.TbService.PHEC;
            var breadcrumbs = new List<Breadcrumb>
            {
                new Breadcrumb {Label = "Service Directory", Url = "/ServiceDirectory"}
            };


            if (region != null)
            {
                breadcrumbs.Add(new Breadcrumb
                {
                    Label = region.Name,
                    Url = $"/ServiceDirectory/Region/{region.Code}"
                });
            }
            breadcrumbs.Add((new Breadcrumb { Label = ContactDetails.DisplayName, Url = $"/ContactDetails/{ContactDetails.Username}" }));

            ViewData["Breadcrumbs"] = breadcrumbs;
        }
    }
}
