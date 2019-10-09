using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ntbs_service.Services;
using ntbs_service.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using ntbs_service.Models.Validations;
using ntbs_service.Pages;
using ntbs_service.Models.Enums;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.WebUtilities;

namespace ntbs_service.Pages_Search
{
    public class IndexModel : ValidationModel
    {
        private readonly NtbsContext context;
        public INotificationService service;
        public string CurrentFilter { get; set; }
        public PaginatedList<NotificationBannerModel> SearchResults;
        public string NextPageUrl;
        public string NextPageText;
        public string PreviousPageUrl;
        public string PreviousPageText;
        public List<Sex> Sexes { get; set; }

        [BindProperty(SupportsGet = true)]
        public SearchParameters SearchParameters { get; set; }
        [BindProperty(SupportsGet = true)]
        public PartialDate PartialDob { get; set; }
        public bool? SearchParamsExist { get; set; }

        public IndexModel(INotificationService service, NtbsContext context)
        {
            this.context = context;
            this.service = service;
        }

        public async Task<IActionResult> OnGetAsync(int? pageIndex)
        {
            Sexes = context.GetAllSexesAsync().Result.ToList();
            if(!ModelState.IsValid) 
            {
                return Page();
            }

            // TODO any better way to fix this? default check any without setting sexId
            if(SearchParameters.SexId == null) {
                SearchParameters.SexId = 0;
            }

            var pageSize = 50;

            var draftStatusList = new List<NotificationStatus>() {NotificationStatus.Draft};
            var nonDraftStatusList = new List<NotificationStatus>() {NotificationStatus.Notified, NotificationStatus.Denotified};
            IQueryable<Notification> draftsIQ = service.GetBaseQueryableNotificationByStatus(draftStatusList);
            IQueryable<Notification> nonDraftsIQ = service.GetBaseQueryableNotificationByStatus(nonDraftStatusList);

            if (!String.IsNullOrEmpty(SearchParameters.IdFilter))
            {
                SearchParamsExist = true;
                draftsIQ = service.FilterById(draftsIQ, SearchParameters.IdFilter);
                nonDraftsIQ = service.FilterById(nonDraftsIQ, SearchParameters.IdFilter);
            }

            if (SearchParameters.SexId != null && SearchParameters.SexId != 0)
            {
                SearchParamsExist = true;
                draftsIQ = service.FilterBySex(draftsIQ, (int) SearchParameters.SexId);
                nonDraftsIQ = service.FilterBySex(nonDraftsIQ, (int) SearchParameters.SexId);
            }

            if (PartialDob != null)
            {
                SearchParamsExist = true;
                draftsIQ = service.FilterByPartialDate(draftsIQ, PartialDob);
                nonDraftsIQ = service.FilterByPartialDate(nonDraftsIQ, PartialDob);
            }

            IQueryable<Notification> notificationsIQ = service.OrderQueryableByNotificationDate(draftsIQ).Union(service.OrderQueryableByNotificationDate(nonDraftsIQ));

            var notifications = await PaginatedList<Notification>.CreateAsync(
                notificationsIQ.AsNoTracking(), pageIndex ?? 1, pageSize);

            SearchResults = notifications.SelectItems(NotificationBannerModel.WithLink);

            SetPaginationDetails();

            return Page();
        }

        public ContentResult OnGetValidateSearchProperty(string key, string value)
        {
            return ValidateProperty(new IndexModel(service, context), key, value);
        }

        public void SetPaginationDetails() {
            var queryString = Request.Query;
            var previousPageQueryString = new Dictionary<string, string>();
            foreach(var key in queryString.Keys) {
                previousPageQueryString[key] = queryString[key].ToString();
            }
            var nextPageQueryString = previousPageQueryString;
            if(SearchResults?.HasPreviousPage ?? false) 
            {
                PreviousPageText = "Page " + (SearchResults.PageIndex - 1) + " of " + (SearchResults.TotalPages);
                previousPageQueryString["pageIndex"] = "" + (SearchResults.PageIndex - 1);
                PreviousPageUrl = QueryHelpers.AddQueryString("/Search", previousPageQueryString);
            }
            if(SearchResults?.HasNextPage ?? false)
            {
                NextPageText = "Page " + (SearchResults.PageIndex + 1) + " of " + (SearchResults.TotalPages);
                nextPageQueryString["pageIndex"] = "" + (SearchResults.PageIndex + 1);
                NextPageUrl = QueryHelpers.AddQueryString("/Search", nextPageQueryString);
            }
        }
    }
}
