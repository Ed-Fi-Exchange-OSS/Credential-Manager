using System.ComponentModel.DataAnnotations;

namespace Ed_Fi.Credential.MVC.Models
{
    public class ApplicationModel
    {
        public int ApplicationId { get; set; }
        public string ApplicationName { get; set; }

        [Display(Name = "Vendor Id")]
        public int? VendorVendorId { get; set; }

        [Display(Name = "Claim Set Name")]
        public string ClaimSetName { get; set; }

        [Display(Name = "Profile")]
        public int? ProfileId { get; set; }
        [Display(Name = "Profile Name")]
        public string ProfileName { get; set; }

        [Display(Name = "Plain English")]
        public string PlainEnglish { get; set; }

        [Display(Name = "Claimset Type")]
        public string ClaimSetType { get; set; }

        //for modal
        public string Action { get; set; }
        public string Title { get; set; }
    }
}