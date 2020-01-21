using Ed_Fi.Credential.Domain.Model;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ed_Fi.Credential.MVC.Areas.Vendor.Models
{
    public class VendorApiClientModel
    {
        public int ApiClientId { get; set; }
        [Display(Name = "Api Client Name")]
        public string Name { get; set; }
        public string Key { get; set; }
        public bool SecretIsHashed { get; set; }
        public string Secret { get; set; }
        public string SecretDisplay { get; set; }

        [Display(Name = "Application")]
        public string ApplicationApplicationName { get; set; }
        [Display(Name = "Claimset")]
        public string ApplicationClaimSetName { get; set; }

        public List<LatestEdOrg> Organizations { get; set; }
       
        public List<string> ProfileNames { get; set; }

        public VendorApiClientModel() {
            Organizations= new List<LatestEdOrg>();
            ProfileNames = new List<string>();
        }
    }
}