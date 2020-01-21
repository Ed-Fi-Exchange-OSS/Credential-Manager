using System;
using System.Configuration;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Ed_Fi.Credential.MVC
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            Version = Assembly.GetExecutingAssembly().GetName().Version;

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
  
        }

        public static Version Version { get; set; }
    }
}
