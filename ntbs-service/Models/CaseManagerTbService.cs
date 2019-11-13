namespace ntbs_service.Models
{
    public class CaseManagerTbService
    {
        public string TbServiceCode { get; set; }
        public TBService TbService { get; set; }

        public string CaseManagerEmail { get; set; }
        public CaseManager CaseManager { get; set; }
    }
}
