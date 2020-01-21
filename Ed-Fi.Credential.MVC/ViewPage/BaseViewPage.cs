using System.Web.Mvc;

namespace Ed_Fi.Credential.MVC.ViewPage
{
    public class BaseViewPage<TModel> : WebViewPage<TModel>
    {
        public IBaseViewPageContext ViewPageContext { get; private set; }

        protected override void InitializePage()
        {
            base.InitializePage();
            ViewPageContext = ViewData[BaseViewPageContext.BASE_VIEW_PAGE_CONTEXT] as IBaseViewPageContext;
        }

        public override void Execute()
        {
        }
    }

    public class BaseViewPage : WebViewPage
    {
        public IBaseViewPageContext ViewPageContext { get; private set; }

        protected override void InitializePage()
        {
            base.InitializePage();
            ViewPageContext = ViewData[BaseViewPageContext.BASE_VIEW_PAGE_CONTEXT] as IBaseViewPageContext;
        }

        public override void Execute()
        {
        }
    }
}