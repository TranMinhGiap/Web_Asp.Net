using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace YT1
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //
            routes.MapRoute(
                name: "CategoryProduct",
                url: "san-pham",
                defaults: new { controller = "Products", action = "Index", alias = UrlParameter.Optional },
                namespaces: new[] { "YT1.Controllers" }
            );
            routes.MapRoute(
                name: "Home",
                url: "trang-chu",
                defaults: new { controller = "Home", action = "Index", alias = UrlParameter.Optional },
                namespaces: new[] { "YT1.Controllers" }
            );
            routes.MapRoute(
                name: "PaginationProduct",
                url: "danh-muc-san-pham/{id}",
                defaults: new { controller = "Products", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "YT1.Controllers" }
            );
            routes.MapRoute(
                name: "DetailProduct",
                url: "chi-tiet/{alias}/{id}",
                defaults: new { controller = "Products", action = "Details", id = UrlParameter.Optional },
                namespaces: new[] { "YT1.Controllers" }
            );
            routes.MapRoute(
                name: "DetailsNews",
                url: "tin-tuc/d-{id}",
                defaults: new { controller = "News", action = "Details", id = UrlParameter.Optional },
                namespaces: new[] { "YT1.Controllers" }
            );
            routes.MapRoute(
                name: "News",
                url: "tin-tuc/{id}",
                defaults: new { controller = "News", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "YT1.Controllers" }
            );
            routes.MapRoute(
                name: "DetailsPromotion",
                url: "khuyen-mai/km-{id}",
                defaults: new { controller = "Promotion", action = "Details", id = UrlParameter.Optional },
                namespaces: new[] { "YT1.Controllers" }
            );
            routes.MapRoute(
                name: "Promotion",
                url: "khuyen-mai",
                defaults: new { controller = "Promotion", action = "Index" },
                namespaces: new[] { "YT1.Controllers" }
            );
            routes.MapRoute(
                name: "DetailsContact",
                url: "lien-he/lh-{id}",
                defaults: new { controller = "Contact", action = "Details", id = UrlParameter.Optional },
                namespaces: new[] { "YT1.Controllers" }
            );
            routes.MapRoute(
                name: "Contact",
                url: "lien-he",
                defaults: new { controller = "Contact", action = "Index" },
                namespaces: new[] { "YT1.Controllers" }
            );
            routes.MapRoute(
                name: "DetailsIntroduce",
                url: "gioi-thieu/gt-{id}",
                defaults: new { controller = "Introduce", action = "Details", id = UrlParameter.Optional },
                namespaces: new[] { "YT1.Controllers" }
            );
            routes.MapRoute(
                name: "Introduce",
                url: "gioi-thieu",
                defaults: new { controller = "Introduce", action = "Index" },
                namespaces: new[] { "YT1.Controllers" }
            );
            //

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] {"YT1.Controllers"}
            );
        }
    }
}
