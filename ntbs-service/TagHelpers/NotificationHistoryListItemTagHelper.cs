using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ntbs_service.TagHelpers
{
    // ReSharper disable once ClassNeverInstantiated.Global
    // ReSharper disable once UnusedType.Global
    [HtmlTargetElement("li", Attributes = "notification-history-item")]
    public class NotificationHistoryListItemTagHelper : TagHelper
    {
        public NotificationHistoryListItemModel NotificationHistoryItem { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            await base.ProcessAsync(context, output);

            output.Attributes.TryGetAttribute("class", out var classAttributeValue);
            object value = $"{classAttributeValue?.Value} history-list-item";
            output.Attributes.SetAttribute("class", value);

            var userHtml = NotificationHistoryItem.UserId == null
                ? $@"<span class=""history-list-item__user"">{NotificationHistoryItem.Username}</span>"
                : $@"<a class=""history-list-item__user"" href=""/ContactDetails/{NotificationHistoryItem.UserId}"">
                    {NotificationHistoryItem.Username}
                </a>";
            output.Content.SetHtmlContent($@"
                <span class=""history-list-item__date"">{NotificationHistoryItem.Date:dd MMM yyyy, hh:mm}</span>
                {userHtml}
                <span class=""history-list-item__action"">{NotificationHistoryItem.Action}</span>
                <span class=""history-list-item__subject"">{NotificationHistoryItem.Subject}</span>
            ");
        }
    }

    public class NotificationHistoryListItemModel
    {
        public DateTime Date { get; set; }
        public string Username { get; set; }
        public string UserId { get; set; }
        public string Action { get; set; }
        public string Subject { get; set; }
    }
}
