using Ed_Fi.Credential.MVC.Controllers.Base;
using System;
using System.Configuration;
using System.Web.Mvc;

namespace Ed_Fi.Credential.MVC.Controllers
{
    public class HomeController : BaseController
    {

        public HomeController(IBaseControllerContext controllerContext)
            : base(controllerContext)
        {
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LogOut()
        {
            // TODO: add ability to clear user info cookie to DPI.Security
            var userInfoCookie = Response.Cookies["dpiUserInfo"];
            if (userInfoCookie != null)
            {
                userInfoCookie.Value = null;
                userInfoCookie.Expires = DateTime.Now.AddDays(-1);
            }

            return Redirect(ConfigurationManager.AppSettings["WAMS.LogoutUrl"]);
        }

        public ActionResult KeepAlive()
        {
            return Json("OK", JsonRequestBehavior.AllowGet);
        }

        public ActionResult SessionTimeout()
        {
            Session.Abandon();
            return View();
        }
    }
}