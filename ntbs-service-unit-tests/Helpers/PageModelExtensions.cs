using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq;

namespace ntbs_service_unit_tests.Helpers
{
    public static class PageModelExtensions
    {
        public static void MockOutSession(this PageModel pageModel, HttpContext context = null)
        {
            if (context != null)
            {
                context.Session = new Mock<ISession>().Object;
            }
            else
            {
                var httpContext = new Mock<HttpContext>();
                httpContext.Setup(ctx => ctx.Session).Returns(new Mock<ISession>().Object);
                pageModel.PageContext.HttpContext = httpContext.Object;
            }
        }
    }
}
