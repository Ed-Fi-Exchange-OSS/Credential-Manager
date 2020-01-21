using Ed_Fi.Credential.Business;
using Ed_Fi.Credential.Domain.Enums;
using Ed_Fi.Credential.Domain.Model;
using Ed_Fi.Credential.MVC.Areas.Vendor.Models;
using Ed_Fi.Credential.MVC.Controllers.Base;
using Ed_Fi.Credential.MVC.ImplementationSpecific;
using Ed_Fi.Credential.MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Ed_Fi.Credential.MVC.Areas.Vendor.Controllers
{
    [RequirePrivilege(WisePrivilege.ViewMonitoring)]
    public class ProfileController : BaseController
    {
        private readonly IApiClientBusiness _apiClientBusiness;
        private readonly IUserBusiness _userBusiness;
        private readonly IVendorBusiness _vendorBusiness;
        private readonly IAgreementBusiness _agreementBusiness;
        private readonly ISiteContentBusiness _siteContentBusiness;
        private readonly IClaimSetBusiness _claimSetBusiness;

        public ProfileController(IBaseControllerContext controllerContext, IApiClientBusiness apiClientBusiness, IUserBusiness userBusiness, IVendorBusiness vendorBusiness, IAgreementBusiness agreementBusiness, ISiteContentBusiness siteContentBusiness, IClaimSetBusiness claimSetBusiness)
            : base(controllerContext)
        {
            _apiClientBusiness = apiClientBusiness;
            _userBusiness = userBusiness;
            _vendorBusiness = vendorBusiness;
            _agreementBusiness = agreementBusiness;
            _siteContentBusiness = siteContentBusiness;
            _claimSetBusiness = claimSetBusiness;
        }

        public ActionResult Index()
        {
            var user = _userBusiness.GetUserByWamsId(WamsUser.WamsId);
            if (user == null)//no user found with this id
            {
                user = _userBusiness.GetNonAsmUserByEmail(WamsUser.Email);
                if (user != null)//user without wamsid found with matching email
                {
                    var asmUser = _userBusiness.GetAsmUserByWamsId(WamsUser.WamsId);
                    if (asmUser != null)
                    {
                        user.UserAsmUser = new UserAsmUser { WamsId = asmUser.WamsId, WamsUser = asmUser.UserName };
                        _userBusiness.SaveChanges(WamsUser.WamsId);
                    }

                }
                else
                {
                    var vendor =
                        _vendorBusiness.GetVendorIdByDomain(WamsUser.Email.Substring(WamsUser.Email.IndexOf("@")+1));
                    if (vendor !=null)
                    {
                        user = new User { Email = WamsUser.Email, FullName = WamsUser.FullName, VendorVendorId = vendor.VendorId };
                        var asmUser = _userBusiness.GetAsmUserByWamsId(WamsUser.WamsId);
                        if (asmUser != null)
                        {
                            user.UserAsmUser = new UserAsmUser { WamsId = asmUser.WamsId, WamsUser = asmUser.UserName };
                        }
                        _userBusiness.Create(user);
                        _userBusiness.SaveChanges(WamsUser.WamsId);
                        user.Vendor = vendor;
                    }
                    else
                    {
                        return View("UserAccountError");
                    }

                }
            }

            if (user.VendorVendorId == null)
            {
                return View("UserAccountError");
            }

            var nonTestApiClients = _apiClientBusiness.GetApiClients(WamsUser.WamsId);
            var hasNonTestApiClients = nonTestApiClients.Any();

            var agreement = _agreementBusiness.FindByWamsIdNotVendorSub(WamsUser.WamsId, AgreementType.Vendor);
            var agree = (agreement != null && agreement.Agree);
          
            var currentUser = 
            new UserModel
            {
                UserId = user.UserId,
                FullName = user.FullName,
                WamsUserName = user.UserAsmUser?.WamsUser,
                VendorVendorId = user.VendorVendorId,
                Email = user.Email,
                VendorName = user.Vendor?.VendorName
            };
            var users = _userBusiness.GetUsers(user.VendorVendorId.GetValueOrDefault());
            var userModels = users.Select(u => new UserModel
            {
                UserId = u.UserId,
                FullName = u.FullName,
                WamsUserName = u.UserAsmUser?.WamsUser,
                VendorVendorId = u.VendorVendorId,
                Email = u.Email,
                VendorName = u.Vendor?.VendorName
            }).ToList();

            var message = _siteContentBusiness.GetByContentTypeId((int)SiteContentType.VendorAgreement);
            var model = new VendorProfileModel
            {
                VendorName = user.Vendor.VendorName,
                HasAgreed = agree,
                HasNonTestApiClients = hasNonTestApiClients,
                AgreementMessage = message.Body,
                Users = userModels,
                CurrentUser = currentUser
            };

            return View(model);
        }

        [RequirePrivilege(WisePrivilege.ViewCredentials)]
        public ActionResult Credentials()
        {
            var models = new List<VendorApiClientModel>();
            var currentUser = _userBusiness.GetUserByWamsId(WamsUser.WamsId);

            if (currentUser.VendorVendorId != null)
            {
                var clients = _apiClientBusiness.GetApiClientsByVendor(currentUser.VendorVendorId.GetValueOrDefault());
            
                models = clients.Select(c => new VendorApiClientModel
                {
                    ApiClientId = c.ApiClientId, ApplicationApplicationName = c.Application.ApplicationName,
                    ApplicationClaimSetName = c.Application.ClaimSetName,
                    Key = c.Key, Name = c.Name,
                    Organizations = c.ApplicationEducationOrganizations?.Select(e => e.EdOrg).ToList(),
                    ProfileNames = c.Application.Profiles?.Select(p => p.ProfileName).ToList(),
                    Secret = c.Secret, SecretIsHashed = c.SecretIsHashed
                }).ToList();
            }

            return View(models);
        }

     
      
        [HttpPost]
        public JsonResult Agreement(bool agree)
        {
            _agreementBusiness.UpsertNotVendorSub(
                EducationOrganizationId,
                WamsUser.WamsId,
                agree,
                AgreementType.Vendor,
                WamsUser.WamsId);

            return Json(null);
        }

        [RequirePrivilege(WisePrivilege.ViewCredentials)]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UpdateUser(UserModel model)
        {
            if (model != null && ModelState.IsValid)
            {
                try
                {
                    var user = _userBusiness.GetUser(model.UserId);
                    user.FullName = model.FullName;
                    user.Email = model.Email;
                    _userBusiness.Update(WamsUser.WamsId, user);
                    _userBusiness.SaveChanges(WamsUser.WamsId);
                }
                catch (Exception)
                {
                    return Json(new
                    {
                        success = false,
                        Errors = string.Format("Could not update user")
                    });
                }
            }

            return Json(new { success = true });
        }

        [RequirePrivilege(WisePrivilege.ViewCredentials)]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ResetSecret(int id)
        {
            var client = _apiClientBusiness.GetApiClient(id);
            var model = new ChangeSecretModel { ApiClientId = client.ApiClientId, Key = client.Key, Secret = Guid.NewGuid().ToString() };

            return PartialView("_ResetSecret", model);
        }

        [RequirePrivilege(WisePrivilege.ViewCredentials)]
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

        public JsonResult AccessRead(string claimsetName)
        {
            var actions = _claimSetBusiness.GetAllowedActionsByClaimset(claimsetName).OrderBy(a => a.ResourceName).ThenBy(a => a.ActionName);

            var grouped = actions.GroupBy(g => g.ResourceName).Select(t => new AllowedActionsByClaimset
            {
                ClaimSetName = claimsetName,
                ResourceName = t.Key,
                ActionName = string.Join(",", t.Select(v => v.ActionName))
            });

            return Json(grouped);
        }

        public JsonResult EmailDomains()
        {
            var models = new List<EmailDomainModel>();
            var currentUser = _userBusiness.GetUserByWamsId(WamsUser.WamsId);

            if (currentUser.VendorVendorId != null)
            {
                models = _vendorBusiness.GetDomains(currentUser.VendorVendorId.GetValueOrDefault())
                    .Select(m => new EmailDomainModel { EmailDomain = m.EmailDomain, VendorEmailDomainId = m.VendorEmailDomainId }).ToList();

            }

            return Json(models);

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult AddVendorDomain(string domain)
        {
            if (!string.IsNullOrWhiteSpace(domain))
            {
                if (!_vendorBusiness.EmailDomainExists(domain))
                {


                    var currentUser = _userBusiness.GetUserByWamsId(WamsUser.WamsId);
                    var vendor = _vendorBusiness.GetVendorById(currentUser.VendorVendorId.GetValueOrDefault());
                    try
                    {
                        vendor.VendorEmailDomains.Add(new VendorEmailDomain { VendorVendorId = currentUser.VendorVendorId.GetValueOrDefault(), EmailDomain = domain });
                        _vendorBusiness.SaveChanges(WamsUser.WamsId);

                        return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                    }
                    catch (Exception)
                    {
                        return Json(new
                        {
                            success = false,
                            Errors = "Could not add domain"
                        });
                    }

                }
                return Json(new
                {
                    success = false,
                    Errors = "Domain already in use"
                });
            }
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult DeleteVendorDomain(int domainId)
        {

            try
            {

                _vendorBusiness.DeleteDomain(domainId);
                _vendorBusiness.SaveChanges(WamsUser.WamsId);

            }
            catch (Exception)
            {
                return Json(new
                {
                    success = false,
                    Errors = "Could not delete domain"
                });
            }


            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
    }
}