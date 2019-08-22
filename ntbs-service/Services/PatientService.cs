using System.Threading.Tasks;
using ntbs_service.DataAccess;
using ntbs_service.Models;

namespace ntbs_service.Services
{
    public interface IPatientService
    {
        void UpdateUkBorn(Patient patient);
    }

    public class PatientService : IPatientService
    {
        public void UpdateUkBorn(Patient patient)
        {
            if (patient.Country == null) {
                patient.UkBorn = null;
                return;
            }

            switch (patient.Country.IsoCode)
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