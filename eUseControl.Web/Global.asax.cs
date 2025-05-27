using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Data.Entity;
using eUseControl.BusinessLogic.Interfaces;
using eUseControl.BusinessLogic.Services;
using eUseControl.Data.Context;
using eUseControl.Data.Initializer;
using eUseControl.Data.Repository;
using eUseControl.Domain.Interfaces;
using eUseControl.Web;
using Unity;
using Unity.AspNet.Mvc;

namespace eUseControl.Web
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Inițializare bază de date
            Database.SetInitializer(new DbInitializer());

            // Inițializare MVC
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
           
            var container = new UnityContainer();

            // Înregistrarea implementărilor concrete pentru interfețe:
            container.RegisterType<IAccessoryRepository, AccessoryRepository>();
            container.RegisterType<IAccessoryService, AccessoryService>();
            // Alte înregistrări:
            // container.RegisterType<IOtherService, OtherService>();
            container.RegisterType<IProductRepository, ProductRepository>();
            container.RegisterType<IProductService, ProductService>();
            
            container.RegisterType<IFavoriteRepository, FavoriteRepository>();
            container.RegisterType<IFavoriteService, FavoriteService>();
            
            container.RegisterType<IShopRepository, ShopRepository>();
            container.RegisterType<IShopService, ShopService>();
            
            container.RegisterType<IUserRepository, UserRepository>();
            container.RegisterType<IUserService, UserService>();
            
            // Add these registrations in your Application_Start method
            container.RegisterType<IContactRepository, ContactRepository>();
            container.RegisterType<IContactService, ContactService>();
            
            container.RegisterType<IOrderRepository, OrderRepository>();
            container.RegisterType<IOrderService, OrderService>();
            
            // Register MedicalConsultation dependencies
            container.RegisterType<IMedicalConsultationRepository, MedicalConsultationRepository>();
            container.RegisterType<IMedicalConsultationService, MedicalConsultationService>();
            
            // Register OurTeam dependencies
            container.RegisterType<ITeamMemberRepository, TeamMemberRepository>();
            container.RegisterType<ITeamService, TeamService>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));

        }
    }
}