using System.Collections.Generic;
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

        protected string GetPathForId(string subPath, int id, Dictionary<string, string> queryDictionary = null)
        {
            return RouteHelper.GetNotificationPath(subPath, id, queryDictionary);
        }

        protected string GetHandlerPath(Dictionary<string, string> queryDictionary, string handlerPath, int id = 0)
        {
            return GetPathForId($"{NotificationSubPath}/{handlerPath}", id, queryDictionary);
        }
    }
}
