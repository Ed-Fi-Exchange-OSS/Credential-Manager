using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ed_Fi.Credential.Domain.Model;

namespace Ed_Fi.Credential.MVC.Areas.Agency.Models
{
    public class AgencyClientModel
    {
        public int ApiClientId { get; set; }
        [Display(Name = "Api Client Name")]
        public string Name { get; set; }
        public string Key { get; set; }
        public bool SecretIsHashed { get; set; }
        public string Secret { get; set; }
        public string SecretDisplay { get; set; }
        public string ApplicationApplicationName { get; set; }
        [Display(Name = "Claim Set Name")]
        [Required]
        public string ApplicationClaimSetName { get; set; }
        [Display(Name = "Vendor")]
        [Required]
        public int ApplicationVendorVendorId { get; set; }
        [Display(Name = "Vendor Name")]
        public string ApplicationVendorVendorName { get; set; }


      
        [Display(Name = "Resource Access")]
        public virtual string ApplicationClaimsetDetailPlainEnglish { get; set; }
        public bool ApplicationClaimsetDetailPrimarySis { get; set; } // PrimarySis
        public bool ApplicationClaimsetDetailPublicClaimset { get; set; } // PublicClaimset
        public bool ApplicationClaimsetDetailChoiceClaimset { get; set; } // ChoiceClaimset
        public bool ApplicationClaimsetDetailSchoolLevelClaimset { get; set; } // SchoolLevelClaimset
        public bool ApplicationClaimsetDetailReadOnlyClaimset { get; set; } // ReadOnlyClaimset

        public string ApplicationClaimsetDetailClaimsetTypeName { get; set; } // ReadOnlyClaimset
        public int? ApplicationClaimsetDetailClaimsetTypeId { get; set; } // ReadOnlyClaimset

        public List<LatestEdOrg> Organizations { get; set; }

        public List<string> ProfileNames { get; set; }

       public string Tooltip { get { return SecretIsHashed ? Constants.HASHED_SECRET_MESSAGE : Constants.NOT_HASHED_SECRET_MESSAGE; } }

        //Editing lists
        public string Action { get; set; }
        public List<SelectListItem> Vendors { get; set; }
        public List<SelectListItem> Applications { get; set; }
        public List<SelectListItem> Schools { get; set; }
        public string VendorSubscriptionMessage { get; set; }
        public bool IsNew
        {
            get { return ApiClientId == 0; }
        }
        public List<string> SelectedSchoolIds { get; set; }

        public string SchoolsString {
            get { return "[\"" + string.Join("\",\"", SelectedSchoolIds) +"\"]";
            }
        }


        public AgencyClientModel()
        {
            Organizations = new List<LatestEdOrg>();
            ProfileNames = new List<string>();
        }
    }
}