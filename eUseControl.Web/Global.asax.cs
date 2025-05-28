using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Data.Entity;
using Unity;
using Unity.AspNet.Mvc;

using eUseControl.BusinessLogic.Interfaces;
using eUseControl.BusinessLogic.Services;
using eUseControl.Data.Context;
using eUseControl.Data.Initializer;
using eUseControl.Data.Repository;
using eUseControl.Domain.Interfaces;

namespace eUseControl.Web
{
    public class Global : HttpApplication
    {
        protected void Application_Start()
        {
            // Inițializare bază de date
            Database.SetInitializer(new DbInitializer());

            // Inițializare MVC
            AreaRegistration.RegisterAllAreas();
           
            RouteConfig.RegisterRoutes(RouteTable.Routes);
      

            // Unity container setup
            var container = new UnityContainer();

            container.RegisterType<IAccessoryRepository, AccessoryRepository>();
            container.RegisterType<IAccessoryService, AccessoryService>();

            container.RegisterType<IProductRepository, ProductRepository>();
            container.RegisterType<IProductService, ProductService>();

            container.RegisterType<IFavoriteRepository, FavoriteRepository>();
            container.RegisterType<IFavoriteService, FavoriteService>();

            container.RegisterType<IShopRepository, ShopRepository>();
            container.RegisterType<IShopService, ShopService>();

            container.RegisterType<IUserRepository, UserRepository>();
            container.RegisterType<IUserService, UserService>();
            container.RegisterType<IAuthenticationService, AuthenticationService>();

            container.RegisterType<IContactRepository, ContactRepository>();
            container.RegisterType<IContactService, ContactService>();

            container.RegisterType<IOrderRepository, OrderRepository>();
            container.RegisterType<IOrderService, OrderService>();

            container.RegisterType<IMedicalConsultationRepository, MedicalConsultationRepository>();
            container.RegisterType<IMedicalConsultationService, MedicalConsultationService>();

            container.RegisterType<ITeamMemberRepository, TeamMemberRepository>();
            container.RegisterType<ITeamService, TeamService>();

            // Injectăm containerul Unity în MVC
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}
