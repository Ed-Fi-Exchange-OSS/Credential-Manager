using Ed_Fi.Credential.Business;
using Ed_Fi.Credential.Business.Common.Interfaces;
using Ed_Fi.Credential.Domain.Model;
using Ed_Fi.Credential.MVC.Controllers.Base;
using Ed_Fi.Credential.MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Ed_Fi.Credential.MVC.ImplementationSpecific;

namespace Ed_Fi.Credential.MVC.Controllers.DPI
{
    [RequirePrivilege(WisePrivilege.EditCredential)]
    public class VendorsController : BaseController
    {
        private readonly IClaimSetBusiness _claimSetBusiness;
        private readonly IVendorBusiness _vendorBusiness;
        private readonly IApplicationBusiness _applicationBusiness;
        private readonly IProfileBusiness _profileBusiness;
        private readonly IUserBusiness _userBusiness;
        private readonly IApiClientBusiness _apiClientBusiness;
        private readonly IOdsBusiness _odsBusiness;


        public VendorsController(IBaseControllerContext controllerContext, IVendorBusiness vendorBusiness, IClaimSetBusiness claimSetBusiness, IApplicationBusiness applicationBusiness, IProfileBusiness profileBusiness, IUserBusiness userBusiness, IApiClientBusiness apiClientBusiness, IOdsBusiness odsBusiness)
            : base(controllerContext)
        {
            _vendorBusiness = vendorBusiness;
            _claimSetBusiness = claimSetBusiness;
            _applicationBusiness = applicationBusiness;
            _profileBusiness = profileBusiness;
            _userBusiness = userBusiness;
            _apiClientBusiness = apiClientBusiness;
            _odsBusiness = odsBusiness;
        }

        public ActionResult Index()
        {
            return RedirectToAction("List");
        }
        
        private void ValidateSaveApiClient(ApiClientModel model)
        {
            var keyExist = _apiClientBusiness.DoesKeyExist(model.ApiClientId, model.Key);
            if (keyExist)
            {
                Error = string.Format("Api Client {0} could not be saved because the key '{1}' already exists.", model.Name, model.Key);
                return;
            }

            var nameExist = _apiClientBusiness.DoesNameExist(model.ApiClientId, model.Name);
            if (nameExist)
            {
                Error = string.Format("Api Client {0} could not be saved because the name '{0}' already exists.", model.Name);
            }
            if (model.SecretDisplay != Domain.SecretDisplayConstant.SecretDisplayHashed)
            {
                if (model.SecretDisplay.Trim().Length < 16)
                {
                    Error = string.Format("Api Client {0} could not be saved because the secret must be at least 16 characters.", model.Name);
                    return;
                }
                if (model.SecretDisplay.Trim() == model.Key)
                {
                    Error = string.Format("Api Client {0} could not be saved because the secret must not match the key.", model.Name);
                    return;
                }
                if (model.SecretDisplay.Trim() == model.Name)
                {
                    Error = string.Format("Api Client {0} could not be saved because the secret must not match the name.", model.Name);
                    return;
                }
            }


        }
        
        #region DataTables
        public ActionResult List()
        {
            var vendors = _vendorBusiness.GetVendorsWithUsers();
            var edit = (bool)ViewData[Constants.CAN_USER_EDIT_VENDOR_SUBSCRIPTION];
            var vendorModels = vendors.Select(vendor => new VendorAndUsersModel
            {
                VendorId = vendor.VendorId,
                VendorName = vendor.VendorName,
                 NamespacePrefixes = vendor.VendorNamespacePrefixes.Select(n => new NamespacePrefixModel { VendorNamespacePrefixId = n.VendorNamespacePrefixId, NamespacePrefix = n.NamespacePrefix }).ToList(),
                EmailDomains = vendor.VendorEmailDomains.Select(n => new EmailDomainModel { VendorEmailDomainId = n.VendorEmailDomainId, EmailDomain = n.EmailDomain }).ToList(),
                Users = vendor.Users.Select(u => new UserModel { Email = u.Email, FullName = u.FullName }).ToList(),
                CanEdit = edit
            }).ToList();

            return View(vendorModels);
        }

