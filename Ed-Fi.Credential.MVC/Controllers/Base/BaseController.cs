using Ed_Fi.Credential.MVC.ImplementationSpecific;
using Ed_Fi.Credential.MVC.ViewPage;
using RestSharp.Extensions;
using System.Web.Mvc;

namespace Ed_Fi.Credential.MVC.Controllers.Base
{
    public interface IBaseControllerContext
    {
        IBaseViewPageContext ViewPageContext { get; }
        ISessionInfo SessionInfo { get; }
    }
    public class BaseControllerContext : IBaseControllerContext
    {
        public IBaseViewPageContext ViewPageContext { get; private set; }
        public ISessionInfo SessionInfo { get; private set; }

        public BaseControllerContext(IBaseViewPageContext baseViewPageContext, ISessionInfo sessionInfo)
        {
            ViewPageContext = baseViewPageContext;
            SessionInfo = sessionInfo;
        }
    }

    public class BaseController : Controller
    {
        public WamsPrincipal WamsUser
        {
            get
            {
                return _baseControllerContext.SessionInfo.User;
            }
        }

        public int EducationOrganizationId
        {
            get { return _baseControllerContext.SessionInfo.CurrentAgencyId.GetValueOrDefault(); }
        }

        private readonly IBaseControllerContext _baseControllerContext;

        public BaseController(IBaseControllerContext controllerContext)
        {
            _baseControllerContext = controllerContext;

            ViewData.Add(BaseViewPageContext.BASE_VIEW_PAGE_CONTEXT, _baseControllerContext.ViewPageContext);
            
        }

        public string Error { get; set; }

        public bool HasErrors {
            get { return Error.HasValue(); }
        }


        public bool CanUserViewReport => (bool)ViewData[Constants.CAN_USER_VIEW_REPORT];
        public bool CanUserEditCredential => (bool)ViewData[Constants.CAN_USER_EDIT_CREDENTIAL];
        public bool CanUserEditVendorSubscription => (bool)ViewData[Constants.CAN_USER_EDIT_VENDOR_SUBSCRIPTION];
        public bool CanUserViewCredentials => (bool)ViewData[Constants.CAN_USER_VIEW_CREDENTIALS];
    }
}