using System.Collections.Generic;
using System.Web.Mvc.Filters;

namespace Ed_Fi.Credential.MVC.ImplementationSpecific
{
    public class DebugWamsAuthenticationFilter : IAuthenticationFilter
    {
        private readonly ISessionInfo _sessionInfo;

        public DebugWamsAuthenticationFilter(ISessionInfo sessionInfo)
        {
            _sessionInfo = sessionInfo;
        }

        public virtual void OnAuthentication(AuthenticationContext filterContext)
        {
            var roles = new List<WamsRole>();

            roles.Add(new WamsRole
            {
                EducationOrganizationId = Constants.DPI,
                AgencyName = "DPI",
                Role = "AllFunctions",
                Privileges = new List<WisePrivilege> { WisePrivilege.EditAgreement, WisePrivilege.EditCredential, WisePrivilege.EditVendorSubscription, WisePrivilege.ViewCredentials, WisePrivilege.ViewReport, WisePrivilege.ViewMonitoring }
            });
            roles.Add(new WamsRole
            {
                EducationOrganizationId = Constants.DPI,
                AgencyName = "DPI",
                Role = "Vendor",
                Privileges = new List<WisePrivilege> { WisePrivilege.ViewCredentials, WisePrivilege.ViewMonitoring }
            });
            roles.Add(new WamsRole
            {
                EducationOrganizationId = 2,
                AgencyName = "Test Agency 2",
                Role = "Agency",
                Privileges = new List<WisePrivilege> { WisePrivilege.EditAgreement, WisePrivilege.EditVendorSubscription }
            });
            var wamsPrincipal = new WamsPrincipal("1234567890", "Demo", "User", "dev@dpi.wi.gov", roles);

            _sessionInfo.User = wamsPrincipal;

            _sessionInfo.SetCurrentAgency(Constants.DPI);

            filterContext.HttpContext.User = wamsPrincipal;
            filterContext.Controller.ViewBag.User = wamsPrincipal;
        }

        public virtual void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
        }
    }
}