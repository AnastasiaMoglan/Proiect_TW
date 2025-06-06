﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace eUseControl.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // Accessories Category Route
            routes.MapRoute(
                name: "AccessoriesCategory",
                url: "Accessories/Category/{id}",
                defaults: new { controller = "Accessories", action = "Category" }
            );

            // Accessories Detail Route
            routes.MapRoute(
                name: "AccessoriesDetail",
                url: "Accessories/Detail/{id}",
                defaults: new { controller = "Accessories", action = "Detail" }
            );

            // Default Route
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
