namespace ntbs_service_unit_tests.TestHelpers
{
    public static class ContextHelper
    {
        public static void DisableAudits()
        {
            Audit.Core.Configuration.AuditDisabled = true;
        }
    }
}
