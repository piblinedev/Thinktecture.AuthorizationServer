﻿/*
 * Copyright (c) Dominick Baier, Brock Allen.  All rights reserved.
 * see license.txt
 */

using System;
using System.IdentityModel.Selectors;
using System.IdentityModel.Services;
using System.IdentityModel.Tokens;
using System.Security.Claims;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Security;
using Thinktecture.AuthorizationServer.Interfaces;
using Thinktecture.IdentityModel.Extensions;
using Thinktecture.IdentityModel.WSTrust;

namespace Thinktecture.AuthorizationServer.OAuth2
{
    public class WSTrustResourceOwnerCredentialValidation : IResourceOwnerCredentialValidation
    {
        readonly string _address;
        readonly string _realm;
        readonly string _issuerThumbprint;

        public WSTrustResourceOwnerCredentialValidation(ICredentialAuthorizationResource credentialAuthorization)
        {
            _address = credentialAuthorization.Address;
            _realm = credentialAuthorization.Realm;
            _issuerThumbprint = credentialAuthorization.IssuerThumbprint;
        }

        public ClaimsPrincipal Validate(string userName, string password)
        {
            var binding = new UserNameWSTrustBinding(SecurityMode.TransportWithMessageCredential);
            var credentials = new ClientCredentials();
            credentials.UserName.UserName = userName;
            credentials.UserName.Password = password;
            GenericXmlSecurityToken genericToken;

            Tracing.Start("ResourceOwner validation started");
            Tracing.InformationFormat("ResourceOwner validation Address:{0} Realm:{1} Thumbprint:{2}", _address, _realm,_issuerThumbprint);
            try
            {
                genericToken = WSTrustClient.Issue(
                    new EndpointAddress(_address),
                    new EndpointAddress(_realm),
                    binding,
                    credentials) as GenericXmlSecurityToken;
            }

            catch (MessageSecurityException ex)
            {
                Tracing.Error("WSTrustResourceOwnerCredentialValidation failed: " + ex);
                return null;
            }

            var config = new SecurityTokenHandlerConfiguration();
            config.AudienceRestriction.AllowedAudienceUris.Add(new Uri(_realm));

            config.CertificateValidationMode = X509CertificateValidationMode.None;
            config.CertificateValidator = X509CertificateValidator.None;

            var registry = new ConfigurationBasedIssuerNameRegistry();
            registry.AddTrustedIssuer(_issuerThumbprint, _address);
            config.IssuerNameRegistry = registry;

            var handler = SecurityTokenHandlerCollection.CreateDefaultSecurityTokenHandlerCollection(config);

            var token = genericToken.ToSecurityToken();
            var principal = new ClaimsPrincipal(handler.ValidateToken(token));

            Tracing.Information("Successfully requested token for user via WS-Trust");
            Tracing.InformationFormat("Token received for user via WS-Trust {0}", token);
            Tracing.Stop("ResourceOwner validation finished");
            
            return FederatedAuthentication.FederationConfiguration.IdentityConfiguration.ClaimsAuthenticationManager
                                       .Authenticate("ResourceOwnerPasswordValidation", principal);
        }
    }
}
