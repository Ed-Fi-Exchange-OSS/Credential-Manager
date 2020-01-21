namespace Ed_Fi.Credential.Business.Models
{
    public class EdOrgL2Rule
    {
        public short SchoolYear { get; set; }
        public int EdOrgId { get; set; }
        public string EdOrgName { get; set; }
        public bool IsChoice { get; set; }
        public string EdOrgType => IsChoice ? "Choice" : "Public";
        public int RuleCode { get; set; }
        public string KbaUrl { get; set; }
        public string ShortDescription { get; set; }
        public string ValidationCategories { get; set; }
        public int Count { get; set; }
    }
}
