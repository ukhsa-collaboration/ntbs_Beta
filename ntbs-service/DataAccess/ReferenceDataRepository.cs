using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ntbs_service.Models;

namespace ntbs_service.DataAccess
{
    public interface IReferenceDataRepository
    {
        Task<IList<Country>> GetAllCountriesAsync();
        Task<IList<Country>> GetAllHighTbIncidenceCountriesAsync();
        Task<IList<Country>> GetAllCountriesApartFromUKAsync();
        Task<Country> GetCountryByIdAsync(int id);
        Task<IList<TBService>> GetAllTbServicesAsync();
        Task<TBService> GetTbServiceByCodeAsync(string code);
        Task<CaseManager> GetCaseManagerByEmailAsync(string email);
        Task<IList<Hospital>> GetHospitalsByTbServiceCodesAsync(IEnumerable<string> tbServices);
        Task<IList<CaseManager>> GetCaseManagersByTbServiceCodesAsync(IEnumerable<string> tbServiceCodes);
        Task<Hospital> GetHospitalByGuidAsync(Guid guid);
        Task<IList<Sex>> GetAllSexesAsync();
        Task<IList<Ethnicity>> GetAllOrderedEthnicitiesAsync();
        Task<IList<Site>> GetAllSitesAsync();
        Task<Occupation> GetOccupationByIdAsync(int id);
        Task<IList<Occupation>> GetAllOccupationsAsync();
        Task<List<string>> GetTbServiceCodesMatchingRolesAsync(IEnumerable<string> roles);
        Task<List<string>> GetPhecCodesMatchingRolesAsync(IEnumerable<string> roles);
        IQueryable<TBService> GetTbServicesQueryable();
        IQueryable<TBService> GetDefaultTbServicesForNhsUserQueryable(IEnumerable<string> roles);
        IQueryable<TBService> GetDefaultTbServicesForPheUserQueryable(IEnumerable<string> roles);
        Task<ManualTestType> GetManualTestTypeAsync(int value);
        Task<IList<ManualTestType>> GetManualTestTypesAsync();
        Task<SampleType> GetSampleTypeAsync(int value);
        Task<IList<SampleType>> GetSampleTypesAsync();
        Task<IList<SampleType>> GetSampleTypesForManualTestType(int manualTestTypeId);
        Task<IList<VenueType>> GetAllVenueTypesAsync();
    }

    public class ReferenceDataRepository : IReferenceDataRepository
    {
        private readonly NtbsContext _context;

        public ReferenceDataRepository(NtbsContext context)
        {
            this._context = context;
        }

        public async Task<IList<Country>> GetAllCountriesAsync()
        {
            return await _context.Country.ToListAsync();
        }

        public async Task<IList<Country>> GetAllHighTbIncidenceCountriesAsync()
        {
            return await _context.Country
                .Where(c => c.HasHighTbOccurence)
                .ToListAsync();
        }

        public async Task<IList<Country>> GetAllCountriesApartFromUKAsync()
        {
            return await _context.Country.Where(c => c.IsoCode != Countries.UkCode).ToListAsync();
        }

        public async Task<Country> GetCountryByIdAsync(int countryId)
        {
            return await _context.Country.FindAsync(countryId);
        }

        public async Task<IList<TBService>> GetAllTbServicesAsync()
        {
            return await _context.TbService.ToListAsync();
        }

        public async Task<TBService> GetTbServiceByCodeAsync(string code)
        {
            return await _context.TbService.SingleOrDefaultAsync(t => t.Code == code);
        }

        public async Task<CaseManager> GetCaseManagerByEmailAsync(string email)
        {
            return await _context.CaseManager
                .Include(c => c.CaseManagerTbServices)
                .ThenInclude(ct => ct.TbService)
                .SingleOrDefaultAsync(c => c.Email == email);
        }

        public async Task<IList<Hospital>> GetHospitalsByTbServiceCodesAsync(IEnumerable<string> tbServices)
        {
            return await _context.Hospital
                .Where(h => tbServices.Contains(h.TBServiceCode))
                .ToListAsync();
        }

        public async Task<IList<CaseManager>> GetCaseManagersByTbServiceCodesAsync(IEnumerable<string> tbServiceCodes)
        {
            return await _context.TbService
                .Where(t => tbServiceCodes.Contains(t.Code))
                .SelectMany(t => t.CaseManagerTbServices.Select(join => join.CaseManager))
                .Distinct()
                .OrderBy(c => c.FamilyName)
                .ToListAsync();
        }

        public async Task<Hospital> GetHospitalByGuidAsync(Guid guid)
        {
            return await _context.Hospital.SingleOrDefaultAsync(h => h.HospitalId == guid);
        }

        public async Task<IList<Sex>> GetAllSexesAsync()
        {
            return await _context.Sex.ToListAsync();
        }

        public async Task<IList<Ethnicity>> GetAllOrderedEthnicitiesAsync()
        {
            return await _context.Ethnicity.OrderBy(e => e.Order).ToListAsync();
        }

        public async Task<IList<Site>> GetAllSitesAsync()
        {
            return await _context.Site.ToListAsync();
        }

        public async Task<Occupation> GetOccupationByIdAsync(int id)
        {
            return await _context.Occupation.FindAsync(id);
        }

        public async Task<IList<Occupation>> GetAllOccupationsAsync()
        {
            return await _context.Occupation.ToListAsync();
        }

        public async Task<List<string>> GetTbServiceCodesMatchingRolesAsync(IEnumerable<string> roles)
        {
            return await _context.TbService.Where(tb => roles.Contains(tb.ServiceAdGroup)).Select(tb => tb.Code).ToListAsync();
        }

        public async Task<List<string>> GetPhecCodesMatchingRolesAsync(IEnumerable<string> roles)
        {
            return await _context.PHEC.Where(ph => roles.Contains(ph.AdGroup)).Select(ph => ph.Code).ToListAsync();
        }

        public IQueryable<TBService> GetTbServicesQueryable()
        {
            return _context.TbService;
        }

        public IQueryable<TBService> GetDefaultTbServicesForNhsUserQueryable(IEnumerable<string> roles)
        {
            return _context.TbService.Where(tb => roles.Contains(tb.ServiceAdGroup));
        }

        public IQueryable<TBService> GetDefaultTbServicesForPheUserQueryable(IEnumerable<string> roles)
        {
            return _context.TbService.Include(tb => tb.PHEC).Where(tb => roles.Contains(tb.PHEC.AdGroup));
        }

        public async Task<ManualTestType> GetManualTestTypeAsync(int id)
        {
            return await _context.ManualTestType
                .Include(t => t.ManualTestTypeSampleTypes)
                    .ThenInclude(join => join.SampleType)
                .FirstAsync(t => t.ManualTestTypeId == id);
        }

        public async Task<IList<ManualTestType>> GetManualTestTypesAsync()
        {
            return await _context.ManualTestType.ToListAsync();
        }

        public async Task<SampleType> GetSampleTypeAsync(int id)
        {
            return await _context.SampleType.FirstAsync(t => t.SampleTypeId == id);
        }

        public async Task<IList<SampleType>> GetSampleTypesAsync()
        {
            return await _context.SampleType.ToListAsync();
        }

        public async Task<IList<SampleType>> GetSampleTypesForManualTestType(int manualTestTypeId)
        {
            return await _context.SampleType
                .Where(s => s.ManualTestTypeSampleTypes.Any(join => join.ManualTestTypeId == manualTestTypeId))
                .ToListAsync();
        }

        public async Task<IList<VenueType>> GetAllVenueTypesAsync()
        {
            return await _context.VenueType.ToListAsync();
        }
    }
}
