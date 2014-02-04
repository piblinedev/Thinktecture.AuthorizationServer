/*
 * Copyright (c) Dominick Baier, Brock Allen.  All rights reserved.
 * see license.txt
 */

using System.IdentityModel.Services;
using System.IdentityModel.Services.Configuration;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Thinktecture.AuthorizationServer.Interfaces;
using Thinktecture.IdentityModel.Tokens;

namespace Thinktecture.AuthorizationServer.WebHost
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // don't let the JWT handler change claim types
            JwtSecurityTokenHandler.InboundClaimTypeMap = ClaimMappings.None;
            JwtSecurityTokenHandler.OutboundClaimTypeMap = ClaimMappings.None;

            //GlobalConfiguration.Configuration.EnableSystemDiagnosticsTracing();
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            AutofacConfig.Configure();
            DataProtectionConfig.Configure();

            AntiForgeryConfig.UniqueClaimTypeIdentifier = Constants.ClaimTypes.Subject;
            FederatedAuthentication.FederationConfigurationCreated += FederatedAuthentication_FederationConfigurationCreated;

            //Remove the default FormUrlEncoded handler the JqueryMvc version is subclassed from the default one.
            var formatter = GlobalConfiguration.Configuration.Formatters.FirstOrDefault(x => x.GetType().Name.Contains("FormUrlEncodedMediaTypeFormatter"));
            if (null != formatter)
                GlobalConfiguration.Configuration.Formatters.Remove(formatter);
        }

        private void FederatedAuthentication_FederationConfigurationCreated(object sender, FederationConfigurationCreatedEventArgs e)
        {
            var svc = DependencyResolver.Current.GetService<IAuthorizationServerAdministratorsService>();

            e.FederationConfiguration.IdentityConfiguration.ClaimsAuthenticationManager = new SubjectClaimsTransformer(svc);
            e.FederationConfiguration.IdentityConfiguration.ClaimsAuthorizationManager = new AuthorizationManager();
        }
    }
}