using Ed_Fi.Credential.Business;
using Ed_Fi.Credential.Business.Common.Interfaces;
using Ed_Fi.Credential.Domain.Enums;
using Ed_Fi.Credential.MVC.Areas.Agency.Models;
using Ed_Fi.Credential.MVC.Controllers.Base;
using Ed_Fi.Credential.MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Ed_Fi.Credential.MVC.ImplementationSpecific;

namespace Ed_Fi.Credential.MVC.Areas.Agency.Controllers
{
    [RequirePrivilege(WisePrivilege.EditVendorSubscription)]
    public class VendorSubscriptionController : BaseController
    {
        private readonly IApiClientBusiness _apiClientBusiness;
        private readonly IAgreementBusiness _agreementBusiness;
        private readonly IVendorBusiness _vendorBusiness;
        private readonly IVendorSubscriptionBusiness _vendorSubscriptionBusiness;
        private readonly ISiteContentBusiness _siteContentBusiness;
        private readonly IOdsBusiness _odsBusiness;
        private readonly IApplicationBusiness _applicationBusiness;
      
        public VendorSubscriptionController(IBaseControllerContext controllerContext,
            IApiClientBusiness apiClientBusiness, IAgreementBusiness agreementBusiness, IVendorBusiness vendorBusiness,
            IVendorSubscriptionBusiness vendorSubscriptionBusiness, ISiteContentBusiness siteContentBusiness,
            IOdsBusiness odsBusiness, IApplicationBusiness applicationBusiness)
            : base(controllerContext)
        {
            _apiClientBusiness = apiClientBusiness;
            _vendorSubscriptionBusiness = vendorSubscriptionBusiness;
            _siteContentBusiness = siteContentBusiness;
            _odsBusiness = odsBusiness;
            _applicationBusiness = applicationBusiness;
            _vendorBusiness = vendorBusiness;
            _agreementBusiness = agreementBusiness;
        }


        public ActionResult Credentials()
        {
            var agreement = _agreementBusiness.FindByAgencyNotVendorSub(EducationOrganizationId, AgreementType.Agency);
            var agree = agreement != null && agreement.Agree;
            var agreementSiteContent = _siteContentBusiness.GetByContentTypeId((int)SiteContentType.AgencyAgreement);
            var subscriptionSiteContent = _siteContentBusiness.GetByContentTypeId((int)SiteContentType.VendorSubscription);
            var org = _odsBusiness.GetEducationOrganization(EducationOrganizationId);
            bool isChoice = org.IsChoice;
            var model = new VendorSubscriptionModel
            {
                AgreementMessage = agreementSiteContent.Body,
                VendorSubscriptionMessage = subscriptionSiteContent.Body,
                HasAgreed = agree,
                IsChoice = isChoice,
            };
            var clients = _apiClientBusiness.GetApiClientsWithDetails(EducationOrganizationId).Select(a => new AgencyClientModel
            {
                ApiClientId = a.ApiClientId,
                ApplicationApplicationName = a.Application.ApplicationName,
                ApplicationClaimSetName = a.Application.ClaimSetName,
                ApplicationClaimsetDetailChoiceClaimset = a.Application.ClaimsetDetail.ChoiceClaimset,
                ApplicationClaimsetDetailClaimsetTypeId = a.Application.ClaimsetDetail.ClaimsetTypeId,
                ApplicationClaimsetDetailClaimsetTypeName = a.Application.ClaimsetDetail.ClaimsetType.Name,
                ApplicationClaimsetDetailPlainEnglish = a.Application.ClaimsetDetail.PlainEnglish,
                ApplicationClaimsetDetailPrimarySis = a.Application.ClaimsetDetail.PrimarySis,
                ApplicationClaimsetDetailPublicClaimset = a.Application.ClaimsetDetail.PublicClaimset,
                ApplicationClaimsetDetailReadOnlyClaimset = a.Application.ClaimsetDetail.ReadOnlyClaimset,
                ApplicationClaimsetDetailSchoolLevelClaimset = a.Application.ClaimsetDetail.SchoolLevelClaimset,
                ApplicationVendorVendorId = a.Application.Vendor.VendorId,
                ApplicationVendorVendorName = a.Application.Vendor.VendorName,
                Name = a.Name,
                Key = a.Key,
                SecretIsHashed = a.SecretIsHashed,
                Secret = a.Secret,
                ProfileNames = a.Application.Profiles.Select(p => p.ProfileName).ToList(),
                Organizations = a.ApplicationEducationOrganizations.Select(e => e.EdOrg).ToList(),

            }).ToList();

            model.AgencyApiClients =
                clients.Where(c => c.ApplicationClaimsetDetailSchoolLevelClaimset == false).ToList();
            model.SchoolApiClients =
                clients.Where(c => c.ApplicationClaimsetDetailSchoolLevelClaimset == true).ToList();

            return View(model);
        }


