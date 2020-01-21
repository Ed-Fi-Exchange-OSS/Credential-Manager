using System.ComponentModel.DataAnnotations;

namespace Ed_Fi.Credential.MVC.Models
{
    public class NewVendorModel
    {

        [Display(Name = "Vendor Name")]
        public string VendorName { get; set; }

        [Display(Name = "Namespace Prefix")]
        public string NamespacePrefix { get; set; }
        [Display(Name = "Email Domain")]
        public string EmailDomain { get; set; }
    }
}