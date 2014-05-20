using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BadmintonBuddy
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Badminton", action = "Home", id = UrlParameter.Optional }
            //);

            routes.MapRoute(
                name: "Default",
                url: "{action}/{name}/{id}",
                defaults: new { controller = "Badminton", action = "Home", name = UrlParameter.Optional, id = UrlParameter.Optional }
            );
        }
    }
}