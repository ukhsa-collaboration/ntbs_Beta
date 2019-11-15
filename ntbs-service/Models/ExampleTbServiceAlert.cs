using System;

namespace ntbs_service.Models
{
    public class ExampleTbServiceAlert : Alert
    {
        public string TransferReason { get; set; }
        public string MessageToNewCaseManager { get; set; }
    }
}
