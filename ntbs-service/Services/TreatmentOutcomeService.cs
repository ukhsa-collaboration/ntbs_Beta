using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using ntbs_service.DataAccess;
using ntbs_service.Models;
using ntbs_service.Models.Entities;

namespace ntbs_service.Services
{
    public interface ITreatmentOutcomeService
    {
    }

    public class TreatmentOutcomeService : ITreatmentOutcomeService
    {
        private readonly IAlertRepository _alertRepository;

        public TreatmentOutcomeService(
            IAlertRepository alertRepository)
        {
            _alertRepository = alertRepository;
        }

        public async Task<bool> IsTreatmentOutcomeNeededAsync(IList<TreatmentEvent> treatmentEvents)
        {
            return false;
        }
    }
}
