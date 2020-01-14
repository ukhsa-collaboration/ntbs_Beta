using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;
using ntbs_service.Services;

namespace ntbs_service.Pages.LabResults
{
    public class IndexModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly ISpecimenService _specimenService;

        public IEnumerable<UnmatchedSpecimen> UnmatchedSpecimens { get; private set; }

        public IndexModel(
            ISpecimenService specimenService,
            IUserService userService)
        {
            _specimenService = specimenService;
            _userService = userService;
        }

        public async Task OnGetAsync()
        {
            var permissionsFilter = await _userService.GetUserPermissionsFilterAsync(User);
            if (permissionsFilter.Type == UserType.NationalTeam)
            {
                UnmatchedSpecimens = await _specimenService.GetAllUnmatchedSpecimensAsync();
            }
            else if (permissionsFilter.FilterByTBService)
            {
                UnmatchedSpecimens = await _specimenService.GetUnmatchedSpecimensDetailsForTbServicesAsync(
                    permissionsFilter.IncludedTBServiceCodes);
            }
            else if (permissionsFilter.FilterByPHEC)
            {
                UnmatchedSpecimens = await _specimenService.GetUnmatchedSpecimensDetailsForPhecsAsync(
                    permissionsFilter.IncludedPHECCodes);
            }
        }
    }
}
