using System.Web.Mvc;
using Unity;
using eUseControl.Domain.Interfaces;
using eUseControl.Data.Repository;
using Unity.AspNet.Mvc;

public static class UnityConfig
{
    public static void RegisterComponents()
    {
        var container = new UnityContainer();

        // Înregistrarea implementărilor concrete pentru interfețe:
        container.RegisterType<IAccessoryRepository, AccessoryRepository>();
        // Alte înregistrări:
        // container.RegisterType<IOtherService, OtherService>();

        DependencyResolver.SetResolver(new UnityDependencyResolver(container));
    }
}