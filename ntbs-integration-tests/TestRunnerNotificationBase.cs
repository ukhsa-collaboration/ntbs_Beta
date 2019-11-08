using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using ntbs_service;
using ntbs_service.Helpers;

namespace ntbs_integration_tests
{
    public abstract class TestRunnerNotificationBase : TestRunnerBase
    {
        protected virtual string NotificationSubPath { get; }

        protected TestRunnerNotificationBase(NtbsWebApplicationFactory<Startup> factory) : base(factory) { }

        protected string GetCurrentPathForId(int id)
        {
            return GetPathForId(NotificationSubPath, id);
        }

        protected string GetPathForId(string subPath, int id)
        {
            return RouteHelper.GetNotificationPath(subPath, id);
        }

        protected string GetHandlerPath(Dictionary<string, string> formData, string handlerPath, int id = 0)
        {
            var queryString = string.Join("&", formData.Select(kvp => $"{kvp.Key}={kvp.Value}"));
            return GetPathForId($"{NotificationSubPath}/{handlerPath}?{queryString}", id);
        }
    }
}
