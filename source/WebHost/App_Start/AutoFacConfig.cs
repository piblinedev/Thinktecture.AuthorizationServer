/*
 * Copyright (c) Dominick Baier, Brock Allen.  All rights reserved.
 * see license.txt
 */

using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using System.Web.Http;
using System.Web.Mvc;
using Thinktecture.AuthorizationServer.EF;
using Thinktecture.AuthorizationServer.Interfaces;
using Thinktecture.AuthorizationServer.OAuth2;
using Thinktecture.AuthorizationServer.WebHost.Security;

namespace Thinktecture.AuthorizationServer.WebHost
{
    internal class AutofacConfig
    {
        public static void Configure()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<EFStoredGrantManager>().As<IStoredGrantManager>();
            builder.RegisterType<EFAuthorizationServerConfiguration>().As<IAuthorizationServerConfiguration>();
            builder.RegisterType<EFAuthorizationServerAdministration>().As<IAuthorizationServerAdministration>();
            builder.RegisterType<EFAuthorizationServerAdministratorsService>()
                   .As<IAuthorizationServerAdministratorsService>();

            //builder.RegisterType<Thinktecture.Samples.AssertionGrantValidator>().As<IAssertionGrantValidation>();

            builder.RegisterType<DefaultAssertionGrantValidator>().As<IAssertionGrantValidation>();
            builder.RegisterType<AuthorizationServerContext>().InstancePerHttpRequest();

            builder.Register(x => CreateCredentialAuthorizationResource()).As<ICredentialAuthorizationResource>();
            builder.RegisterType<WSTrustResourceOwnerCredentialValidation>().As<IResourceOwnerCredentialValidation>();

            builder.RegisterModule(new AutofacWebTypesModule());

            builder.RegisterControllers(typeof (AuthorizeController).Assembly);
            builder.RegisterControllers(typeof (AutofacConfig).Assembly);
            builder.RegisterApiControllers(typeof (TokenController).Assembly);
            builder.RegisterApiControllers(typeof (AutofacConfig).Assembly);

            var container = builder.Build();

            // MVC
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            // Web API
            var resolver = new AutofacWebApiDependencyResolver(container);
            GlobalConfiguration.Configuration.DependencyResolver = resolver;
        }

        private static ICredentialAuthorizationResource CreateCredentialAuthorizationResource()
        {
            return new DefaultOAuth2CredentialAuthorizationResource
                {
                    Address = Settings.CredentialResourceAddress.Trim(),
                    Realm = Settings.CredentialResourceRealm.Trim(),
                    IssuerThumbprint = Settings.CredentialResourceThumbprint.Trim()
                };
        }
    }
}