﻿using System.Web.Mvc;
using System.Web.Routing;

namespace FinalTask.WebUI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Admin",
                url: "{area}/{controller}/{action}/{id}",
                defaults: new { area = "Admin", controller = "Admin", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
