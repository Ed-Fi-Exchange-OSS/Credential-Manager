using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Ed_Fi.Credential.MVC.ImplementationSpecific
{
    public class RequirePrivilege : FilterAttribute, IAuthorizationFilter
    {
        private readonly List<string> _privileges;

        public RequirePrivilege(params WisePrivilege[] privileges)
        {
            _privileges = privileges.Select(x => x.ToString()).ToList();
        }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            var user = filterContext.HttpContext.User as WamsPrincipal;
            if (user == null || user.Identity.IsAuthenticated == false)
            {
                filterContext.Result = new RedirectResult(ConfigurationManager.AppSettings["WAMS.LoginUrl"]);
                return;
            }

            var sessionInfo = (ISessionInfo)DependencyResolver.Current.GetService(typeof(ISessionInfo));

            var missingPrivileges = _privileges.Where(privilege => user.HasPrivilege(sessionInfo.CurrentAgencyId.GetValueOrDefault(), privilege) == false);
            if (missingPrivileges.Any())
            {
                throw new HttpException((int)HttpStatusCode.Forbidden, string.Format("User is missing required privileges: {0}", string.Join(", ", missingPrivileges)));
            }

        }
    }

    public class WiseAnyAuthorize : FilterAttribute, IAuthorizationFilter
    {
        private readonly List<string> _privileges;

        public WiseAnyAuthorize(params WisePrivilege[] privileges)
        {
            _privileges = privileges.Select(x => x.ToString()).ToList();
        }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            var user = filterContext.HttpContext.User as WamsPrincipal;
            if (user == null || user.Identity.IsAuthenticated == false)
            {
                filterContext.Result = new RedirectResult(ConfigurationManager.AppSettings["WAMS.LoginUrl"]);
                return;
            }

            var sessionInfo = (ISessionInfo)DependencyResolver.Current.GetService(typeof(ISessionInfo));

            var matchingPrivileges = _privileges.Where(privilege => user.HasPrivilege(sessionInfo.CurrentAgencyId.GetValueOrDefault(), privilege)).ToList();
            if (!matchingPrivileges.Any())
            {
                throw new HttpException((int)HttpStatusCode.Forbidden, string.Format("User is missing required privileges: {0}", string.Join(", ", _privileges)));
            }

        }
    }
}