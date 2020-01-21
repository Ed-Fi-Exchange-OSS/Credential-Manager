using System.ComponentModel.DataAnnotations;

namespace Ed_Fi.Credential.MVC.Models
{
    public class ApiClientModel
    {
        public int ApiClientId { get; set; }
        public string Key { get; set; }
         public string Secret { get; set; }
        public string SecretDisplay { get; set; }
        [Display(Name = "Api Client Name")]
        public string Name { get; set; }
        public bool IsApproved { get; set; }
        public bool UseSandbox { get; set; }
        public int SandboxType { get; set; }
        public bool SecretIsHashed { get; set; }

        [Display(Name = "Application")]
        public int? ApplicationApplicationId { get; set; }
        [Display(Name = "Application")]
        public string ApplicationApplicationName { get; set; }

        [Display(Name = "User")]
        public int? UserUserId { get; set; }


        [Display(Name = "Vendor")]
        public string ApplicationVendorVendorName { get; set; }
        [Display(Name = "Profile")]
        public string ApplicationProfile { get; set; }
        [Display(Name = "Claimset")]
        public string ApplicationClaimSetName { get; set; }
    
        public string Tooltip { get { return SecretIsHashed ? Constants.HASHED_SECRET_MESSAGE : Constants.NOT_HASHED_SECRET_MESSAGE; } }
        
        public string Errors { get; set; }
        //for modal
        public string Action { get; set; }
        public string Title { get; set; }
    }
}