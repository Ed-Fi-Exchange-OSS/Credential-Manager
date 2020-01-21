using Ed_Fi.Credential.Business;
using Ed_Fi.Credential.Domain.Model;
using Ed_Fi.Credential.Domain.Ods;
using Ed_Fi.Credential.MVC.Controllers.Base;
using Ed_Fi.Credential.MVC.ImplementationSpecific;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Ninject;
using Ninject.Extensions.Conventions;
using Ninject.Web.Common;
using Ninject.Web.Common.WebHost;
using Ninject.Web.Mvc.FilterBindingSyntax;
using System;
using System.Web;
using System.Web.Mvc;
using Ed_Fi.Credential.MVC.Ninject;
using WebActivatorEx;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(NinjectWebCommon), "Start")]
[assembly: ApplicationShutdownMethod(typeof(NinjectWebCommon), "Stop")]


namespace Ed_Fi.Credential.MVC.Ninject
{
    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper BOOTSTRAPPER = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            BOOTSTRAPPER.Initialize(CreateKernel);
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            BOOTSTRAPPER.ShutDown();
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
           kernel.BindFilter<DebugWamsAuthenticationFilter>(FilterScope.First, 0);
           

            kernel.BindFilter<CurrentAgencyFilter>(FilterScope.Global, 1);

            kernel.Bind(x => x.FromAssemblyContaining(
                typeof(BaseController),
                typeof(BaseBusiness<>),
                typeof(AgencyBusiness)
                )
                .SelectAllClasses()
                .BindDefaultInterface());

            kernel.Rebind<IEdFiAdminDbContext>().To<EdFiAdminDbContext>().InRequestScope();
            kernel.Rebind<IOdsDbContext>().To<OdsDbContext>().InRequestScope();
            kernel.Rebind<IEdFiSecurityDbContext>().To<EdFiSecurityDbContext>().InRequestScope();
            kernel.Rebind<ISessionInfo>().To<SessionInfo>().InRequestScope();
        }
    }
}