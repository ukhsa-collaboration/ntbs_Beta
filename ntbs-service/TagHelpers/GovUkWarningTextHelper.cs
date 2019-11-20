using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using NHSUK.FrontEndLibrary.TagHelpers.Constants;

namespace ntbs_service.TagHelpers
{
    [HtmlTargetElement("govuk-warning-text")]
    public class GovUkWarningTextHelper : TagHelper
    {
        [HtmlAttributeName("is-hidden")]
        public bool? IsHidden { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            await base.ProcessAsync(context, output);
            output.TagName = HtmlElements.Div;

            output.AddClass("govuk-warning-text", HtmlEncoder.Default);

            if (IsHidden == true)
            {
                output.AddClass("hidden", HtmlEncoder.Default);
            }

            output.PreContent.SetHtmlContent(
                "<span class=\"govuk-warning-text__icon\" aria-hidden=\"true\">!</span>" +
                "<div class=\"govuk-warning-text__text\">" +
                "<span class=\"govuk-warning-text__assistive\">Warning</span>");

            var content = (await output.GetChildContentAsync()).GetContent();
            output.Content.SetHtmlContent(content);

            output.PostContent.SetHtmlContent(
                "</div>");
        }
    }
}
