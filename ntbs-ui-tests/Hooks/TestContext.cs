using System.Collections.Generic;

namespace ntbs_ui_tests.Hooks
{
    public class TestContext
    {
        public UserConfig LoggedInUser { get; set; }
        public List<int> AddedNotificationIds { get; set; } = new List<int>();
    }
}
