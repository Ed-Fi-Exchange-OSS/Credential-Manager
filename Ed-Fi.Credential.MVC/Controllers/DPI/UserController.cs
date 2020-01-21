using Ed_Fi.Credential.Business;
using Ed_Fi.Credential.Domain.Model;
using Ed_Fi.Credential.MVC.Controllers.Base;
using Ed_Fi.Credential.MVC.ImplementationSpecific;
using Ed_Fi.Credential.MVC.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Ed_Fi.Credential.MVC.Controllers.DPI
{
    [RequirePrivilege(WisePrivilege.EditCredential)]
    public class UserController : BaseController
    {
        private readonly IVendorBusiness _vendorBusiness;
        private readonly IUserBusiness _userBusiness;

        public UserController(IBaseControllerContext controllerContext, IUserBusiness userBusiness, IVendorBusiness vendorBusiness)
            : base(controllerContext)
        {
            _vendorBusiness = vendorBusiness;
            _userBusiness = userBusiness;
        }

        public ActionResult Index()
        {
          return View();
        }

        public JsonResult Read()
        {
            var users = _userBusiness.GetUsers();
           var userModels = users.Select(u => new UserModel
           {
               UserId = u.UserId,
               FullName = u.FullName,
               WamsUserName = u.UserAsmUser?.WamsUser,
               VendorVendorId = u.VendorVendorId,
               Email = u.Email,
               VendorName = u.Vendor?.VendorName
           });

            return Json(userModels);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Edit(int userId)
        {
            UserModel model = new UserModel();
            if (userId > 0)
            {
                var user = _userBusiness.GetUser(userId);
                ;
                model.UserId = user.UserId;
                model.FullName = user.FullName;
                model.WamsUserName = user.UserAsmUser?.WamsUser;
                model.VendorVendorId = user.VendorVendorId;
                model.Email = user.Email;
                model.Title = "Edit User";
            }
            else
            {
                model.Title = "New User";
            }

            model.Vendors = _vendorBusiness.Read().Select(v => new SelectListItem
                {Text = v.VendorName, Value = v.VendorId.ToString()});
            return PartialView("_User", model);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(UserModel userModel)
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
                                Errors = "Could not find WAMS User ID"
                            });
                        }

                        if (_userBusiness.AsmUserInUse(userModel.WamsUserName, userModel.UserId))
                        {
                            return Json(new 
                            {
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
                    }
                    else
                    {
                        user = new User();
                        user.Email = userModel.Email;
                        user.FullName = userModel.FullName;
                        user.VendorVendorId = userModel.VendorVendorId;
                        if (userModel.WamsUserNameIsEmpty == false && newAsmUser != null)
                        {
                            user.UserAsmUser = new UserAsmUser { WamsId = newAsmUser.WamsId, WamsUser = newAsmUser.UserName };
                        }

                        _userBusiness.Create(WamsUser.WamsId, user,true);
                    }
                }
                catch (Exception)
                {
                    return Json(new 
                    {
                        Errors = $"Could not update user"
                    });
                }
            }
            else
            {
                var allErrors = ModelState.Values.SelectMany(v => v.Errors.Select(b => b.ErrorMessage));
                return Json(new 
                {
                    Errors = string.Join("<br />", allErrors)
                });
            }

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

         [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult Delete(int userId)
        {
                try
                {
                    var user = _userBusiness.GetUser(userId);

                    if (user != null)
                    {
                        if(user.UserAsmUser != null)
                            _userBusiness.Remove(user.UserAsmUser);
                        _userBusiness.Remove(user);
                        _userBusiness.SaveChanges(WamsUser.WamsId);
                    }
                }
                catch (Exception)
                {
                    return Json( new
                    {
                        Errors = $"Could not delete user"
                    });
                }
           
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
    }
}