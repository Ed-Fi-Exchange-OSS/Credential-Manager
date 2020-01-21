using System.ComponentModel.DataAnnotations;

namespace Ed_Fi.Credential.MVC.Areas.Vendor.Models
{
    public class UrlModel
    {
        public string Name { get; set; }
        
        [Display(Name="URL")]
        public string Url { get; set; }
    }
}