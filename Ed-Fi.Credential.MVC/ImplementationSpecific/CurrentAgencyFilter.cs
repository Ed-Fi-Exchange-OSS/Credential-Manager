using Ed_Fi.Credential.Business.Common.Interfaces;
using Microsoft.Ajax.Utilities;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using System.Web.Routing;

namespace Ed_Fi.Credential.MVC.ImplementationSpecific
{

    public class CurrentAgencyFilter : IAuthenticationFilter
    {
        private readonly ISessionInfo _sessionInfo;
        private readonly IOdsBusiness _odsBusiness;

        public CurrentAgencyFilter(ISessionInfo sessionInfo, IOdsBusiness odsBusiness)
        {
            _sessionInfo = sessionInfo;
            _odsBusiness = odsBusiness;
        }

        public void OnAuthentication(AuthenticationContext filterContext)
        {
            var wamsUser = (WamsPrincipal)filterContext.RequestContext.HttpContext.User;
            var agencies = wamsUser.Roles
                .Select(x => x.EducationOrganizationId)
                .Distinct()
                .ToList();

            if (_sessionInfo.CurrentAgencyId == null)
            {
                if (agencies.Count() == 1)
                {
                    var edOrgId = wamsUser.Roles.First().EducationOrganizationId;
                    var agency = _odsBusiness.GetEducationOrganization(Convert.ToInt32(edOrgId));

                    _sessionInfo.SetCurrentAgency(edOrgId);
                    filterContext.Controller.ViewBag.DefaultAgency = agency.NameOfInstitution;
                }
                else
                {
                    var area = (string)filterContext.RouteData.DataTokens["area"];
                    var controller = (string)filterContext.RouteData.Values["controller"];
                    var action = (string)filterContext.RouteData.Values["action"];
                    if (controller.Equals("Debug", StringComparison.InvariantCultureIgnoreCase) ||
                        (
                            area.IsNullOrWhiteSpace()
                            && controller.Equals("WamsUser", StringComparison.InvariantCultureIgnoreCase)
                            && (action.Equals("Index", StringComparison.InvariantCultureIgnoreCase) || action.Equals("ChangeAgency", StringComparison.InvariantCultureIgnoreCase)))
                        )
                        return;

                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary
                    {
                        {"area", ""},
                        {"controller", "WamsUser"},
                        {"action", "Index"}
                    });
                }
            }
            else
            {
                var agency = _odsBusiness.GetEducationOrganization(_sessionInfo.CurrentAgencyId.GetValueOrDefault());
                filterContext.Controller.ViewBag.DefaultAgency = agency != null ? agency.NameOfInstitution : "";
            }

            if (_sessionInfo.CurrentAgencyId.HasValue)
            {
                var impersonateAgencyPrivilege = "EditCredential";
                var dpiAgencyKey = Constants.DPI;

                var privileges = wamsUser.GetPrivileges(_sessionInfo.CurrentAgencyId.GetValueOrDefault());
                filterContext.Controller.ViewBag.Privileges = privileges;
                filterContext.Controller.ViewBag.MultipleAgency = agencies.Count() > 1 || wamsUser.HasPrivilege(dpiAgencyKey, impersonateAgencyPrivilege);

                filterContext.Controller.ViewData[Constants.CAN_USER_VIEW_REPORT] = privileges.Contains(WisePrivilege.ViewReport);
                filterContext.Controller.ViewData[Constants.CAN_USER_EDIT_CREDENTIAL] = privileges.Contains(WisePrivilege.EditCredential);
                filterContext.Controller.ViewData[Constants.CAN_USER_EDIT_VENDOR_SUBSCRIPTION] = privileges.Contains(WisePrivilege.EditVendorSubscription);
                filterContext.Controller.ViewData[Constants.CAN_USER_VIEW_CREDENTIALS] = privileges.Contains(WisePrivilege.ViewCredentials);

            }
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
        }
    }


}