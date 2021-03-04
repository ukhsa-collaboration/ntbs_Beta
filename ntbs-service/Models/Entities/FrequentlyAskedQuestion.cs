namespace ntbs_service.Models.Entities
{
    public class FrequentlyAskedQuestion
    {

        public int Id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public int OrderIndex { get; set; }
        public string AnchorLink { get; set; }
    }
}
