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
            switch (patient.CountryId) 
            {
                case (int)CountryCode.UK:
                    patient.UkBorn = true;
                    break;
                case null:
                case (int)CountryCode.UNKNOWN:
                    patient.UkBorn = null;
                    break;
                default:
                    patient.UkBorn = false;    
                    break;    
            }
        }
    }
}