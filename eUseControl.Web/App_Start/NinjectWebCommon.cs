using System;
using System.Web;
using eUseControl.BusinessLogic.Interfaces;
using eUseControl.BusinessLogic.Services;
using eUseControl.Domain.Interfaces;
using eUseControl.Data.Repository;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Ninject;
using Ninject.Web.Common;
using Ninject.Web.Mvc;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(eUseControl.Web.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(eUseControl.Web.App_Start.NinjectWebCommon), "Stop")]

namespace eUseControl.Web.App_Start
{
    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }

        public static void Stop()
        {
            bootstrapper.ShutDown();
        }

        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();

            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);

                // Set Ninject as the default dependency resolver
                DependencyResolver.SetResolver(new NinjectDependencyResolver(kernel));

                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Register your services here so Ninject can inject them
        /// </summary>
        /// <param name="kernel"></param>
        private static void RegisterServices(IKernel kernel)
        {
            // Register your interfaces and implementations here
            kernel.Bind<IUserRepository>().To<UserRepository>();
            kernel.Bind<IUserService>().To<UserService>();
            // Add more bindings if needed
        }
    }
}
