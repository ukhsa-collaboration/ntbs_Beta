using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ntbs_service.Models;

namespace ntbs_service.DataAccess
{
    public interface IPatientRepository
    {
        Task<IList<Patient>> GetPatientsAsync();
        Task UpdatePatientAsync(Patient patient);
        Task AddPatientAsync(Patient patient);
        Task DeletePatientAsync(Patient patient);
        Task<Patient> GetPatientAsync(int? patientId);
        Task<Patient> FindPatientByIdAsync(int? patientId);
        bool PatientExists(int patientId);
    }
    
    public class PatientRepository : IPatientRepository 
    {
        private readonly NtbsContext _context;
        public PatientRepository(NtbsContext context) 
        {
            _context = context;
        }

        public async Task<IList<Patient>> GetPatientsAsync() 
        {
            return await _context.Patient
                .Include(p => p.Country)
                .Include(p => p.Ethnicity)
                .Include(p => p.Sex).ToListAsync();
        }

        public async Task UpdatePatientAsync(Patient patient) 
        {
            _context.Attach(patient).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task AddPatientAsync(Patient patient) 
        {
            _context.Patient.Add(patient);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePatientAsync(Patient patient)
        {
            _context.Patient.Remove(patient);
            await _context.SaveChangesAsync();
        }

        public async Task<Patient> GetPatientAsync(int? patientId)
        {
            return await _context.Patient
                .Include(p => p.Country)
                .Include(p => p.Ethnicity)
                .Include(p => p.Sex).FirstOrDefaultAsync(m => m.PatientId == patientId);
        }

        public async Task<Patient> FindPatientByIdAsync(int? patientId)
        {
            return await _context.Patient.FirstOrDefaultAsync(m => m.PatientId == patientId);
        }

        public bool PatientExists(int patientId)
        {
            return _context.Patient.Any(e => e.PatientId == patientId);
        }
    }
}