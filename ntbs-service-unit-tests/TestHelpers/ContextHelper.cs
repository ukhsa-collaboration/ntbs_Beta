namespace ntbs_service_unit_tests.TestHelpers
{
    public static class ContextHelper
    {
        // This method should be used in the constructor of any unit tests which use an NTBS context
        public static void DisableAudits()
        {
            Audit.Core.Configuration.AuditDisabled = true;
        }
    }
}
