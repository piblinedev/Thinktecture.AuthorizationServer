/*
 * Copyright (c) Dominick Baier, Brock Allen.  All rights reserved.
 * see license.txt
 */

using Newtonsoft.Json.Serialization;
using System.Web.Http;
using Thinktecture.IdentityModel.Tokens.Http;

namespace Thinktecture.AuthorizationServer.WebHost
{
    internal static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute("OAuth2 Token Endpoint", "{appName}/oauth/token", new {Controller = "Token"},
                                       new {appName = "^[a-zA-Z0-9]+$"},
                                       new AuthenticationHandler(CreateClientAuthConfig(), config));

            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }

        public static AuthenticationConfiguration CreateClientAuthConfig()
        {
            var authConfig = new AuthenticationConfiguration
                {
                    InheritHostClientIdentity = false,
#if DEBUG
                    RequireSsl = false
#endif
                };

            // accept arbitrary credentials on basic auth header,
            // validation will be done in the protocol endpoint
            authConfig.AddBasicAuthentication((id, secret) => true, retainPassword: true);
            return authConfig;
        }
    }
}