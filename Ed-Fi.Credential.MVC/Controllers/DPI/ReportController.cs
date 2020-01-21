using Ed_Fi.Credential.Business;
using Ed_Fi.Credential.MVC.Controllers.Base;
using Ed_Fi.Credential.MVC.ImplementationSpecific;
using Ed_Fi.Credential.MVC.Models;
using System.Linq;
using System.Web.Mvc;

namespace Ed_Fi.Credential.MVC.Controllers.DPI
{
    [RequirePrivilege(WisePrivilege.ViewReport)]
    public class ReportController : BaseController
    {
         private readonly IApiClientBusiness _apiClientBusiness;
        private readonly IClaimSetBusiness _claimSetBusiness;

        public ReportController(IBaseControllerContext controllerContext, IApiClientBusiness apiClientBusiness, IClaimSetBusiness claimSetBusiness)
            : base(controllerContext)
        {
            _apiClientBusiness = apiClientBusiness;
            _claimSetBusiness = claimSetBusiness;
        }

        // GET: Report
        public ActionResult ApisByAgency()
        {
            return View();
        }

        public JsonResult ByAgencyLea()
        {
            var cross = _apiClientBusiness.GetCrosstab();
            var leaModels = cross.Select(c => new
            {
                EducationOrganizationId = c.SchoolId ?? c.LeaId.GetValueOrDefault(),
                Name = c.SchoolName ?? c.LeaName,
                Lea = string.Format("{0} - {1}", c.LeaId, c.LeaName)
            }).Distinct();

            return Json(leaModels);

        }

        public JsonResult ByAgencyApi(int orgId)
        {
            var apis = _apiClientBusiness.GetApiClients(orgId);

            var models = apis.Select(api => new
            {
                ApiClientId = api.ApiClientId,
                VendorName = api.Application.Vendor.VendorName,
                Key = api.Key,
                Claimset = api.Application.ClaimSetName,
                Profile = api.Application.Profiles?.Select(p => p.ProfileName).FirstOrDefault()
            });

            return Json(models);

        }

        public ActionResult Crosstab()
        {
            return View();
        }

        public JsonResult CrosstabRead()
        {
            var crosstab = _apiClientBusiness.GetCrosstab();
            return Json(crosstab);

        }

        public ActionResult SecurityStrategies()
        {
            return View();
        }

        public JsonResult SecurityStrategyRead()
        {
            var strategies = _claimSetBusiness.GetSecurityStrategies().Select(n => new
            {
                Id = string.Format("{0}-{1}", n.ClaimsetId, n.ResourceClaimId),
                n.ClaimSetName,
                n.ResourceName,
                n.ReadStrategy,
                n.CreateStrategy,
                n.UpdateStrategy,
                n.DeleteStrategy
            });
            return Json(strategies);

        }

        public ActionResult ClaimsetDetails()
        {
            return View();
        }

        public JsonResult ClaimsetDetailsRead()
        {
            var strategies = _claimSetBusiness.GetClaimSetDetailsWithProfiles().Select(c=>new ClaimsetDetailModel
            {
                Active = c.Active,
                ChoiceClaimset = c.ChoiceClaimset,
                ClaimSetName = c.ClaimSetName,
                ClaimsetTypeName = c.ClaimsetType.Name,
                LogicalDomain = c.LogicalDomain,
                PlainEnglish = c.PlainEnglish,
                PrimarySis = c.PrimarySis,
                ProfileId = c.ProfileId,
                ProfileName = c.ProfileName,
                PublicClaimset = c.PublicClaimset,
                ReadOnlyClaimset = c.ReadOnlyClaimset,
                RequirementYear = c.RequirementYear,
                SchoolLevelClaimset = c.SchoolLevelClaimset
            });
            return Json(strategies);

        }

    }
}