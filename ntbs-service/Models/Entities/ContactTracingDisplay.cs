namespace ntbs_service.Models.Entities
{
    public partial class ContactTracing
    {
        public int? TotalContactsIdentified => CalculateSum(AdultsIdentified, ChildrenIdentified);

        public int? TotalContactsScreened => CalculateSum(AdultsScreened, ChildrenScreened);

        public int? TotalContactsActiveTB => CalculateSum(AdultsActiveTB, ChildrenActiveTB);

        public int? TotalContactsLatentTB => CalculateSum(AdultsLatentTB, ChildrenLatentTB);

        public int? TotalContactsStartedTreatment => CalculateSum(AdultsStartedTreatment, ChildrenStartedTreatment);

        public int? TotalContactsFinishedTreatment => CalculateSum(AdultsFinishedTreatment, ChildrenFinishedTreatment);

        private int? CalculateSum(int? x, int? y)
        {
            if (x == null && y == null)
            {
                return null;
            }
            return (x ?? 0) + (y ?? 0);
        }
    }
}
