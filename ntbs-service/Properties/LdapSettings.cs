namespace ntbs_service.Properties
{
    public class LdapSettings
    {
        public string AdAddressName { get; set; }
        public int Port { get; set; }
        public string UserIdentifier { get; set; }
        public string Password { get; set; }

        /// <summary>
        /// Base domain in the fully qualified domain names, e.g. "DC=ntbs,DC=phe,DC=com"
        /// </summary>
        public string BaseDomain { get; set; }
    }
}
