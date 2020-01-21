using System.ComponentModel.DataAnnotations;

namespace Ed_Fi.Credential.MVC.Models
{
    public class ClaimsetDetailModel
    {
        [Display(Name = "Name")]
        public string ClaimSetName { get; set; } 
        public string PlainEnglish { get; set; }
        [Display(Name = "Domain")]
        public string LogicalDomain { get; set; } 
        public bool PrimarySis { get; set; }
        [Display(Name="Public")]
        public bool PublicClaimset { get; set; }
        [Display(Name = "Choice")]
        public bool ChoiceClaimset { get; set; }
        [Display(Name = "School Level")]
        public bool SchoolLevelClaimset { get; set; }
        [Display(Name = "SPED")]
        public bool ReadOnlyClaimset { get; set; }
        public bool Active { get; set; }
        [Display(Name = "Year")]
        public int? RequirementYear { get; set; } 
        public int? ProfileId { get; set; }
        [Display(Name = "Profile")]
        public string ProfileName { get; set; } 
        public string ClaimsetTypeName { get; set; }
    }
}