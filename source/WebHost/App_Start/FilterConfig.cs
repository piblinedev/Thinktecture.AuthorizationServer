/*
 * Copyright (c) Dominick Baier, Brock Allen.  All rights reserved.
 * see license.txt
 */

using System.Web.Mvc;
using Thinktecture.AuthorizationServer.WebHost.Security;
using Thinktecture.IdentityModel.Web.Mvc;

namespace Thinktecture.AuthorizationServer.WebHost
{
    internal class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new FrameOptionsAttribute(FrameOptions.Deny));
            filters.Add(new DataProtectionConfigurationFilter());
        }
    }
}