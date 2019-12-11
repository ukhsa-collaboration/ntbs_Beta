using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using ntbs_service.Models;
using ntbs_service.Models.Entities;
using ntbs_service.Services;

namespace ntbs_service.Helpers
{
    public static class DateTimeExtension
    {
        public static FormattedDate ConvertToFormattedDate(this DateTime? dateTime)
        {
            return dateTime == null ? new FormattedDate() : dateTime.Value.ConvertToFormattedDate();
        }

        public static FormattedDate ConvertToFormattedDate(this DateTime dateTime)
        {
            return new FormattedDate
            {
                Day = dateTime.Day.ToString(), 
                Month = dateTime.Month.ToString(), 
                Year = dateTime.Year.ToString()
            };
        }

        public static string ConvertToString(this DateTime? dateTime)
        {
            return dateTime?.ConvertToString();
        }

        public static string ConvertToString(this DateTime dateTime)
        {
            return dateTime.ToString("dd MMM yyyy");
        }
    }

    public static class NotificationExtensions
    {
        public static IEnumerable<NotificationBannerModel> CreateNotificationBanners(
            this IEnumerable<Notification> notifications,
            ClaimsPrincipal user,
            IAuthorizationService authorizationService)
        {
            return notifications.Select(async n =>
                {
                    var fullAccess = await authorizationService.CanEdit(user, n);
                    return new NotificationBannerModel(n, fullAccess: fullAccess, showLink: true);
                })
                .Select(n => n.Result);
        }
    }

    public static class NullableBoolExtensions
    {
        public static string FormatYesNo(this bool? x)
        {
            if (x == null)
            {
                return string.Empty;
            }
            else
            {
                return x.Value ? "Yes" : "No";
            }
        }
    }
}