        [HttpPost]
        public JsonResult Agreement(bool agree)
        {
            _agreementBusiness.UpsertNotVendorSub(
                EducationOrganizationId,
                WamsUser.WamsId,
                agree,
                AgreementType.Agency,
                WamsUser.WamsId);

            return Json(null);
        }


        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ResetSecret(int id)
        {
            var client = _apiClientBusiness.GetApiClient(id);
            var model = new ChangeSecretModel { ApiClientId = client.ApiClientId, Key = client.Key, Secret = Guid.NewGuid().ToString() };

            return PartialView("_ResetSecret", model);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult ResetSecret(ChangeSecretModel model)
        {
            try
            {
                if (model != null && ModelState.IsValid)
                {
                    _apiClientBusiness.ChangeSecret(WamsUser.WamsId, model.ApiClientId, model.Secret);

                }
                return Json(new { success = true });
            }
            catch (Exception)
            {
                return Json(new
                {
                    success = false,
                    Errors = string.Format("Could not reset secret")
                });
            }


        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult NewSubscription(bool isSchoolLevel)
        {
            var org = _odsBusiness.GetEducationOrganization(EducationOrganizationId);
            bool isChoice = org.IsChoice;
            var subscriptionSiteContent = _siteContentBusiness.GetByContentTypeId((int)SiteContentType.VendorSubscription);

            var model = new AgencyClientModel { Action = "NewSubscription" };
            model.VendorSubscriptionMessage = subscriptionSiteContent.Body;
            model.ApplicationClaimsetDetailSchoolLevelClaimset = isSchoolLevel;

            model.Vendors = _vendorBusiness.GetVendors().Where(v =>
                    v.Applications.Any(c => c.ClaimsetDetail.Active
                                           && c.ClaimsetDetail.SchoolLevelClaimset == isSchoolLevel
                                           && ((isChoice && c.ClaimsetDetail.ChoiceClaimset) || (!isChoice && c.ClaimsetDetail.PublicClaimset))))
                .OrderBy(v => v.VendorName)
                .Select(t => new SelectListItem { Value = t.VendorId.ToString(), Text = t.VendorName.ToString() }).ToList();

            model.Schools = _odsBusiness.GetLeaSchools(EducationOrganizationId)
              .Select(t => new { t.EducationOrganizationId, t.NameOfInstitution }).ToList()
              .Select(t => new SelectListItem() { Value = t.EducationOrganizationId.ToString(), Text = t.NameOfInstitution }).ToList()
              ;
            model.SelectedSchoolIds = new List<string>();
            model.Applications = VendorApplications(model.ApplicationClaimsetDetailSchoolLevelClaimset, model.ApplicationVendorVendorId, model.ApplicationClaimSetName);

            return PartialView("EditSubscription", model);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult NewSubscription(AgencyClientModel model)
        {
            if (model != null && ModelState.IsValid)
            {
                try
                {
                    var vendor = _vendorBusiness.GetVendorById(model.ApplicationVendorVendorId);

                    int?[] selectedSchoolIds = null;
                    if (model.SelectedSchoolIds != null && model.SelectedSchoolIds.Count > 0)
                    {
                        selectedSchoolIds = model.SelectedSchoolIds.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => (int?)int.Parse(x)).ToArray();
                    }

                    //TODO- this is implementation specific and not handled here
                    var email = _vendorSubscriptionBusiness.Subscribe(WamsUser.WamsId, EducationOrganizationId, vendor.VendorName,
                           model.ApplicationClaimSetName, selectedSchoolIds);
                    if (email.To.Any())
                    {
                        MailService ms = new MailService();
                        ms.SendEmail(email);
                    }

                    if (vendor != null)
                    {
                        _agreementBusiness.UpsertVendorSub(
                            EducationOrganizationId,
                            WamsUser.WamsId,
                            vendor.VendorId);
                    }
                    return Json(new { success = true });
                }
                catch (Exception e)
                {
                    return Json(new
                    {
                        success = false,
                        Errors = string.Format("Could not add subscription: {0}", e.Message)
                    });


                }
            }


            return Json(new { success = true });


        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult EditSubscription(int id)
        {
            var org = _odsBusiness.GetEducationOrganization(EducationOrganizationId);
            bool isChoice = org.IsChoice;
            var subscriptionSiteContent = _siteContentBusiness.GetByContentTypeId((int)SiteContentType.VendorSubscription);

            var client = _apiClientBusiness.GetApiClientWithDetails(id);
            var model = new AgencyClientModel
            {
                ApiClientId = client.ApiClientId,
                ApplicationApplicationName = client.Application.ApplicationName,
                ApplicationClaimSetName = client.Application.ClaimSetName,
                ApplicationClaimsetDetailChoiceClaimset = client.Application.ClaimsetDetail.ChoiceClaimset,
                ApplicationClaimsetDetailClaimsetTypeId = client.Application.ClaimsetDetail.ClaimsetTypeId,
                ApplicationClaimsetDetailClaimsetTypeName = client.Application.ClaimsetDetail.ClaimsetType.Name,
                ApplicationClaimsetDetailPlainEnglish = client.Application.ClaimsetDetail.PlainEnglish,
                ApplicationClaimsetDetailPrimarySis = client.Application.ClaimsetDetail.PrimarySis,
                ApplicationClaimsetDetailPublicClaimset = client.Application.ClaimsetDetail.PublicClaimset,
                ApplicationClaimsetDetailReadOnlyClaimset = client.Application.ClaimsetDetail.ReadOnlyClaimset,
                ApplicationClaimsetDetailSchoolLevelClaimset = client.Application.ClaimsetDetail.SchoolLevelClaimset,
                ApplicationVendorVendorId = client.Application.Vendor.VendorId,
                ApplicationVendorVendorName = client.Application.Vendor.VendorName,
                Name = client.Name,
                Key = client.Key,
                SecretIsHashed = client.SecretIsHashed,
                Secret = client.Secret,
                ProfileNames = client.Application.Profiles.Select(p => p.ProfileName).ToList(),
                Organizations = client.ApplicationEducationOrganizations.Select(e => e.EdOrg).ToList(),

            };
            model.Action = "EditSubscription";
            model.VendorSubscriptionMessage = subscriptionSiteContent.Body;

            model.Schools = _odsBusiness.GetLeaSchools(EducationOrganizationId)
             .Select(t => new { t.EducationOrganizationId, t.NameOfInstitution }).ToList()
             .Select(t => new SelectListItem() { Value = t.EducationOrganizationId.ToString(), Text = t.NameOfInstitution, Selected = model.Organizations.Any(o => o.EducationOrganizationId == t.EducationOrganizationId) }).ToList()
             ;
            model.SelectedSchoolIds = model.Organizations.Select(o => o.EducationOrganizationId.ToString()).ToList();
            model.Applications = VendorApplications(model.ApplicationClaimsetDetailSchoolLevelClaimset, model.ApplicationVendorVendorId, model.ApplicationClaimSetName);

            return PartialView("EditSubscription", model);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult EditSubscription(AgencyClientModel model)
        {
            try
            {
                if (model != null && ModelState.IsValid)
                {
                    var apiClientId = model.ApiClientId;
                    int?[] selectedSchoolIds = null;
                    if (model.SelectedSchoolIds != null && model.SelectedSchoolIds.Count > 0)
                    {
                        selectedSchoolIds = model.SelectedSchoolIds.Select(x => (int?)int.Parse(x)).ToArray();
                    }

                    _vendorSubscriptionBusiness.Modify(WamsUser.WamsId, apiClientId,
                        model.ApplicationClaimSetName, selectedSchoolIds);
                    _agreementBusiness.UpsertVendorSub(
                        EducationOrganizationId,
                        WamsUser.WamsId,
                        model.ApplicationVendorVendorId);

                }
                return Json(new { success = true });
            }
            catch (Exception e)
            {
                 return Json(new
                {
                    success = false,
                    Errors = string.Format("Could not update subscription")
                });
            }


        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult DeleteSubscription(int id)
        {
            _vendorSubscriptionBusiness.Unsubscribe(WamsUser.WamsId, EducationOrganizationId, id);

            return Json(new { success = true });
        }

        public JsonResult GetApplicationClaimset(bool isSchoolLevel, int vendorId)
        {
            var result = VendorApplications(isSchoolLevel, vendorId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private List<SelectListItem> VendorApplications(bool isSchoolLevel, int vendorId, string claimset = null)
        {
            var org = _odsBusiness.GetEducationOrganization(EducationOrganizationId);
            bool isChoice = org.IsChoice;


            var applications = _applicationBusiness.GetApplicationsByVendor(vendorId);
            var claimSets = applications.Where(a =>
                a.ClaimsetDetail.SchoolLevelClaimset == isSchoolLevel && a.ClaimsetDetail.Active);
            if (isChoice)
            {
                claimSets = claimSets.Where(a => a.ClaimsetDetail.ChoiceClaimset);
            }
            else
            {
                claimSets = claimSets.Where(a => a.ClaimsetDetail.PublicClaimset);
            }

            var latestYears = claimSets.GroupBy(c => c.ClaimsetDetail.ClaimsetTypeId).Select(c => new { type = c.Key, maxyear = c.Max(g => g.ClaimsetDetail.RequirementYear) });

            var result = claimSets
                .Where(c => c.ClaimSetName == claimset || c.ClaimsetDetail.RequirementYear == null || latestYears.Any(y => y.type == c.ClaimsetDetail.ClaimsetTypeId && y.maxyear == c.ClaimsetDetail.RequirementYear))
                .GroupBy(g => new { g.ClaimsetDetail.ClaimSetName, g.ClaimsetDetail.PlainEnglish })
                .Select(t => new SelectListItem { Value = t.Key.ClaimSetName, Text = t.Key.PlainEnglish }).Distinct().ToList();

            return result;
        }
    }
}