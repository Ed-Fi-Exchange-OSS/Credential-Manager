using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Ed_Fi.Credential.MVC.Models
{
    public class UserModel
    {
        [ScaffoldColumn(false)]
        public int UserId { get; set; }
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Display(Name = "WAMS User Id")]
        public string WamsUserName { get; set; }

        [Display(Name = "Vendor")]
        public int? VendorVendorId { get; set; }

        public string VendorName { get; set; }

           //for modal
        public string Action { get; set; }
        public string Title { get; set; }
        
        public bool WamsUserNameIsEmpty
        {
            get { return string.IsNullOrEmpty(WamsUserName) || string.IsNullOrWhiteSpace(WamsUserName); }
        }
        public IEnumerable<SelectListItem> Vendors { get; set; }

    }
}