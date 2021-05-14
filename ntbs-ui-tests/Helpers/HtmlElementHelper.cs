using System.Collections.ObjectModel;
using ntbs_service.Helpers;
using OpenQA.Selenium;

namespace ntbs_ui_tests.Helpers
{
    public static class HtmlElementHelper
    {
        public static IWebElement FindElementById(IWebDriver browser, string elementId)
        {
            return browser.FindElement(By.Id(elementId));
        }

        public static ReadOnlyCollection<IWebElement> FindElementsById(IWebDriver browser, string elementId)
        {
            return browser.FindElements(By.Id(elementId));
        }

        public static IWebElement FindElementByXpath(IWebDriver browser, string xpath)
        {
            return browser.FindElement(By.XPath(xpath));
        }

        public static ReadOnlyCollection<IWebElement> FindElementsByXpath(IWebDriver browser, string xpath)
        {
            return browser.FindElements(By.XPath(xpath));
        }

        public static string GetSectionIdFromSection(string section)
        {
            return OverviewSubPathToAnchorMap.GetOverviewAnchorId(
                (string)typeof(NotificationSubPaths).GetProperty($"Edit{section}").GetValue(null, null));
        }
    }
}
