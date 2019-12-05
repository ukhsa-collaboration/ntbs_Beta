namespace ntbs_service.Models.Entities
{
    public class CaseManagerTbService
    {
        public string TbServiceCode { get; set; }
        public virtual TBService TbService { get; set; }

        public string CaseManagerEmail { get; set; }
        public virtual CaseManager CaseManager { get; set; }
    }
}
