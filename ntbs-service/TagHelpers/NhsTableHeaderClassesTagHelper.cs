using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ntbs_service.TagHelpers
{
    [HtmlTargetElement("nhs-table-item")]
    public class NhsTableHeaderClassesTagHelper : TagHelper
    {
        public string Classes { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            // Defer to other tag helpers first
            // https://docs.microsoft.com/en-us/aspnet/core/mvc/views/tag-helpers/authoring?view=aspnetcore-3.0#avoid-tag-helper-conflicts
            // If more helpers are to be run in conjunction will need to investigate tag helper 'order'
            if (!output.Content.IsModified)
            {
                _ = await output.GetChildContentAsync();
            }

            if (!String.IsNullOrEmpty(Classes))
            {
                var classList = Classes;
                if (output.Attributes.TryGetAttribute("class", out var classAttributeValue))
                {
                    classList = $"{classAttributeValue.Value} {Classes}";
                }

                output.Attributes.SetAttribute("class", classList);
            }
        }
    }
}
