using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Ed_Fi.Credential.MVC.Models
{
    public class MoveApiClientModel
    {
        public int ApiClientId { get; set; }
        public int? OldApplicationApplicationId { get; set; }
        [Display(Name = "Api Client Name")]
        public string Name { get; set; }

        [Display(Name = "Application")]
        public int? ApplicationApplicationId { get; set; }

        public List<SelectListItem> Applications { get; set; }

    }
}