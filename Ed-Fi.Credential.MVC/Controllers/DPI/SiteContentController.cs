using Ed_Fi.Credential.Business;
using Ed_Fi.Credential.Domain.Enums;
using Ed_Fi.Credential.MVC.Controllers.Base;
using Ed_Fi.Credential.MVC.ImplementationSpecific;
using Ed_Fi.Credential.MVC.Models;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ed_Fi.Credential.MVC.Controllers.DPI
{
    [RequirePrivilege(WisePrivilege.EditCredential)]
    public class SiteContentController : BaseController
    {
        private readonly ISiteContentBusiness _contentBusiness;

        public SiteContentController(IBaseControllerContext controllerContext, ISiteContentBusiness contentBusiness, ILookupBusiness lookupBusiness)
            :base(controllerContext)
        {
            _contentBusiness = contentBusiness;

            var lookupDomain = lookupBusiness.GetByType(typeof (SiteContentType).Name);
            var lookupModel = lookupDomain.Select(l => new SelectListItem {Text = l.Description, Value = l.Id.ToString()})
                .ToList(); 

            ViewData[Constants.CONTENT_TYPES] = lookupModel;
        }

        [HttpGet]
        public ActionResult Index(int siteContentTypeId = (int)SiteContentType.AgencyAgreement)
        {
            var domain = _contentBusiness.GetByContentTypeId(siteContentTypeId);

            var contentModel = new SiteContentModel
            {
                Id = domain.Id,
                Body = domain.Body,
                SiteContentTypeId = domain.SiteContentTypeId
            };
        
            var model = new SiteContentIndexModel
            {
                SiteContentTypeId = siteContentTypeId,
                SiteContent = contentModel
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(SiteContentIndexModel model)
        {
             var domain = _contentBusiness.GetByContentTypeId(model.SiteContent.SiteContentTypeId);
            domain.Body = HttpUtility.HtmlDecode(model.SiteContent.Body);
            _contentBusiness.SaveChanges(WamsUser.WamsId);
            model.SiteContentTypeId = model.SiteContent.SiteContentTypeId;
            return RedirectToAction("Index", new {siteContentTypeId = model.SiteContent.SiteContentTypeId});
        }
    }
}