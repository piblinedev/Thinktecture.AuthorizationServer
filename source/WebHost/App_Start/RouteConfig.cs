/*
 * Copyright (c) Dominick Baier, Brock Allen.  All rights reserved.
 * see license.txt
 */

using System.Web.Mvc;
using System.Web.Routing;

namespace Thinktecture.AuthorizationServer.WebHost
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("OAuth2 Authorize Endpoint", "{appName}/oauth/authorize", new
                {
                    controller = "Authorize",
                    action = "Index"
                });

            routes.MapRoute("Signout", "Signout", new {controller = "Account", action = "SignOut"},
                            new[] {"Thinktecture.AuthorizationServer.WebHost.Controllers"});

            routes.MapRoute("Home", "{action}", new {controller = "Home", action = "Index"},
                            new[] {"Thinktecture.AuthorizationServer.WebHost.Controllers"});
        }
    }
}