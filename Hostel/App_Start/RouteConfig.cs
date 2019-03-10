using System.Web.Mvc;
using System.Web.Routing;

namespace Hostel.App_Start
{
    public class RouteConfig
    {
        public static void Configure(RouteCollection routes)
        {
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}/{op}/{oop}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional, op = UrlParameter.Optional, oop = UrlParameter.Optional }
            );
        }
    }
}