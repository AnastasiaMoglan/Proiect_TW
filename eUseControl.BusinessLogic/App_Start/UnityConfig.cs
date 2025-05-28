using System.Web.Mvc;
using Unity;


using eUseControl.BusinessLogic.Interfaces;
using eUseControl.BusinessLogic.Services;
using eUseControl.Domain.Interfaces;
using eUseControl.Data.Repository;
using Unity.AspNet.Mvc;

namespace eUseControl.Web
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();

            // Înregistrează implementările concrete pentru interfețe
            container.RegisterType<IUserRepository, UserRepository>();
            container.RegisterType<IAuthenticationService, AuthenticationService>();
            container.RegisterType<IUserService, UserService>();

            // Configurează MVC să folosească Unity
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}