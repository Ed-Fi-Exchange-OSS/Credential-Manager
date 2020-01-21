using System.ComponentModel.DataAnnotations;

namespace Ed_Fi.Credential.MVC.Models
{
    public class ApiEdOrgModel
    {
        public int ApiClientId { get; set; }

        [Display(Name = "Agency Key")]
        public int EducationOrganizationId { get; set; }

        [Display(Name = "Agency Name")]
        public string NameOfInstitution { get; set; }

        [Display(Name = "Is School")]
        public bool IsSchool { get; set; }

        [Display(Name = "LEA")]
        public string Lea { get; set; }

        [Display(Name = "LEA ID")]
        public int LeaId { get; set; }

        [Display(Name = "LEA Name")]
        public string LeaName { get; set; }

        [Display(Name = "City")]
        public string City { get; set; }
    }
}