using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ntbs_service.Helpers;
using ntbs_service.Models;
using ntbs_service.Pages;
using ntbs_service.Services;

namespace ntbs_service.Pages_NotificationSites
{
    public class EditModel : ValidationModel
    {
        // Todo: Commonise all these validations within ValidationModel, to avoid creating razor pages for particular properties
        public ContentResult OnPostValidateNotificationSiteProperty(string key, string value)
        {
            return OnPostValidateProperty(new NotificationSite(), key, value);
        }
    }
}