using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ntbs_service.Models;
using ntbs_service.Models.Enums;

namespace ntbs_service.DataAccess
{
    public interface IAlertRepository
    {
    }

    public class AlertRepository : IAlertRepository
    {
        private readonly NtbsContext _context;

        public AlertRepository(NtbsContext context)
        {
            this._context = context;
        }
    }
}