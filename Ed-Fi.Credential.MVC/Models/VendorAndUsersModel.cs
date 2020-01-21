using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ed_Fi.Credential.MVC.Models
{
    public class VendorAndUsersModel
    {
        public int VendorId { get; set; }

        [Display(Name = "Vendor Name")]
        public string VendorName { get; set; }

        [Display(Name = "Namespace Prefixes")]
        public List<NamespacePrefixModel> NamespacePrefixes { get; set; }

        [Display(Name = "Email Domains")]
        public List<EmailDomainModel> EmailDomains { get; set; }

        [Display(Name = "Users")]
        public List<UserModel> Users { get; set; }

        public bool CanEdit { get; set; }

    }

    public class NamespacePrefixModel {
        public int VendorNamespacePrefixId { get; set; } // VendorNamespacePrefixId (Primary key)
        public string NamespacePrefix { get; set; } // NamespacePrefix (length: 255)
    }
   
}