using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ntbs_service.Models;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;
using ntbs_service.Models.ReferenceEntities;

namespace ntbs_service.DataAccess
{
    public interface IReferenceDataRepository
    {
        Task<IList<Country>> GetAllCountriesAsync();
        Task<IList<Country>> GetAllHighTbIncidenceCountriesAsync();
        Task<IList<Country>> GetAllCountriesApartFromUkAsync();
        Task<Country> GetCountryByIdAsync(int id);
        Task<IList<TBService>> GetAllTbServicesAsync();
        Task<IList<PHEC>> GetAllPhecs();
        Task<TBService> GetTbServiceByCodeAsync(string code);
        Task<IList<User>> GetAllCaseManagers();
        Task<User> GetCaseManagerByUsernameAsync(string username);
        Task<IList<Hospital>> GetHospitalsByTbServiceCodesAsync(IEnumerable<string> tbServices);
        Task<IList<User>> GetCaseManagersByTbServiceCodesAsync(IEnumerable<string> tbServiceCodes);
        Task<IList<TBService>> GetTbServicesFromPhecCodeAsync(string phecCode);
        Task<TBService> GetTbServiceFromHospitalIdAsync(Guid hospitalId);
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
        Task<IList<TreatmentOutcome>> GetTreatmentOutcomesForType(TreatmentOutcomeType type);
        Task<TreatmentOutcome> GetTreatmentOutcomeForTypeAndSubType(TreatmentOutcomeType type, TreatmentOutcomeSubType? subType);
        Task<string> GetLocationPhecCodeForPostcodeAsync(string postcode);
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
            return await GetBaseCountryQueryable().ToListAsync();
        }

        public async Task<IList<Country>> GetAllHighTbIncidenceCountriesAsync()
        {
            return await GetBaseCountryQueryable()
                .Where(c => c.HasHighTbOccurence)
                .ToListAsync();
        }

        public async Task<IList<Country>> GetAllCountriesApartFromUkAsync()
        {
            return await GetBaseCountryQueryable()
                .Where(c => c.IsoCode != Countries.UkCode).ToListAsync();
        }

        private IQueryable<Country> GetBaseCountryQueryable()
        {
            return _context.Country
                .Where(c => c.LegacyCountry == false)
                .OrderBy(c => c.Name);
        }

        public async Task<Country> GetCountryByIdAsync(int countryId)
        {
            return await _context.Country.FindAsync(countryId);
        }

        public async Task<IList<TBService>> GetAllTbServicesAsync()
        {
            return await GetTbServicesQueryable().ToListAsync();
        }

        public async Task<IList<PHEC>> GetAllPhecs()
        {
            return await _context.PHEC.ToListAsync();
        }

        public async Task<TBService> GetTbServiceByCodeAsync(string code)
        {
            return await GetTbServicesQueryable().SingleOrDefaultAsync(t => t.Code == code);
        }

        public async Task<IList<TBService>> GetTbServicesFromPhecCodeAsync(string phecCode)
        {
            return await GetTbServicesQueryable()
                .Where(s => s.PHECCode == phecCode)
                .ToListAsync();
        }

        public async Task<TBService> GetTbServiceFromHospitalIdAsync(Guid hospitalId)
        {
            return await _context.Hospital
                .Where(h => h.HospitalId == hospitalId)
                .Select(h => h.TBService)
                .SingleOrDefaultAsync();
        }
        
        public async Task<IList<User>> GetAllCaseManagers()
        {
            return await _context.User
                .Where(u => u.IsCaseManager)
                .ToListAsync();
        }

        public async Task<User> GetCaseManagerByUsernameAsync(string username)
        {
            return await _context.User
                .Include(c => c.CaseManagerTbServices)
                .ThenInclude(ct => ct.TbService)
                .Where(u => u.IsCaseManager)
                .SingleOrDefaultAsync(c => c.Username == username);
        }

        public async Task<IList<User>> GetCaseManagersByTbServiceCodesAsync(IEnumerable<string> tbServiceCodes)
        {
            return await GetTbServicesQueryable()
                .Where(t => tbServiceCodes.Contains(t.Code))
                .SelectMany(t => t.CaseManagerTbServices.Select(join => join.CaseManager))
                .Where(user => user.IsCaseManager)
                .Distinct()
                .OrderBy(c => c.FamilyName)
                .ToListAsync();
        }

        public async Task<IList<Hospital>> GetHospitalsByTbServiceCodesAsync(IEnumerable<string> tbServices)
        {
            return await _context.Hospital
                .Where(h => tbServices.Contains(h.TBServiceCode))
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
            return await GetTbServicesQueryable().Where(tb => roles.Contains(tb.ServiceAdGroup)).Select(tb => tb.Code).ToListAsync();
        }

        public async Task<List<string>> GetPhecCodesMatchingRolesAsync(IEnumerable<string> roles)
        {
            return await _context.PHEC.Where(ph => roles.Contains(ph.AdGroup)).Select(ph => ph.Code).ToListAsync();
        }

        public IQueryable<TBService> GetTbServicesQueryable()
        {
            return _context.TbService.OrderBy(s => s.Name);
        }

        public IQueryable<TBService> GetDefaultTbServicesForNhsUserQueryable(IEnumerable<string> roles)
        {
            return GetTbServicesQueryable()
                .Where(tb => roles.Contains(tb.ServiceAdGroup));
        }

        public IQueryable<TBService> GetDefaultTbServicesForPheUserQueryable(IEnumerable<string> roles)
        {
            return GetTbServicesQueryable()
                .Include(tb => tb.PHEC)
                .Where(tb => roles.Contains(tb.PHEC.AdGroup));
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

        public async Task<IList<TreatmentOutcome>> GetTreatmentOutcomesForType(TreatmentOutcomeType type)
        {
            return await _context.TreatmentOutcome
                .Where(t => t.TreatmentOutcomeType == type)
                .ToListAsync();
        }

        public async Task<IList<VenueType>> GetAllVenueTypesAsync()
        {
            return await _context.VenueType.ToListAsync();
        }

        public async Task<TreatmentOutcome> GetTreatmentOutcomeForTypeAndSubType(
            TreatmentOutcomeType type,
            TreatmentOutcomeSubType? subType)
        {
            return await _context.TreatmentOutcome.SingleOrDefaultAsync(t => 
                t.TreatmentOutcomeType == type
                && t.TreatmentOutcomeSubType == subType);
        }

        public async Task<string> GetLocationPhecCodeForPostcodeAsync(string postcode)
        {
            var Postcode = await _context.PostcodeLookup
                .Where(p => p.Postcode == postcode.Replace(" ", ""))
                .Include(p => p.LocalAuthority)
                    .ThenInclude(la => la.LocalAuthorityToPHEC)
                        .ThenInclude(pl => pl.PHEC)
                .SingleOrDefaultAsync();
            return Postcode?.LocalAuthority?.LocalAuthorityToPHEC?.PHECCode;
        }
    }
}
