using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace ntbs_service.TagHelpers
{
    [HtmlTargetElement("nhs-details", Attributes = "is-open")]
    public class NhsDetailsExpanderOpenTagHelper : TagHelper
    {
        [HtmlAttributeName("is-open")]
        public bool IsOpen { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            // Defer to other tag helpers first
            // https://docs.microsoft.com/en-us/aspnet/core/mvc/views/tag-helpers/authoring?view=aspnetcore-3.0#avoid-tag-helper-conflicts
            // If more helpers are to be run in conjunction will need to investigate tag helper 'order'
            if (!output.Content.IsModified)
            {
                _ = await output.GetChildContentAsync();
            }

            if (IsOpen)
            {
                if (!output.Attributes.TryGetAttribute("open", out var _))
                {
                    output.Attributes.Add("open", null);
                }
            }
        }
    }
}
