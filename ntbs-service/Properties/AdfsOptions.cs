namespace ntbs_service.Properties
{
    public class AdfsOptions
    {
        /** Base url for the AD FS instace */
        public string AdfsUrl { get; set; }
        /** The ntbs url, used as identifier of the app to adfs */
        public string Wtrealm { get; set; }
        /** The amount of time an authentication cookie is valid for */
        public string CookieExpireTimeSpan { get; set; }
    }
}
