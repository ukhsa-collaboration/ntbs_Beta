using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ntbs_service.DataAccess;
using ntbs_service.Helpers;
using ntbs_service.Models;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Entities.Alerts;
using ntbs_service.Models.Enums;
using Serilog;

namespace ntbs_service.Services
{
    public interface IVersionService
    {
        ReleaseVersion GetReleaseVersion();

    }

    public class VersionService : IVersionService
    {
        private readonly NtbsContext _context;


        public VersionService(
            NtbsContext context)
        {
            _context = context;
        }

        public ReleaseVersion GetReleaseVersion()
        {
            return this._context.ReleaseVersion.SingleOrDefault() ?? new ReleaseVersion{Version = "unknown"};
        }
    }
}
