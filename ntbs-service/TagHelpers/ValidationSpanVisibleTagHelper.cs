using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace ntbs_service.TagHelpers
{
    [HtmlTargetElement("span", Attributes = "has-error")]
    public class ValidationSpanVisibleTagHelper : TagHelper
    {
        [HtmlAttributeName("has-error")]
        public bool HasError { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            // As this is to interact on spans with nhs-span-type, defer to other tag helpers first
            // https://docs.microsoft.com/en-us/aspnet/core/mvc/views/tag-helpers/authoring?view=aspnetcore-3.0#avoid-tag-helper-conflicts
            // If more helpers are to be run in conjunction will need to investigate tag helper 'order'
            if (!output.Content.IsModified)
            {
                _ = await output.GetChildContentAsync();
            }

            if (!HasError)
            {
                var classList = "hidden";
                if (output.Attributes.TryGetAttribute("class", out var classAttributeValue))
                {
                    classList = $"{classAttributeValue.Value} {classList}";
                }

                output.Attributes.SetAttribute("class", classList);
            }
        }
    }
}
