using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebTH
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Contact",
                url: "lien-he",
                defaults: new { controller = "Contact", action = "Index", alias = UrlParameter.Optional },
                namespaces: new[] { "WebTH.Controllers" }
            );

            routes.MapRoute(
                name: "DetailAdvise",
                url: "{alias}-n{id}",
                defaults: new { controller = "Advise", action = "Detail", id = UrlParameter.Optional },
                namespaces: new[] { "WebTH.Controllers" }
            );

            routes.MapRoute(
                name: "Advise",
                url: "tu-van",
                defaults: new { controller = "Advise", action = "Index", alias = UrlParameter.Optional },
                namespaces: new[] { "WebTH.Controllers" }
            );

            routes.MapRoute(
                name: "ProductAdv",
                url: "san-pham",
                defaults: new { controller = "ProductAdv", action = "Index", alias = UrlParameter.Optional },
                namespaces: new[] { "WebTH.Controllers" }
            );

            routes.MapRoute(
                name: "About",
                url: "cau-chuyen-that-th",
                defaults: new { controller = "About", action = "Index", alias = UrlParameter.Optional },
                namespaces: new[] { "WebTH.Controllers" }
            );

            routes.MapRoute(
                name: "CheckOut",
                url: "thanh-toan",
                defaults: new { controller = "ShoppingCart", action = "CheckOut", alias = UrlParameter.Optional },
                namespaces: new[] { "WebTH.Controllers" }
            );

            routes.MapRoute(
name: "vnpay_return",
url: "vnpay_return",
defaults: new { controller = "ShoppingCart", action = "VnpayReturn", alias = UrlParameter.Optional },
namespaces: new[] { "WebTH.Controllers" }
);

            routes.MapRoute(
                name: "ShoppingCart",
                url: "gio-hang",
                defaults: new { controller = "ShoppingCart", action = "Index", alias = UrlParameter.Optional },
                namespaces: new[] { "WebTH.Controllers" }
            );

            routes.MapRoute(
                name: "DetailProduct",
                url: "chi-tiet/{alias}-p{id}",
                defaults: new { controller = "Products", action = "Detail", alias = UrlParameter.Optional },
                namespaces: new[] { "WebTH.Controllers" }
            );

            routes.MapRoute(
                name: "Products",
                url: "cua-hang",
                defaults: new { controller = "Products", action = "Index", alias = UrlParameter.Optional },
                namespaces: new[] { "WebTH.Controllers" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "WebTH.Controllers" }
            );
        }
    }
}
