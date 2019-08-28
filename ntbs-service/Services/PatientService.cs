using System.Threading.Tasks;
using ntbs_service.DataAccess;
using ntbs_service.Models;

namespace ntbs_service.Services
{
    public interface IPatientService
    {
        Task<Patient> GetPatientAsync(int? id);
        Task UpdatePatientAsync(Patient patient);
    }

    public class PatientService : IPatientService
    {
        private IPatientRepository repository;
        private NtbsContext context;
        public PatientService(IPatientRepository repository, NtbsContext context) {
            this.repository = repository;
            this.context = context;
        }

        public async Task<Patient> GetPatientAsync(int? id) {
            return await repository.GetPatientAsync(id);
        }

        public async Task UpdatePatientAsync(Patient patient)
        {
            await UpdatePatientFlags(patient);
            await repository.UpdatePatientAsync(patient);
        }

        private async Task UpdatePatientFlags(Patient patient)
        {
            await UpdateUkBorn(patient);

            if (patient.IsNhsNumberUnknown)
            {
                patient.NhsNumber = null;
            }

            if (patient.IsPostcodeUnknown)
            {
                patient.Postcode = null;
            }
        }

        private async Task UpdateUkBorn(Patient patient)
        {
            var country = await context.GetCountryByIdAsync(patient.CountryId);
            if (country == null) {
                patient.UkBorn = null;
                return;
            }

            switch (country.IsoCode)
            {
                case Countries.UkCode:
                    patient.UkBorn = true;
                    break;
                case Countries.UnknownCode:
                    patient.UkBorn = null;
                    break;
                default:
                    patient.UkBorn = false;    
                    break;    
            }
        }
    }
}