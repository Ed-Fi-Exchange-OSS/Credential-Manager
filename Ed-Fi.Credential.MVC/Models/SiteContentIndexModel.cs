using System.Web.Mvc;

namespace Ed_Fi.Credential.MVC.Models
{
    public class SiteContentModel
    {
        public int Id { get; set; }
        public int SiteContentTypeId { get; set; }
        [AllowHtml]
        public string Body { get; set; }
    }

    public class SiteContentIndexModel
    {
        public int SiteContentTypeId { get; set; }

        public SiteContentModel SiteContent { get; set; }
    }
}