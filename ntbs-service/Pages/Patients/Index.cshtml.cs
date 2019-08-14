using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ntbs_service.Models;
using ntbs_service.Data;

namespace ntbs_service.Pages_Patients
{
    public class IndexModel : PageModel
    {
        private readonly IPatientRepository _repository;

        public IndexModel(IPatientRepository repository)
        {
            _repository = repository;
        }

        public IList<Patient> Patients { get;set; }

        public async Task OnGetAsync()
        {
            Patients = await _repository.GetPatientsAsync();
        }
    }
}
