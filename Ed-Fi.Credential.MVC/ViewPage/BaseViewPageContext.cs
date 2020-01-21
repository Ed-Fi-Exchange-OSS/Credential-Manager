using Ed_Fi.Credential.MVC.ImplementationSpecific;

namespace Ed_Fi.Credential.MVC.ViewPage
{
    public interface IBaseViewPageContext
    {
        WamsPrincipal WamsUser { get; }
        ISessionInfo SessionInfo { get; }
    }

    public class BaseViewPageContext : IBaseViewPageContext
    {
        public const string BASE_VIEW_PAGE_CONTEXT = "BaseViewPageContext";

        public WamsPrincipal WamsUser { get { return SessionInfo.User; } }
        public ISessionInfo SessionInfo { get; private set; }

        public BaseViewPageContext(ISessionInfo sessionInfo)
        {
            SessionInfo = sessionInfo;
        }
    }
}