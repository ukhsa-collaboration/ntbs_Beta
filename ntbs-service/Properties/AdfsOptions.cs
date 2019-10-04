public class AdfsOptions
{
    /** This allows us to use group names agnostic of any naming convention prefixes that exist in the setup */
    public string AdGroupsPrefix { get; set; }
    public string NationalTeamAdGroup { get; set; } = "Global.NIS.NTBS.NTA";
    /** TB service groups are of the format Global.NIS.NTBS.Service_<service-specifc-postfix> */
    public string ServiceGroupAdPrefix { get; set; } = "Global.NIS.NTBS.Service";
    /** The ntbs url, used as identifier of the app to adfs */
    public string Wtrealm { get; set; }
}