        public ActionResult Detail(int id)
        {
            var vendor = _vendorBusiness.GetFullVendorById(id);

            var vendorModel = new VendorAndUsersModel
            {
                VendorId = vendor.VendorId,
                VendorName = vendor.VendorName,
                NamespacePrefixes = vendor.VendorNamespacePrefixes.Select(n => new NamespacePrefixModel { VendorNamespacePrefixId = n.VendorNamespacePrefixId, NamespacePrefix = n.NamespacePrefix }).ToList(),
                EmailDomains = vendor.VendorEmailDomains.Select(n => new EmailDomainModel { VendorEmailDomainId = n.VendorEmailDomainId, EmailDomain = n.EmailDomain }).ToList(),
                Users = vendor.Users.Select(u => new UserModel { Email = u.Email, FullName = u.FullName, UserId = u.UserId }).ToList(),
                CanEdit = (bool)ViewData[Constants.CAN_USER_EDIT_VENDOR_SUBSCRIPTION]
            };

            return View(vendorModel);
        }
        
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult NewVendor(NewVendorModel vendorModel)
        {
            if (vendorModel != null && ModelState.IsValid)
            {
                try
                {
                    var vendor = new Vendor
                    {
                        VendorName = vendorModel.VendorName,
                        VendorNamespacePrefixes = new List<VendorNamespacePrefix> { new VendorNamespacePrefix { NamespacePrefix = vendorModel.NamespacePrefix ?? vendorModel.VendorName } },
                    };

                    if (!string.IsNullOrWhiteSpace(vendorModel.EmailDomain))
                    {
                        vendor.VendorEmailDomains.Add(new VendorEmailDomain { EmailDomain = vendorModel.EmailDomain });
                    }

                    _vendorBusiness.Create(WamsUser.WamsId, vendor);
                    _vendorBusiness.SaveChanges(WamsUser.WamsId);

                    var newid = _vendorBusiness.GetVendorByName(vendor.VendorName).VendorId;
                    return Json(new { success = true, newid = newid }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {
                    return Json(
                        new
                        {
                            success = false,
                            Errors = string.Format("Could not create {0}", vendorModel.VendorName)
                        });
                }
            }

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult EditVendor(int id, string vendorName, string namespacePrefix)
        {
            if (!string.IsNullOrWhiteSpace(vendorName))
            {

                var vendor = _vendorBusiness.GetVendorById(id);
                try
                {
                    vendor.VendorName = vendorName;
                     _vendorBusiness.SaveChanges(WamsUser.WamsId);
                }
                catch (Exception)
                {
                    return Json(new
                    {
                        success = false,
                        Errors = "Could not update vendor"
                    });
                }

            }
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult AddVendorPrefix(int id, string prefix)
        {
            if (!string.IsNullOrWhiteSpace(prefix))
            {

                var vendor = _vendorBusiness.GetVendorById(id);
                try
                {
                    vendor.VendorNamespacePrefixes.Add(new VendorNamespacePrefix { VendorVendorId = id, NamespacePrefix = prefix });
                    _vendorBusiness.SaveChanges(WamsUser.WamsId);

                    vendor = _vendorBusiness.GetVendorById(id);
                    var newid = vendor.VendorNamespacePrefixes.Where(p => p.NamespacePrefix == prefix).Select(p => p.VendorNamespacePrefixId).FirstOrDefault();
                    return Json(new { success = true, newid = newid }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {
                    return Json(new
                    {
                        success = false,
                        Errors = "Could not add prefix"
                    });
                }

            }
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult DeleteVendorPrefix(int id, int prefixId)
        {

            try
            {

                _vendorBusiness.DeletePrefix(prefixId);
                _vendorBusiness.SaveChanges(WamsUser.WamsId);
            }
            catch (Exception)
            {
                return Json(new
                {
                    success = false,
                    Errors = "Could not delete prefix"
                });
            }


            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult AddVendorDomain(int id, string domain)
        {
            if (!string.IsNullOrWhiteSpace(domain))
            {

                var vendor = _vendorBusiness.GetVendorById(id);
                try
                {
                    vendor.VendorEmailDomains.Add(new VendorEmailDomain { VendorVendorId = id, EmailDomain = domain });
                    _vendorBusiness.SaveChanges(WamsUser.WamsId);

                    vendor = _vendorBusiness.GetVendorById(id);
                    var newid = vendor.VendorEmailDomains.Where(p => p.EmailDomain == domain).Select(p => p.VendorEmailDomainId).FirstOrDefault();
                    return Json(new { success = true, newid = newid }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {
                    return Json(new
                    {
                        success = false,
                        Errors = "Could not add prefix"
                    });
                }

            }
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

         [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult DeleteVendorDomain(int id, int domainId)
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

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult User_Create(int vendorId)
        {

            var model = new UserModel
            {
                VendorVendorId = vendorId,
                Action = "User_Create",
                Title = "New User"
            };

            return PartialView("_User", model);
        }

         [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult User_Create(UserModel userModel)
        {
            if (userModel != null && ModelState.IsValid)
            {
                try
                {
                    AsmUser asmUser = null;

                    if (userModel.WamsUserNameIsEmpty == false)
                    {
                        asmUser = _userBusiness.GetAsmUser(userModel.WamsUserName);

                        if (asmUser == null)
                        {
                            return Json(new
                            {
                                success = false,
                                Errors = $"Could not find WAMS User ID '{userModel.WamsUserName}'"
                            });
                        }

                        if (_userBusiness.AsmUserInUse(userModel.WamsUserName))
                        {
                            return Json(new
                            {
                                success = false,
                                Errors = $"WAMS User ID '{userModel.WamsUserName}' is already in use"
                            });
                        }
                    }


                    var user = new User
                    {
                        Email = userModel.Email,
                        FullName = userModel.FullName,
                        VendorVendorId = userModel.VendorVendorId,
                        UserAsmUser = userModel.WamsUserNameIsEmpty ? null : new UserAsmUser { WamsId = asmUser.WamsId, WamsUser = asmUser.UserName }
                    };

                    _userBusiness.Create(user);

                    _userBusiness.SaveChanges(WamsUser.WamsId);
                    var newid = _userBusiness.GetUsers().First(u => u.FullName == userModel.FullName && u.Email == userModel.Email).UserId;
                    return Json(new
                    {
                        success = true,
                        Email = userModel.Email,
                        FullName = userModel.FullName,
                        newid = newid
                    }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {
                    return Json(new
                    {
                        success = false,
                        Errors = string.Format("Could not create user {0}", userModel.FullName)
                    });
                }
            }

            return Json(new { success = false, Errors = "Could not create user" }, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult User_Update(int userId)
        {
            var user = _userBusiness.GetUser(userId);

            var model = new UserModel
            {
                UserId = user.UserId,
                FullName = user.FullName,
                WamsUserName = user.UserAsmUser?.WamsUser,
                VendorVendorId = user.VendorVendorId,
                Email = user.Email,
                Action = "User_Update",
                Title = "Edit User"
            };


            return PartialView("_User", model);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult User_Update(UserModel userModel)
        {
            if (ModelState.IsValid && userModel != null)
            {
                try
                {
                    AsmUser newAsmUser = null;

                    if (userModel.WamsUserNameIsEmpty == false)
                    {
                        newAsmUser = _userBusiness.GetAsmUser(userModel.WamsUserName);

                        if (newAsmUser == null)
                        {
                            return Json(new
                            {
                                success = false,
                                Errors = "Could not find WAMS User ID"
                            });
                        }

                        if (_userBusiness.AsmUserInUse(userModel.WamsUserName, userModel.UserId))
                        {
                            return Json(new
                            {
                                success = false,
                                Errors = $"WAMS User ID '{userModel.WamsUserName}' is already in use"
                            });
                        }
                    }

                    var user = _userBusiness.GetUser(userModel.UserId);

                    if (user != null)
                    {
                        user.Email = userModel.Email;
                        user.FullName = userModel.FullName;
                        user.VendorVendorId = userModel.VendorVendorId;

                        if (userModel.WamsUserNameIsEmpty)
                        {
                            if (user.UserAsmUser != null)
                            {
                                _userBusiness.Remove(user.UserAsmUser);
                            }
                        }
                        else
                        {
                            if (user.UserAsmUser == null)
                            {
                                if (newAsmUser != null)
                                    user.UserAsmUser = new UserAsmUser { WamsId = newAsmUser.WamsId, WamsUser = newAsmUser.UserName };
                            }
                            else
                            {
                                if (newAsmUser != null)
                                {
                                    user.UserAsmUser.WamsId = newAsmUser.WamsId;
                                    user.UserAsmUser.WamsUser = newAsmUser.UserName;
                                }
                            }
                        }

                        _userBusiness.SaveChanges(WamsUser.WamsId);
                        return Json(new
                        {
                            Email = userModel.Email,
                            FullName = userModel.FullName,
                            UserId = userModel.UserId,
                            success = true
                        }, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception)
                {
                    return Json(new
                    {
                        success = false,
                        Errors = $"Could not update user {userModel.FullName}"
                    });
                }
            }

            return Json(new
            {
                success = false,
                Errors = "Could not update user"
            }, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult User_Delete(int userId)
        {
            try
            {
                var user = _userBusiness.GetUser(userId);

                if (user != null)
                {

                    _userBusiness.Destroy(WamsUser.WamsId, user);
                    _userBusiness.SaveChanges(WamsUser.WamsId);
                }
                return Json(new { success = true });
            }
            catch (Exception)
            {
                return Json(new
                {
                    success = false,
                    Errors = string.Format("Could not Delete User")
                });
            }


        }

        //Application
        public JsonResult Application_Read2(int vendorId)
        {
            var applications = _applicationBusiness.GetApplicationsByVendor(vendorId);

            var applicationsModels = applications.Select(app => new ApplicationModel
            {
                ApplicationId = app.ApplicationId,
                ApplicationName = app.ApplicationName,
                VendorVendorId = app.VendorVendorId,
                ClaimSetName = app.ClaimSetName,
                ProfileId = app.Profiles.Select(p => p.ProfileId).FirstOrDefault(),
                ProfileName = app.Profiles.Select(p => p.ProfileName).FirstOrDefault(),
                PlainEnglish = app.ClaimsetDetail == null ? "No details" : app.ClaimsetDetail.Active ? app.ClaimsetDetail.PlainEnglish : string.Format("(Deprecated) {0}", app.ClaimsetDetail.PlainEnglish),
                ClaimSetType = app.ClaimsetDetail == null ? "No details" : app.ClaimsetDetail.ClaimsetType.Name
            }).ToList();

            return Json(applicationsModels, JsonRequestBehavior.AllowGet);
        }

         [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Application_Create2(int vendorId)
        {
            var claimSets = _claimSetBusiness.GetClaimSetsWithDetails()
                .Select(c => new SelectListItem { Text = c.ClaimSetName, Value = c.ClaimSetName }).ToList();
            claimSets.Insert(0, new SelectListItem { Text = "", Value = "" });

            ViewData[Constants.CLAIMSET_MODELS] = claimSets;

            var profiles = _profileBusiness.Get()
                .Select(p => new SelectListItem { Text = p.ProfileName, Value = p.ProfileId.ToString() }).ToList();
            profiles.Insert(0, new SelectListItem { Text = "", Value = "" });
            ViewData[Constants.PROFILE_MODELS] = profiles;

          var model = new ApplicationModel
            {
                VendorVendorId = vendorId,
                Action = "Application_Create2",
                Title = "New Application"
            };

            return PartialView("_Application", model);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult Application_Create2(ApplicationModel applicationModel)
        {
            if (applicationModel != null && ModelState.IsValid)
            {
                try
                {
                    var application = new Application
                    {
                        ApplicationName = applicationModel.ApplicationName,
                        VendorVendorId = applicationModel.VendorVendorId,
                        ClaimSetName = applicationModel.ClaimSetName
                    };
                    int profile = applicationModel.ProfileId.GetValueOrDefault();
                    if (profile > 0)
                    {
                        application.Profiles.Add(_profileBusiness.GetById(profile));
                    }
                    _applicationBusiness.Create(WamsUser.WamsId, application);
                    _applicationBusiness.SaveChanges(WamsUser.WamsId);
                }
                catch (Exception)
                {
                    return Json(new
                    {
                        success = false,
                        Errors = string.Format("Could not create application {0}", applicationModel.ApplicationName)
                    });
                }
            }

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Application_Update2(int applicationId)
        {
            var claimSets = _claimSetBusiness.GetClaimSetsWithDetails()
                .Select(c => new SelectListItem { Text = c.ClaimSetName, Value = c.ClaimSetName }).ToList();
            claimSets.Insert(0, new SelectListItem { Text = "", Value = "" });

            ViewData[Constants.CLAIMSET_MODELS] = claimSets;

            var profiles = _profileBusiness.Get()
                .Select(p => new SelectListItem { Text = p.ProfileName, Value = p.ProfileId.ToString() }).ToList();
            profiles.Insert(0, new SelectListItem { Text = "", Value = "" });
            ViewData[Constants.PROFILE_MODELS] = profiles;

            var app = _applicationBusiness.GetApplicationById(applicationId);

            var model = new ApplicationModel
            {
                ApplicationId = app.ApplicationId,
                ApplicationName = app.ApplicationName,
                VendorVendorId = app.VendorVendorId,
                ClaimSetName = app.ClaimSetName,
                ProfileId = app.Profiles.Select(p => p.ProfileId).FirstOrDefault(),
                Action = "Application_Update2",
                Title = "Edit Application"
            };


            return PartialView("_Application", model);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult Application_Update2(ApplicationModel applicationModel)
        {
            if (applicationModel != null && ModelState.IsValid)
            {
                try
                {
                    var app = _applicationBusiness.GetApplicationById(applicationModel.ApplicationId);


                    app.ApplicationName = applicationModel.ApplicationName;
                    app.ClaimSetName = applicationModel.ClaimSetName;

                   
                    int profileId = applicationModel.ProfileId.GetValueOrDefault();
                    if (profileId > 0)
                    {
                        if (!app.Profiles.Any())
                        {
                            app.Profiles.Add(_profileBusiness.GetById(profileId));
                        }
                        else if (app.Profiles.Any(p => p.ProfileId != profileId))
                        {
                            app.Profiles.Clear();
                            app.Profiles.Add(_profileBusiness.GetById(profileId));
                        }
                    }
                    else
                    {
                        app.Profiles.Clear();
                    }
                    _applicationBusiness.SaveChanges(WamsUser.WamsId);
                }
                catch (Exception)
                {
                    return Json(new
                    {
                        success = false,
                        Errors = string.Format("Could not create application {0}", applicationModel.ApplicationName)
                    });
                }
            }

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult Application_Delete2(int applicationId)
        {
            try
            {
                var application = _applicationBusiness.GetApplicationById(applicationId);

                if (application != null)
                {

                    _applicationBusiness.Destroy(WamsUser.WamsId, application);
                    _applicationBusiness.SaveChanges(WamsUser.WamsId);
                }
                return Json(new { success = true });
            }
            catch (Exception)
            {
                return Json(new
                {
                    success = false,
                    Errors = string.Format("Could not Delete Application")
                });
            }


        }

        //ApiClient
        public JsonResult ApiClient_Read2(int applicationId)
        {

            var apiClients = _apiClientBusiness.GetApiClientsByApplication(applicationId);

            var apiClientModels = apiClients.Select(c => new ApiClientModel
            {
                ApiClientId = c.ApiClientId,
                Name = c.Name,
                Key = c.Key,
                Secret = c.Secret,
                SecretIsHashed = c.SecretIsHashed,
                ApplicationApplicationId = c.ApplicationApplicationId,
                SecretDisplay = c.SecretDisplay
            });

            return Json(apiClientModels, JsonRequestBehavior.AllowGet);

        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ApiClient_Create2(int applicationId)
        {
            var model = new ApiClientModel
            {
                ApplicationApplicationId = applicationId,
                Secret = Guid.NewGuid().ToString()
            };
            model.SecretDisplay = model.Secret;
            return PartialView("_ApiClient", model);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult ApiClient_Create2(ApiClientModel model)
        {
            if (model != null && ModelState.IsValid)
            {
                try
                {
                    ValidateSaveApiClient(model);

                    if (!string.IsNullOrWhiteSpace(model.Errors))
                    {
                        return Json(new
                        {
                            success = false,
                            Errors = string.Format("Could not create api client {0} : {1} ", model.Name, model.Errors)
                        });
                    }

                    var client = new ApiClient
                    {
                        Key = model.Key,
                        Secret = model.SecretDisplay,
                        Name = model.Name,
                        ApplicationApplicationId = model.ApplicationApplicationId
                    };
                    _apiClientBusiness.Create(WamsUser.WamsId, client);
                    _apiClientBusiness.SaveChanges(WamsUser.WamsId);

                    model.ApiClientId = client.ApiClientId;
                }
                catch (Exception)
                {
                    return Json(new
                    {
                        success = false,
                        Errors = string.Format("Could not create api client {0} ", model.Name)
                    });
                }

            }
            else
            {
                if (model != null)
                {

                    var errorList = (from item in ModelState.Values
                                     from error in item.Errors
                                     select error.ErrorMessage).ToList();
                    model.Errors = string.Join("<br /> ", errorList);
                    return Json(new
                    {
                        success = false,
                        Errors = string.Format("Could not create api client {0} : {1}", model.Name, model.Errors)
                    });
                }
            }

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ApiClient_Update2(int apiClientId)
        {
            var client = _apiClientBusiness.GetApiClient(apiClientId);


            var apiClientModel = new ApiClientModel
            {
                ApiClientId = client.ApiClientId,
                Name = client.Name,
                Key = client.Key,
                Secret = client.Secret,
                SecretIsHashed = client.SecretIsHashed,
                ApplicationApplicationId = client.ApplicationApplicationId,
                SecretDisplay = client.SecretDisplay
            };

            return PartialView("_ApiClient", apiClientModel);

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult ApiClient_Update2(ApiClientModel model)
        {
            if (model != null && ModelState.IsValid)
            {
                try
                {

                    ValidateSaveApiClient(model);
                    if (HasErrors)
                    {
                        return Json(new
                        {
                            success = false,
                            Errors = string.Format("Could not update Client {0} : {1}", model.Name, model.Errors)
                        });
                    }

                    var client = _apiClientBusiness.GetApiClient(model.ApiClientId);
                    client.Key = model.Key;
                    if (model.SecretDisplay != Domain.SecretDisplayConstant.SecretDisplayHashed)
                    {
                        client.Secret = model.SecretDisplay;
                        client.SecretIsHashed = false;
                    }
                    client.Name = model.Name;
                    client.IsApproved = model.IsApproved;
                    client.UseSandbox = model.UseSandbox;
                    client.SandboxType = model.SandboxType;
                    client.UserUserId = model.UserUserId;
                    if (client.ApplicationApplicationId != model.ApplicationApplicationId)
                    {
                        var application =
                            _applicationBusiness.GetApplicationById(model.ApplicationApplicationId.GetValueOrDefault());
                        client.ApplicationApplicationId = model.ApplicationApplicationId;
                        foreach (var clientApplicationEducationOrganization in client.ApplicationEducationOrganizations.Where(e => e.Application.VendorVendorId == application.VendorVendorId))
                        {
                            clientApplicationEducationOrganization.ApplicationApplicationId =
                                model.ApplicationApplicationId;
                        }
                    }
                    _apiClientBusiness.Update(WamsUser.WamsId, client);
                    _apiClientBusiness.SaveChanges(WamsUser.WamsId);
                }
                catch (Exception)
                {
                    return Json(new
                    {
                        success = false,
                        Errors = string.Format("Could not update Client {0}", model.Name)
                    });



                }

            }

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult MoveClient(int apiClientId)
        {
            var client = _apiClientBusiness.GetApiClient(apiClientId);
            var apiClientModel = new MoveApiClientModel()
            {
                ApiClientId = client.ApiClientId,
                Name = client.Name,
                ApplicationApplicationId = client.ApplicationApplicationId,
                OldApplicationApplicationId = client.ApplicationApplicationId
            };
            var applications =
                _applicationBusiness.GetApplicationsByVendor(client.Application.VendorVendorId.GetValueOrDefault());
            var appList = applications.Select(a => new SelectListItem { Text = $"{a.ApplicationName}-{a.ClaimSetName}", Value = a.ApplicationId.ToString(), Selected = a.ApplicationId == apiClientModel.ApplicationApplicationId }).ToList();

            apiClientModel.Applications = appList;
            return PartialView("_MoveClient", apiClientModel);

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult MoveClient(MoveApiClientModel model)
        {
            if (model != null && ModelState.IsValid)
            {
                try
                {
                    var client = _apiClientBusiness.GetApiClient(model.ApiClientId);
                    if (client.ApplicationApplicationId != model.ApplicationApplicationId)
                    {
                        var application =
                            _applicationBusiness.GetApplicationById(model.ApplicationApplicationId.GetValueOrDefault());
                        client.ApplicationApplicationId = model.ApplicationApplicationId;
                        foreach (var clientApplicationEducationOrganization in client.ApplicationEducationOrganizations.Where(e => e.Application.VendorVendorId == application.VendorVendorId))
                        {
                            clientApplicationEducationOrganization.ApplicationApplicationId =
                                model.ApplicationApplicationId;
                        }
                    }
                    _apiClientBusiness.Update(WamsUser.WamsId, client);
                    _apiClientBusiness.SaveChanges(WamsUser.WamsId);

                    return Json(new
                    {
                        success = true,
                        applicationId = model.ApplicationApplicationId.GetValueOrDefault(),
                        oldApplicationId = model.OldApplicationApplicationId.GetValueOrDefault(),
                    }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {
                    return Json(new
                    {
                        success = false,

                        errors = "Could not move client"
                    });
                }

            }
            return Json(new
            {
                success = false,

                errors = "Could not move client"
            });
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult ApiClient_Delete2(int apiClientId)
        {
            try
            {
                _apiClientBusiness.TryDestroy(WamsUser.WamsId, apiClientId);
                return Json(new { success = true });
            }
            catch (Exception)
            {
                return Json(new
                {
                    success = false,
                    Errors = string.Format("Could not Delete Client")
                });
            }


        }

        //EdOrg
        public JsonResult EdOrg_Read(int apiClientId)
        {
            var LeaModels = new List<ApiEdOrgModel>();

            var apiClient = _apiClientBusiness.GetApiClient(apiClientId);

            foreach (var agency in apiClient.ApplicationEducationOrganizations)
            {
                var edOrg = _odsBusiness.GetEducationOrganization(agency.EducationOrganizationId);

                if (edOrg == null)
                {
                    var school = _odsBusiness.GetSchools(new List<int> { agency.EducationOrganizationId }).FirstOrDefault();
                    //school may no longer be valid
                    if (school != null)
                    {
                        var lea = _odsBusiness.GetEducationOrganization(school.LocalEducationAgencyId);
                        LeaModels.Add(new ApiEdOrgModel
                        {
                            ApiClientId = apiClientId,
                            EducationOrganizationId = agency.EducationOrganizationId,
                            NameOfInstitution = school.NameOfInstitution,
                            IsSchool = true,
                            City = school.City,
                            Lea = string.Format("{0} - {1}", school.LocalEducationAgencyId, lea.NameOfInstitution)
                        });


                    }
                }
                else
                {
                    LeaModels.Add(new ApiEdOrgModel
                    {
                        ApiClientId = apiClientId,
                        EducationOrganizationId = agency.EducationOrganizationId,
                        NameOfInstitution = edOrg.NameOfInstitution,
                        IsSchool = false,
                        City = edOrg.City,
                        Lea = string.Empty
                    });

                }
            }
            return Json(LeaModels, JsonRequestBehavior.AllowGet);

        }

        [RequirePrivilege(WisePrivilege.EditCredential)]
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult EdOrg_Create(int apiClientId)
        {
            var model = new ApiEdOrgModel
            {
                ApiClientId = apiClientId
            };

            return PartialView("_EducationOrganization", model);

        }

        [RequirePrivilege(WisePrivilege.EditCredential)]
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult EdOrg_Create(ApiEdOrgModel model)
        {
            try
            {
                _apiClientBusiness.AddEducationOrganiztion(WamsUser.WamsId, model.EducationOrganizationId, model.ApiClientId);
                return Json(new { success = true });
            }
            catch (Exception)
            {
                return Json(new
                {
                    success = false,
                    Errors = string.Format("Could not Add Agency")
                });
            }


        }

        [RequirePrivilege(WisePrivilege.EditCredential)]
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult EdOrg_Delete(int edOrgId, int apiClientId)
        {
            try
            {
                _apiClientBusiness.RemoveEducationOrganiztion(WamsUser.WamsId, edOrgId, apiClientId);
                return Json(new { success = true });
            }
            catch (Exception)
            {
                return Json(new
                {
                    success = false,
                    Errors = string.Format("Could not Delete Org")
                });
            }


        }

        #endregion
        

        public JsonResult GetUsers(int vendorId)
        {
            var users = _userBusiness.GetUsers(vendorId);
            var userModels = users.Select(u=>new UserModel
            {
                Email = u.Email, FullName = u.FullName, UserId = u.UserId, VendorName = u.Vendor?.VendorName,
                VendorVendorId = u.VendorVendorId, WamsUserName = u.UserAsmUser?.WamsUser
            }).OrderBy(a => a.FullName);
            return Json(userModels, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetNewGuid()
        {
            return Json(Guid.NewGuid(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetApplications(int vendorId)
        {

            var applications = _applicationBusiness.GetApplicationsByVendor(vendorId);

            var applicationsModels = applications.Select(app => new ApplicationModel
            {
                ApplicationId = app.ApplicationId,
                ApplicationName = app.ApplicationName
            }).ToList();
            return Json(applicationsModels, JsonRequestBehavior.AllowGet);
        }
    }
}